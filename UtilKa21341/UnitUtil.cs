using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using LOR_DiceSystem;
using LOR_XML;
using ModJam_Ka21341.BLL;
using ModJam_Ka21341.Buffs;
using TMPro;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ModJam_Ka21341.UtilKa21341
{
    public static class UnitUtil
    {
        public static void InitKeywords(Assembly assembly)
        {
            if (typeof(BattleCardAbilityDescXmlList).GetField("_dictionaryKeywordCache", AccessTools.all)
                    ?.GetValue(BattleCardAbilityDescXmlList.Instance) is Dictionary<string, List<string>> dictionary)
                assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(DiceCardSelfAbilityBase))
                                               && x.Name.StartsWith("DiceCardSelfAbility_"))
                    .Do(x => dictionary[x.Name.Replace("DiceCardSelfAbility_", "")] =
                        new List<string>(((DiceCardSelfAbilityBase)Activator.CreateInstance(x)).Keywords));
        }

        private static void SetBaseKeywordCard(LorId id, ref Dictionary<LorId, DiceCardXmlInfo> cardDictionary,
            ref List<DiceCardXmlInfo> cardXmlList)
        {
            var keywordsList = GetKeywordsList(id).ToList();
            var diceCardXmlInfo2 = CardOptionChange(cardDictionary[id], new List<CardOption>(), true, keywordsList);
            cardDictionary[id] = diceCardXmlInfo2;
            cardXmlList.Add(diceCardXmlInfo2);
        }

        private static void SetCustomCardOption(CardOption option, LorId id, bool keywordsRequired,
            ref Dictionary<LorId, DiceCardXmlInfo> cardDictionary, ref List<DiceCardXmlInfo> cardXmlList)
        {
            var keywordsList = new List<string>();
            if (keywordsRequired) keywordsList = GetKeywordsList(id).ToList();
            var diceCardXmlInfo2 = CardOptionChange(cardDictionary[id], new List<CardOption> { option },
                keywordsRequired,
                keywordsList);
            cardDictionary[id] = diceCardXmlInfo2;
            cardXmlList.Add(diceCardXmlInfo2);
        }

        private static List<LorId> GetAllOnlyCardsId()
        {
            var onlyPageCardList = new List<LorId>();
            foreach (var cardIds in ModParameters.OnlyCardKeywords.Select(x => x.Item2))
                onlyPageCardList.AddRange(cardIds);
            return onlyPageCardList;
        }

        private static IEnumerable<string> GetKeywordsList(LorId id)
        {
            var keywords = ModParameters.OnlyCardKeywords.FirstOrDefault(x => x.Item2.Contains(id))?.Item1;
            var defaultKeyword = ModParameters.DefaultKeyword.FirstOrDefault(x => x.Key == id.packageId);
            var stringList = new List<string> { defaultKeyword.Value };
            if (keywords != null && keywords.Any())
                stringList.AddRange(keywords);
            return stringList;
        }

        private static DiceCardXmlInfo CardOptionChange(DiceCardXmlInfo cardXml, List<CardOption> option,
            bool keywordRequired, List<string> keywords,
            string skinName = "", string mapName = "", int skinHeight = 0)
        {
            return new DiceCardXmlInfo(cardXml.id)
            {
                workshopID = cardXml.workshopID,
                workshopName = cardXml.workshopName,
                Artwork = cardXml.Artwork,
                Chapter = cardXml.Chapter,
                category = cardXml.category,
                DiceBehaviourList = cardXml.DiceBehaviourList,
                _textId = cardXml._textId,
                optionList = option.Any() ? option : cardXml.optionList,
                Priority = cardXml.Priority,
                Rarity = cardXml.Rarity,
                Script = cardXml.Script,
                ScriptDesc = cardXml.ScriptDesc,
                Spec = cardXml.Spec,
                SpecialEffect = cardXml.SpecialEffect,
                SkinChange = string.IsNullOrEmpty(skinName) ? cardXml.SkinChange : skinName,
                SkinChangeType = cardXml.SkinChangeType,
                SkinHeight = skinHeight != 0 ? skinHeight : cardXml.SkinHeight,
                MapChange = string.IsNullOrEmpty(mapName) ? cardXml.MapChange : mapName,
                PriorityScript = cardXml.PriorityScript,
                Keywords = keywordRequired ? keywords : cardXml.Keywords
            };
        }

        public static void ChangeCardItem(ItemXmlDataList instance, string packageId)
        {
            try
            {
                var dictionary = (Dictionary<LorId, DiceCardXmlInfo>)instance.GetType()
                    .GetField("_cardInfoTable", AccessTools.all).GetValue(instance);
                var list = (List<DiceCardXmlInfo>)instance.GetType()
                    .GetField("_cardInfoList", AccessTools.all).GetValue(instance);
                var onlyPageCardList = GetAllOnlyCardsId();
                foreach (var item in dictionary.Where(x => x.Key.packageId == packageId).ToList())
                {
                    if (ModParameters.PersonalCardList.Contains(item.Key))
                    {
                        SetCustomCardOption(CardOption.Personal, item.Key, false, ref dictionary, ref list);
                        continue;
                    }

                    if (ModParameters.EgoPersonalCardList.Contains(item.Key))
                    {
                        SetCustomCardOption(CardOption.EgoPersonal, item.Key, false, ref dictionary, ref list);
                        continue;
                    }

                    if (onlyPageCardList.Contains(item.Key))
                    {
                        SetCustomCardOption(CardOption.OnlyPage, item.Key, true, ref dictionary, ref list);
                        continue;
                    }

                    SetBaseKeywordCard(item.Key, ref dictionary, ref list);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("There was an error while changing the Cards values " + ex.Message);
            }
        }

        public static void ChangePassiveItem(string packageId)
        {
            foreach (var passive in Singleton<PassiveXmlList>.Instance.GetDataAll().Where(passive =>
                         passive.id.packageId == packageId &&
                         ModParameters.UntransferablePassives.Contains(passive.id)))
                passive.CanGivePassive = false;
        }

        public static void BattleAbDialog(BattleDialogUI instance, List<AbnormalityCardDialog> dialogs,
            AbColorType colorType)
        {
            var component = instance.GetComponent<CanvasGroup>();
            var dialog = dialogs[Random.Range(0, dialogs.Count)].dialog;
            var txtAbnormalityDlg = (TextMeshProUGUI)typeof(BattleDialogUI).GetField("_txtAbnormalityDlg",
                AccessTools.all)?.GetValue(instance);
            if (txtAbnormalityDlg != null)
            {
                txtAbnormalityDlg.text = dialog;
                if (colorType == AbColorType.Positive)
                {
                    txtAbnormalityDlg.fontMaterial.SetColor("_GlowColor",
                        SingletonBehavior<BattleManagerUI>.Instance.positiveCoinColor);
                    txtAbnormalityDlg.color = SingletonBehavior<BattleManagerUI>.Instance.positiveTextColor;
                }
                else
                {
                    txtAbnormalityDlg.fontMaterial.SetColor("_GlowColor",
                        SingletonBehavior<BattleManagerUI>.Instance.negativeCoinColor);
                    txtAbnormalityDlg.color = SingletonBehavior<BattleManagerUI>.Instance.negativeTextColor;
                }

                var canvas = (Canvas)typeof(BattleDialogUI).GetField("_canvas",
                    AccessTools.all)?.GetValue(instance);
                if (canvas != null) canvas.enabled = true;
                component.interactable = true;
                component.blocksRaycasts = true;
                txtAbnormalityDlg.GetComponent<AbnormalityDlgEffect>().Init();
            }

            var _ = (Coroutine)typeof(BattleDialogUI).GetField("_routine",
                AccessTools.all)?.GetValue(instance);
            var method = typeof(BattleDialogUI).GetMethod("AbnormalityDlgRoutine", AccessTools.all);
            if (method != null) instance.StartCoroutine(method.Invoke(instance, Array.Empty<object>()) as IEnumerator);
        }

        public static bool CheckSkinProjection(BattleUnitModel owner)
        {
            if (!string.IsNullOrEmpty(owner.UnitData.unitData.workshopSkin)) return true;
            if (owner.UnitData.unitData.bookItem == owner.UnitData.unitData.CustomBookItem) return false;
            owner.view.ChangeSkin(owner.UnitData.unitData.CustomBookItem.GetCharacterName());
            return true;
        }

        public static void UnitReviveAndRecovery(BattleUnitModel owner, int hp, bool recoverLight,
            bool skinChanged = false)
        {
            if (owner.IsDead())
            {
                owner.bufListDetail.GetActivatedBufList()
                    .RemoveAll(x => !x.CanRecoverHp(999) || !x.CanRecoverBreak(999));
                owner.Revive(hp);
                owner.moveDetail.ReturnToFormationByBlink(true);
                owner.view.EnableView(true);
                if (skinChanged)
                    CheckSkinProjection(owner);
                else
                    owner.view.CreateSkin();
            }
            else
            {
                owner.bufListDetail.GetActivatedBufList()
                    .RemoveAll(x => !x.CanRecoverHp(999) || !x.CanRecoverBreak(999));
                owner.RecoverHP(hp);
            }

            owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
            owner.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_sealTemp));
            owner.breakDetail.ResetGauge();
            owner.breakDetail.nextTurnBreak = false;
            owner.breakDetail.RecoverBreakLife(1, true);
            if (recoverLight) owner.cardSlotDetail.RecoverPlayPoint(owner.cardSlotDetail.GetMaxPlayPoint());
        }

        public static void RefreshCombatUI(bool forceReturn = false)
        {
            foreach (var (battleUnit, num) in BattleObjectManager.instance.GetList()
                         .Select((value, i) => (value, i)))
            {
                SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnit.UnitData.unitData, num,
                    true);
                if (forceReturn)
                    battleUnit.moveDetail.ReturnToFormationByBlink(true);
            }

            try
            {
                BattleObjectManager.instance.InitUI();
            }
            catch (IndexOutOfRangeException)
            {
                // ignored
            }
        }

        public static void RemoveImmortalBuff(BattleUnitModel owner)
        {
            if (owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_ImmortalUntilRoundEnd_Ka21341) is
                BattleUnitBuf_ImmortalUntilRoundEnd_Ka21341 buf)
                owner.bufListDetail.RemoveBuf(buf);
        }

        public static bool SpecialCaseEgo(Faction unitFaction, LorId passiveId, Type buffType)
        {
            var playerUnit = BattleObjectManager.instance
                .GetAliveList(ReturnOtherSideFaction(unitFaction)).FirstOrDefault(x =>
                    x.passiveDetail.PassiveList.Exists(y => y.id == passiveId));
            return playerUnit != null && playerUnit.bufListDetail.GetActivatedBufList()
                .Exists(x => !x.IsDestroyed() && x.GetType() == buffType);
        }

        public static Faction ReturnOtherSideFaction(Faction faction)
        {
            return faction == Faction.Player ? Faction.Enemy : Faction.Player;
        }
    }
}