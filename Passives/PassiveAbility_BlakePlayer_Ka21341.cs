using System.Collections.Generic;
using LOR_XML;
using ModJam_Ka21341.BLL;
using ModJam_Ka21341.BLL.MechUtilModels;
using ModJam_Ka21341.Buffs;
using ModJam_Ka21341.UtilKa21341;
using ModJam_Ka21341.UtilKa21341.MechUtil;

namespace ModJam_Ka21341.Passives
{
    public class PassiveAbility_BlakePlayer_Ka21341 : PassiveAbilityBase
    {
        private MechUtil _util;

        public override void OnBattleEnd()
        {
            if (_util.CheckSkinChangeIsActive())
                owner.UnitData.unitData.bookItem.ClassInfo.CharacterSkin = new List<string> { "BlakePlayer_Ka21341" };
        }

        public override void OnWaveStart()
        {
            _util = new MechUtil(new MechUtilModel
            {
                Owner = owner,
                HasEgo = true,
                RefreshUI = true,
                HasEgoAttack = true,
                SkinName = "BlakePlayerEgo_Ka21341",
                EgoType = typeof(BattleUnitBuf_BloodlustEgo_Ka21341),
                HasEgoAbDialog = true,
                EgoAbColorColor = AbColorType.Negative,
                EgoAbDialogList = new List<AbnormalityCardDialog>
                {
                    new AbnormalityCardDialog
                    {
                        id = "BlakeEnemyEgo",
                        dialog = "This unending thirst..."
                    }
                },
                EgoCardId = new LorId(ModParameters.PackageId, 1),
                EgoAttackCardId = new LorId(ModParameters.PackageId, 2)
            });
            if (UnitUtil.CheckSkinProjection(owner))
                _util.DoNotChangeSkinOnEgo();
            if (owner.faction != Faction.Enemy) return;
            if (UnitUtil.SpecialCaseEgo(owner.faction, new LorId(ModParameters.PackageId, 2),
                    typeof(BattleUnitBuf_BloodlustEgo_Ka21341))) _util.ForcedEgo();
        }

        public override void OnRoundStartAfter()
        {
            owner.personalEgoDetail.RemoveCard(new LorId(ModParameters.PackageId, 3));
            owner.personalEgoDetail.AddCard(new LorId(ModParameters.PackageId, 3));
        }

        public override void OnRoundStart()
        {
            if (!_util.EgoCheck()) return;
            _util.EgoActive();
        }

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            _util.OnUseExpireCard(curCard.card.GetID());
            //_util.ChangeToEgoMap(curCard.card.GetID());
        }

        //public override void OnRoundEndTheLast_ignoreDead()
        //{
        //    _util.ReturnFromEgoMap();
        //}
        public override void OnRoundEnd()
        {
            if (owner.faction != Faction.Enemy) return;
            if (UnitUtil.SpecialCaseEgo(owner.faction, new LorId(ModParameters.PackageId, 2),
                    typeof(BattleUnitBuf_BloodlustEgo_Ka21341))) _util.ForcedEgo();
        }
    }
}