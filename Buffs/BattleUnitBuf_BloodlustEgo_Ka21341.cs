using System.Linq;
using Battle.CreatureEffect;
using Sound;
using UnityEngine;

namespace ModJam_Ka21341.Buffs
{
    public class BattleUnitBuf_BloodlustEgo_Ka21341 : BattleUnitBuf
    {
        private const string Path = "6/RedHood_Emotion_Aura";
        private CreatureEffect _aura;

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
            PlayChangingEffect(owner);
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

        private void PlayChangingEffect(BattleUnitModel owner)
        {
            owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
            if (_aura == null)
                _aura = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect(Path, 1f, owner.view,
                    owner.view);
            var original = Resources.Load("Prefabs/Battle/SpecialEffect/RedMistRelease_ActivateParticle");
            if (original != null)
            {
                var gameObject = Object.Instantiate(original) as GameObject;
                if (gameObject != null)
                {
                    gameObject.transform.parent = owner.view.charAppearance.transform;
                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.transform.localRotation = Quaternion.identity;
                    gameObject.transform.localScale = Vector3.one;
                }
            }

            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Battle/Kali_Change");
        }
    }
}