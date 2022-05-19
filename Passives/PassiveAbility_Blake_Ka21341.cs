using System.Collections.Generic;
using System.Linq;
using LOR_XML;
using ModJam_Ka21341.BLL;
using ModJam_Ka21341.BLL.MechUtilModels;
using ModJam_Ka21341.Buffs;
using ModJam_Ka21341.UtilKa21341;
using ModJam_Ka21341.UtilKa21341.MechUtil;

namespace ModJam_Ka21341.Passives
{
    public class PassiveAbility_Blake_Ka21341 : PassiveAbilityBase
    {
        private NpcMechUtil _util;

        public override void OnBattleEnd()
        {
            owner.UnitData.unitData.bookItem.ClassInfo.CharacterSkin = new List<string> { "BlakeEnemy_Ka21341" };
            _util.OnEndBattle();
        }

        public override void OnWaveStart()
        {
            _util = new NpcMechUtil(new NpcMechUtilModel
            {
                Owner = owner,
                MechHp = 40,
                Counter = 0,
                MaxCounter = 4,
                HasEgo = true,
                HasMechOnHp = true,
                RefreshUI = true,
                SkinName = "BlakeEnemyEgo_Ka21341",
                EgoType = typeof(BattleUnitBuf_BloodlustEgo_Ka21341),
                HasEgoAbDialog = true,
                EgoAbColorColor = AbColorType.Negative,
                EgoAbDialogList = new List<AbnormalityCardDialog>
                {
                    new AbnormalityCardDialog
                    {
                        id = "BlakePlayerEgo",
                        dialog = "This unending thirst..."
                    }
                },
                LorIdEgoMassAttack = new LorId(ModParameters.PackageId, 4),
                EgoAttackCardId = new LorId(ModParameters.PackageId, 4)
            }, "BlakePhaseKa21341");
            _util.Restart();
            if (BattleObjectManager.instance.GetAliveList(owner.faction)
                    .Count(x => x.Book.BookId.packageId == ModParameters.PackageId) >
                1) owner.bufListDetail.AddBuf(new BattleUnitBuf_ImmortalUntilSolo_Ka21341());
        }

        public override void OnStartBattle()
        {
            UnitUtil.RemoveImmortalBuff(owner);
            if (BattleObjectManager.instance.GetAliveList(owner.faction)
                    .Count(x => x.Book.BookId.packageId == ModParameters.PackageId) > 1) return;
            var buff = owner.bufListDetail.GetActivatedBufList()
                .FirstOrDefault(x => x is BattleUnitBuf_ImmortalUntilSolo_Ka21341);
            if (buff != null) owner.bufListDetail.RemoveBuf(buff);
        }

        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            _util.MechHpCheck(dmg);
            return base.BeforeTakeDamage(attacker, dmg);
        }

        public override void OnRoundEndTheLast()
        {
            _util.CheckPhase();
        }

        public override void OnRoundStart()
        {
            if (!_util.EgoCheck()) return;
            _util.EgoActive();
        }

        public override void OnRoundEnd()
        {
            _util.ExhaustEgoAttackCards();
            _util.SetOneTurnCard(false);
        }

        //public override void OnDieOtherUnit(BattleUnitModel unit)
        //{
        //    if (_util.GetPhase() < 1) return;
        //    if (unit.faction == owner.faction) _util.SetCounter(4);
        //}

        public override BattleDiceCardModel OnSelectCardAuto(BattleDiceCardModel origin, int currentDiceSlotIdx)
        {
            _util.OnSelectCardPutMassAttack(ref origin);
            return base.OnSelectCardAuto(origin, currentDiceSlotIdx);
        }

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            _util.OnUseCardResetCount(curCard);
            //_util.ChangeToEgoMap(curCard.card.GetID());
        }

        //public override void OnRoundEndTheLast_ignoreDead()
        //{
        //    _util.ReturnFromEgoMap();
        //}
    }
}