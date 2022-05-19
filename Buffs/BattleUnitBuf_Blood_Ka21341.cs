using UnityEngine;

namespace ModJam_Ka21341.Buffs
{
    public class BattleUnitBuf_Blood_Ka21341 : BattleUnitBuf
    {
        private int _damage;
        protected override string keywordId => "BloodJam_Ka21341";
        protected override string keywordIconId => "Nosferatu_Blood";

        public void AddStacks(int stacks)
        {
            stack += stacks;
            stack = Mathf.Clamp(stack, 0, 15);
            if (stack == 0) _owner.bufListDetail.RemoveBuf(this);
        }

        public override void OnRoundEnd()
        {
            _damage = 0;
            if (stack > 9 && stack < 15) _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, _owner);
            if (stack <= 14) return;
            _owner.bufListDetail.AddBuf(new BattleUnitBuf_Bloodlust_Ka21341());
            _owner.bufListDetail.RemoveBuf(this);
        }

        public override int GetDamageIncreaseRate()
        {
            return stack;
        }

        public override void OnSuccessAttack(BattleDiceBehavior behavior)
        {
            if (stack > 4 && behavior.card.target.bufListDetail.GetActivatedBufList()
                    .Exists(x => x.bufType == KeywordBuf.Bleeding)) _owner.RecoverHP(2);
        }

        public override void BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            _damage += dmg;
            if (_damage < 20) return;
            AddStacks(-3);
            _damage = 0;
        }
    }
}