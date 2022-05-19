using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModJam_Ka21341.BLL
{
    public static class ModParameters
    {
        public const string PackageId = "ModJamBlazeKa21341";
        public static string Path;
        public static string Language = GlobalGameManager.Instance.CurrentOption.language;
        public static Dictionary<string, Sprite> ArtWorks = new Dictionary<string, Sprite>();
        public static List<LorId> PersonalCardList = new List<LorId>();
        public static List<LorId> EgoPersonalCardList = new List<LorId>();
        public static List<LorId> UntransferablePassives = new List<LorId>();

        public static List<Tuple<List<string>, List<LorId>, LorId>> OnlyCardKeywords =
            new List<Tuple<List<string>, List<LorId>, LorId>>();

        public static Dictionary<string, string> DefaultKeyword = new Dictionary<string, string>();
        public static Dictionary<string, List<LorId>> SpritePreviewChange = new Dictionary<string, List<LorId>>();
        public static List<LorId> BookList = new List<LorId>();

        public static List<Tuple<string, List<LorId>, string>> SkinNameIds =
            new List<Tuple<string, List<LorId>, string>>();

        public static List<LorId> BannedEmotionSelectionUnit = new List<LorId>();
    }
}