using System.Linq;
using UnityEngine;

namespace ModJam_Ka21341.Buffs
{
    public class BattleUnitBuf_BloodlustEgo_Ka21341 : BattleUnitBuf
    {
        private const string Path = "6/RedHood_Emotion_Aura";
        private GameObject aura;

        public BattleUnitBuf_BloodlustEgo_Ka21341()
        {
            stack = 0;
        }

        public override int paramInBufDesc => 0;
        protected override string keywordId => "BloodLustJamEgo_Ka21341";
        protected override string keywordIconId => "BloodLustJamIcon_Ka21341";
        public override bool isAssimilation => _owner.Book.BookId != new LorId(260013);

        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            var buff = owner.bufListDetail.GetActivatedBufList().FirstOrDefault(x => x is BattleUnitBuf_Blood_Ka21341);
            if (buff != null) owner.bufListDetail.RemoveBuf(buff);
            var creatureEffect =
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect(
                    "6_G/FX_IllusionCard_6_G_BloodAura", 1f, owner.view, owner.view);
            aura = creatureEffect != null ? creatureEffect.gameObject : null;
        }

        public override void OnRoundStart()
        {
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1, _owner);
        }

        public override void Destroy()
        {
            base.Destroy();
            DestroyAura();
        }

        private void DestroyAura()
        {
            if (aura == null) return;
            Object.Destroy(aura);
            aura = null;
        }

        public override void OnDie()
        {
            base.OnDie();
            Destroy();
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