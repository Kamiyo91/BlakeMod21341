using ModJam_Ka21341.Buffs;

namespace ModJam_Ka21341.Passives
{
    public class PassiveAbility_BloodLust_Ka21341 : PassiveAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
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
    }
}