using System;
using System.Collections.Generic;
using System.Linq;
using LOR_XML;
using ModJam_Ka21341.BLL.MechUtilModels;

namespace ModJam_Ka21341.UtilKa21341.MechUtil
{
    public class MechUtil
    {
        private readonly MechUtilModel _model;

        public MechUtil(MechUtilModel model)
        {
            _model = model;
            if (model.HasEgo && model.EgoCardId != null) model.Owner.personalEgoDetail.AddCard(model.EgoCardId);
        }

        public void EgoActive()
        {
            if (_model.Owner.bufListDetail.HasAssimilation()) return;
            _model.EgoActivated = false;
            if (!string.IsNullOrEmpty(_model.SkinName)) _model.Owner.view.SetAltSkin(_model.SkinName);
            _model.Owner.bufListDetail.AddBufWithoutDuplication(
                (BattleUnitBuf)Activator.CreateInstance(_model.EgoType));
            _model.Owner.cardSlotDetail.RecoverPlayPoint(_model.Owner.cardSlotDetail.GetMaxPlayPoint());
            if (_model.HasEgoAttack) _model.Owner.personalEgoDetail.AddCard(_model.EgoAttackCardId);
            if (_model.RefreshUI) UnitUtil.RefreshCombatUI();
            if (_model.HasEgoAbDialog)
                UnitUtil.BattleAbDialog(_model.Owner.view.dialogUI, _model.EgoAbDialogList, _model.EgoAbColorColor);
            BattleObjectManager.instance.GetAliveList(_model.Owner.faction)
                .FirstOrDefault(x => x.Book.BookId == new LorId(260013))?
                .bufListDetail.AddBufWithoutDuplication((BattleUnitBuf)Activator.CreateInstance(_model.EgoType));
        }

        public void OnUseExpireCard(LorId cardId)
        {
            if (!_model.HasEgo || _model.EgoCardId != cardId) return;
            if (_model.EgoCardId != null) _model.Owner.personalEgoDetail.RemoveCard(_model.EgoCardId);
            _model.Owner.breakDetail.ResetGauge();
            _model.Owner.breakDetail.RecoverBreakLife(1, true);
            _model.Owner.breakDetail.nextTurnBreak = false;
            _model.EgoActivated = true;
        }

        public void DoNotChangeSkinOnEgo()
        {
            _model.SkinName = "";
        }

        public bool CheckSkinChangeIsActive()
        {
            return !string.IsNullOrEmpty(_model.SkinName);
        }

        public bool EgoCheck()
        {
            return _model.EgoActivated;
        }

        public void ForcedEgo()
        {
            _model.EgoActivated = true;
        }

        public void ChangeEgoAbDialog(List<AbnormalityCardDialog> value)
        {
            _model.EgoAbDialogList = value;
        }
        //public  void ChangeToEgoMap(LorId cardId)
        //{
        //    if (cardId != _model.EgoAttackCardId ||
        //        SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.isEgo) return;
        //    _model.MapUsed = true;
        //    MapUtil.ChangeMap(new MapModel
        //    {
        //        Stage = _model.EgoMapName,
        //        StageIds = _model.OriginalMapStageIds,
        //        OneTurnEgo = true,
        //        IsPlayer = true,
        //        Component = _model.EgoMapType,
        //        Bgy = _model.BgY ?? 0.5f,
        //        Fy = _model.FlY ?? 407.5f / 1080f
        //    });
        //}

        //public  void ReturnFromEgoMap()
        //{
        //    if (!_model.MapUsed) return;
        //    _model.MapUsed = false;
        //    MapUtil.ReturnFromEgoMap(_model.EgoMapName, _model.OriginalMapStageIds);
        //}
    }
}