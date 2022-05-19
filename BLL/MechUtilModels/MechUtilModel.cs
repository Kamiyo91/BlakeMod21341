using System;
using System.Collections.Generic;
using LOR_XML;

namespace ModJam_Ka21341.BLL.MechUtilModels
{
    public class MechUtilModel
    {
        public BattleUnitModel Owner { get; set; }

        //public string EgoMapName { get; set; }
        //public float? BgY { get; set; }
        //public float? FlY { get; set; }
        //public List<LorId> OriginalMapStageIds { get; set; }
        public bool HasEgo { get; set; }
        public bool HasEgoAttack { get; set; }
        public bool EgoActivated { get; set; }
        public bool RefreshUI { get; set; }

        public string SkinName { get; set; }

        //public bool MapUsed { get; set; }
        public List<AbnormalityCardDialog> EgoAbDialogList { get; set; }
        public bool HasEgoAbDialog { get; set; }
        public AbColorType EgoAbColorColor { get; set; }

        public Type EgoType { get; set; }

        //public Type EgoMapType { get; set; }
        public LorId EgoCardId { get; set; }
        public LorId EgoAttackCardId { get; set; }
    }
}