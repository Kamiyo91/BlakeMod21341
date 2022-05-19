namespace ModJam_Ka21341.Buffs
{
    public class BattleUnitBuf_ImmortalUntilRoundEnd_Ka21341 : BattleUnitBuf
    {
        public override void OnRoundEnd()
        {
            _owner.bufListDetail.RemoveBuf(this);
        }

        public override bool IsInvincibleHp(BattleUnitModel attacker)
        {
            return true;
        }

        public override bool IsImmortal()
        {
            return true;
        }
    }
}