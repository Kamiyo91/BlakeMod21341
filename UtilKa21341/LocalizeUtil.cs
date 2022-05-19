using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using HarmonyLib;
using LOR_XML;
using ModJam_Ka21341.BLL;
using UnityEngine;

namespace ModJam_Ka21341.UtilKa21341
{
    public static class LocalizeUtil
    {
        public static void AddLocalize(string packageId, string path)
        {
            FileInfo[] files;
            try
            {
                var dictionary =
                    typeof(BattleEffectTextsXmlList).GetField("_dictionary", AccessTools.all)
                            ?.GetValue(Singleton<BattleEffectTextsXmlList>.Instance) as
                        Dictionary<string, BattleEffectText>;
                files = new DirectoryInfo(path + "/Localize/" + ModParameters.Language + "/EffectTexts")
                    .GetFiles();
                foreach (var t in files)
                    using (var stringReader = new StringReader(File.ReadAllText(t.FullName)))
                    {
                        var battleEffectTextRoot =
                            (BattleEffectTextRoot)new XmlSerializer(typeof(BattleEffectTextRoot))
                                .Deserialize(stringReader);
                        foreach (var battleEffectText in battleEffectTextRoot.effectTextList)
                        {
                            dictionary.Remove(battleEffectText.ID);
                            dictionary?.Add(battleEffectText.ID, battleEffectText);
                        }
                    }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error loading Effect Texts packageId : " + packageId + " Language : " +
                               ModParameters.Language + " Error : " + ex.Message);
            }

            try
            {
                files = new DirectoryInfo(path + "/Localize/" + ModParameters.Language + "/Books").GetFiles();
                foreach (var t in files)
                    using (var stringReader4 = new StringReader(File.ReadAllText(t.FullName)))
                    {
                        var bookDescRoot =
                            (BookDescRoot)new XmlSerializer(typeof(BookDescRoot)).Deserialize(stringReader4);
                        using (var enumerator4 =
                               Singleton<BookXmlList>.Instance.GetAllWorkshopData()[packageId]
                                   .GetEnumerator())
                        {
                            while (enumerator4.MoveNext())
                            {
                                var bookXml = enumerator4.Current;
                                bookXml.InnerName = bookDescRoot.bookDescList.Find(x => x.bookID == bookXml.id.id)
                                    .bookName;
                            }
                        }

                        using (var enumerator5 = Singleton<BookXmlList>.Instance.GetList()
                                   .FindAll(x => x.id.packageId == packageId).GetEnumerator())
                        {
                            while (enumerator5.MoveNext())
                            {
                                var bookXml = enumerator5.Current;
                                bookXml.InnerName = bookDescRoot.bookDescList.Find(x => x.bookID == bookXml.id.id)
                                    .bookName;
                                Singleton<BookXmlList>.Instance.GetData(bookXml.id).InnerName = bookXml.InnerName;
                            }
                        }

                        (typeof(BookDescXmlList).GetField("_dictionaryWorkshop", AccessTools.all)
                                    .GetValue(Singleton<BookDescXmlList>.Instance) as
                                Dictionary<string, List<BookDesc>>)
                            [packageId] = bookDescRoot.bookDescList;
                    }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error loading Books Texts packageId : " + packageId + " Language : " +
                               ModParameters.Language + " Error : " + ex.Message);
            }

            try
            {
                files = new DirectoryInfo(path + "/Localize/" + ModParameters.Language + "/PassiveDesc")
                    .GetFiles();
                foreach (var t in files)
                    using (var stringReader7 = new StringReader(File.ReadAllText(t.FullName)))
                    {
                        var passiveDescRoot =
                            (PassiveDescRoot)new XmlSerializer(typeof(PassiveDescRoot)).Deserialize(stringReader7);
                        using (var enumerator9 = Singleton<PassiveXmlList>.Instance.GetDataAll()
                                   .FindAll(x => x.id.packageId == packageId).GetEnumerator())
                        {
                            while (enumerator9.MoveNext())
                            {
                                var passive = enumerator9.Current;
                                passive.name = passiveDescRoot.descList.Find(x => x.ID == passive.id.id).name;
                                passive.desc = passiveDescRoot.descList.Find(x => x.ID == passive.id.id).desc;
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error loading Passive Desc Texts packageId : " + packageId + " Language : " +
                               ModParameters.Language + " Error : " + ex.Message);
            }

            try
            {
                var cardAbilityDictionary = typeof(BattleCardAbilityDescXmlList)
                        .GetField("_dictionary", AccessTools.all)
                        ?.GetValue(Singleton<BattleCardAbilityDescXmlList>.Instance) as
                    Dictionary<string, BattleCardAbilityDesc>;
                files = new DirectoryInfo(path + "/Localize/" + ModParameters.Language +
                                          "/BattleCardAbilities").GetFiles();
                foreach (var t in files)
                    using (var stringReader8 = new StringReader(File.ReadAllText(t.FullName)))
                    {
                        foreach (var battleCardAbilityDesc in
                                 ((BattleCardAbilityDescRoot)new XmlSerializer(typeof(BattleCardAbilityDescRoot))
                                     .Deserialize(stringReader8)).cardDescList)
                        {
                            cardAbilityDictionary.Remove(battleCardAbilityDesc.id);
                            cardAbilityDictionary.Add(battleCardAbilityDesc.id, battleCardAbilityDesc);
                        }
                    }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error loading Battle Card Abilities Texts packageId : " + packageId + " Language : " +
                               ModParameters.Language + " Error : " + ex.Message);
            }
        }
    }
}