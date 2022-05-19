using System.Linq;
using ModJam_Ka21341.BLL.MechUtilModels;
using ModJam_Ka21341.Buffs;

namespace ModJam_Ka21341.UtilKa21341.MechUtil
{
    public class NpcMechUtil : MechUtil
    {
        private readonly NpcMechUtilModel _model;
        private readonly string _saveId;

        public NpcMechUtil(NpcMechUtilModel model, string saveId) : base(model)
        {
            _model = model;
            _saveId = saveId;
        }

        public void OnUseCardResetCount(BattlePlayingCardDataInUnitModel curCard)
        {
            if (_model.LorIdEgoMassAttack != curCard.card.GetID()) return;
            _model.Counter = 0;
            _model.Owner.allyCardDetail.ExhaustACardAnywhere(curCard.card);
        }

        public void MechHpCheck(int dmg)
        {
            if (_model.Owner.hp - dmg > _model.MechHp || !_model.HasMechOnHp) return;
            _model.HasMechOnHp = false;
            _model.Owner.SetHp(_model.Owner.MaxHp);
            _model.Owner.breakDetail.ResetGauge();
            _model.Owner.breakDetail.RecoverBreakLife(1, true);
            _model.Owner.breakDetail.nextTurnBreak = false;
            _model.Owner.bufListDetail.AddBuf(new BattleUnitBuf_ImmortalUntilRoundEnd_Ka21341());
        }

        public void MechHpCall()
        {
            if (!_model.HasMechOnHp) return;
            _model.HasMechOnHp = false;
            _model.Owner.SetHp(_model.Owner.MaxHp);
            _model.Owner.breakDetail.ResetGauge();
            _model.Owner.breakDetail.RecoverBreakLife(1, true);
            _model.Owner.breakDetail.nextTurnBreak = false;
            _model.Owner.bufListDetail.AddBuf(new BattleUnitBuf_ImmortalUntilRoundEnd_Ka21341());
        }
        public void CheckPhase()
        {
            if (_model.Owner.hp > _model.MechHp &&
                BattleObjectManager.instance.GetAliveList(_model.Owner.faction).Count > 3 || _model.Phase > 0) return;
            _model.Phase++;
            MechHpCall();
            ForcedEgo();
            SetMassAttack(true);
            SetCounter(_model.MaxCounter);
        }

        public int GetPhase()
        {
            return _model.Phase;
        }

        public void RaiseCounter()
        {
            if (_model.MassAttackStartCount && _model.Counter < _model.MaxCounter) _model.Counter++;
        }

        public void SetMassAttack(bool value)
        {
            _model.MassAttackStartCount = value;
        }

        public void SetOneTurnCard(bool value)
        {
            _model.OneTurnCard = value;
        }

        public void SetCounter(int value)
        {
            _model.Counter = value;
        }

        public void OnSelectCardPutMassAttack(ref BattleDiceCardModel origin)
        {
            if (!_model.MassAttackStartCount || _model.Counter < _model.MaxCounter || _model.OneTurnCard)
                return;
            origin = BattleDiceCardModel.CreatePlayingCard(
                ItemXmlDataList.instance.GetCardItem(_model.LorIdEgoMassAttack));
            SetOneTurnCard(true);
        }

        public void ExhaustEgoAttackCards()
        {
            var cards = _model.Owner.allyCardDetail.GetAllDeck().Where(x => x.GetID() == _model.LorIdEgoMassAttack);
            foreach (var card in cards) _model.Owner.allyCardDetail.ExhaustACardAnywhere(card);
        }

        public void OnEndBattle()
        {
            var stageModel = Singleton<StageController>.Instance.GetStageModel();
            var currentWaveModel = Singleton<StageController>.Instance.GetCurrentWaveModel();
            if (currentWaveModel == null || currentWaveModel.IsUnavailable()) return;
            stageModel.SetStageStorgeData(_saveId, _model.Phase);
        }

        public void Restart()
        {
            Singleton<StageController>.Instance.GetStageModel()
                .GetStageStorageData<int>(_saveId, out var curPhase);
            _model.Phase = curPhase;
            if (_model.Phase < 1) return;
            ForcedEgo();
            SetMassAttack(true);
            SetCounter(0);
        }
    }
}