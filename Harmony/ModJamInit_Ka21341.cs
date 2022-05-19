using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ModJam_Ka21341.BLL;
using ModJam_Ka21341.UtilKa21341;
using MonoMod.Utils;

namespace ModJam_Ka21341.Harmony
{
    public class ModJamInit_Ka21341 : ModInitializer
    {
        public override void OnInitializeMod()
        {
            InitParameters();
            new HarmonyLib.Harmony("LOR.ModJamKaPatch21341_MOD").PatchAll();
            UnitUtil.InitKeywords(Assembly.GetExecutingAssembly());
            UnitUtil.ChangeCardItem(ItemXmlDataList.instance, ModParameters.PackageId);
            UnitUtil.ChangePassiveItem(ModParameters.PackageId);
            SkinUtil.PreLoadBufIcons();
            LocalizeUtil.AddLocalize();
        }

        private static void InitParameters()
        {
            ModParameters.Path = Path.GetDirectoryName(
                Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            SkinUtil.GetArtWorks(new DirectoryInfo(ModParameters.Path + "/ArtWork"));
            ModParameters.BannedEmotionSelectionUnit.AddRange(new List<LorId>
                { new LorId(ModParameters.PackageId, 10000001), new LorId(ModParameters.PackageId, 10000002) });
            ModParameters.EgoPersonalCardList.Add(new LorId(ModParameters.PackageId, 2));
            ModParameters.PersonalCardList.AddRange(new List<LorId>
                { new LorId(ModParameters.PackageId, 1), new LorId(ModParameters.PackageId, 3) });
            ModParameters.UntransferablePassives.Add(new LorId(ModParameters.PackageId, 2));
            ModParameters.BookList.Add(new LorId(ModParameters.PackageId, 1));
            ModParameters.DefaultKeyword.Add(ModParameters.PackageId, "ModJamPage_Ka21341");
            ModParameters.SpritePreviewChange.AddRange(new Dictionary<string, List<LorId>>
            {
                { "BlakeDefault_Ka21341", new List<LorId> { new LorId(ModParameters.PackageId, 10000001) } },
                { "BlakeMinionDefault_Ka21341", new List<LorId> { new LorId(ModParameters.PackageId, 10000002) } }
            });
            ModParameters.SkinNameIds.AddRange(new List<Tuple<string, List<LorId>, string>>
            {
                new Tuple<string, List<LorId>, string>("BlakePlayerEgo_Ka21341",
                    new List<LorId>
                    {
                        new LorId(ModParameters.PackageId, 10000001)
                    },
                    "BlakePlayer_Ka21341"),
                new Tuple<string, List<LorId>, string>("BlakeEnemyEgo_Ka21341",
                    new List<LorId>
                    {
                        new LorId(ModParameters.PackageId, 1)
                    },
                    "BlakeEnemy_Ka21341")
            });
        }
    }
}