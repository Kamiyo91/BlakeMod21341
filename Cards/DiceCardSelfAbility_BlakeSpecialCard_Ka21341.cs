using ModJam_Ka21341.Buffs;
using ModJam_Ka21341.UtilKa21341;

namespace ModJam_Ka21341.Cards
{
    public class DiceCardSelfAbility_BlakeSpecialCard_Ka21341 : DiceCardSelfAbilityBase
    {
        public override bool IsValidTarget(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            return unit != targetUnit && targetUnit.faction == unit.faction && unit.bufListDetail.GetActivatedBufList()
                .Exists(x => x.bufType == KeywordBuf.Bleeding);
        }

        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            Activate(unit, targetUnit);
            self.exhaust = true;
        }

        private static void Activate(BattleUnitModel owner, BattleUnitModel unit)
        {
            SkinUtil.PlayBleedEffect(unit);
            SkinUtil.PlayBleedEffect(owner);
            unit.bufListDetail.RemoveBufAll(KeywordBuf.Bleeding);
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1, unit);
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 1, unit);
            if (owner.bufListDetail.HasBuf<BattleUnitBuf_Bloodlust_Ka21341>() ||
                owner.bufListDetail.HasBuf<BattleUnitBuf_BloodlustEgo_Ka21341>()) return;
            if (owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Blood_Ka21341) is
                BattleUnitBuf_Blood_Ka21341 buff)
            {
                buff.AddStacks(1);
            }
            else
            {
                buff = new BattleUnitBuf_Blood_Ka21341
                {
                    stack = 1
                };
                owner.bufListDetail.AddBuf(buff);
            }
        }

        public override bool IsOnlyAllyUnit()
        {
            return true;
        }
    }
}