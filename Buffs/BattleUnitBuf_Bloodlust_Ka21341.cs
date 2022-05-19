namespace ModJam_Ka21341.Buffs
{
    public class BattleUnitBuf_Bloodlust_Ka21341 : BattleUnitBuf
    {
        public BattleUnitBuf_Bloodlust_Ka21341()
        {
            stack = 0;
        }

        public override int paramInBufDesc => 0;
        protected override string keywordId => "BloodLustJam_Ka21341";
        protected override string keywordIconId => "BloodLustJamIcon_Ka21341";

        public override void OnRoundEnd()
        {
            _owner.bufListDetail.RemoveBuf(this);
        }

        public override void OnRoundStart()
        {
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1, _owner);
        }

        public override int GetDamageIncreaseRate()
        {
            return stack > 0 ? 5 : base.GetDamageIncreaseRate();
        }

        public override void OnSuccessAttack(BattleDiceBehavior behavior)
        {
            if (stack > 4 && behavior.card.target.bufListDetail.GetActivatedBufList()
                    .Exists(x => x.bufType == KeywordBuf.Bleeding)) _owner.RecoverHP(2);
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus { power = 1 });
        }
    }
}