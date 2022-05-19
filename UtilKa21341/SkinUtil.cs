using System.Collections.Generic;
using System.IO;
using System.Linq;
using HarmonyLib;
using ModJam_Ka21341.BLL;
using Sound;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace ModJam_Ka21341.UtilKa21341
{
    public static class SkinUtil
    {
        public static void GetThumbSprite(LorId bookId, ref Sprite result)
        {
            if (ModParameters.PackageId != bookId.packageId) return;
            var sprite = ModParameters.SpritePreviewChange.FirstOrDefault(x => x.Value.Contains(bookId));
            if (string.IsNullOrEmpty(sprite.Key) || !sprite.Value.Any()) return;
            result = ModParameters.ArtWorks[sprite.Key];
        }

        public static void SetEpisodeSlots(UIBookStoryChapterSlot instance, UIBookStoryPanel panel,
            List<UIBookStoryEpisodeSlot> episodeSlots)
        {
            if (instance.chapter != 7) return;
            var uibookStoryEpisodeSlot = episodeSlots.Find(x =>
                x.books.Find(y => y.id.packageId == ModParameters.PackageId) != null);
            if (uibookStoryEpisodeSlot == null) return;
            var books = uibookStoryEpisodeSlot.books;
            var uibookStoryEpisodeSlot2 = episodeSlots[episodeSlots.Count - 1];
            books.RemoveAll(x => x.id.packageId == ModParameters.PackageId);
            uibookStoryEpisodeSlot2.Init(instance.chapter, books, instance);
        }

        public static void SetBooksData(UIOriginEquipPageList instance,
            List<BookModel> books, UIStoryKeyData storyKey)
        {
            if (ModParameters.PackageId != storyKey.workshopId) return;
            var image = (Image)instance.GetType().GetField("img_IconGlow", AccessTools.all).GetValue(instance);
            var image2 = (Image)instance.GetType().GetField("img_Icon", AccessTools.all).GetValue(instance);
            var textMeshProUGUI = (TextMeshProUGUI)instance.GetType().GetField("txt_StoryName", AccessTools.all)
                .GetValue(instance);
            if (books.Count < 0) return;
            image.enabled = true;
            image2.enabled = true;
            image2.sprite = ModParameters.ArtWorks[storyKey.workshopId];
            image.sprite = ModParameters.ArtWorks[storyKey.workshopId];
            textMeshProUGUI.text = "Distorted Bloodfiend";
        }

        public static void GetArtWorks(DirectoryInfo dir)
        {
            if (dir.GetDirectories().Length != 0)
            {
                var directories = dir.GetDirectories();
                foreach (var t in directories) GetArtWorks(t);
            }

            foreach (var fileInfo in dir.GetFiles())
            {
                var texture2D = new Texture2D(2, 2);
                texture2D.LoadImage(File.ReadAllBytes(fileInfo.FullName));
                var value = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height),
                    new Vector2(0f, 0f));
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                ModParameters.ArtWorks[fileNameWithoutExtension] = value;
            }
        }

        public static void PlayBleedEffect(BattleUnitModel unit)
        {
            var gameObject = Util.LoadPrefab("Battle/DiceAttackEffects/New/FX/DamageDebuff/FX_DamageDebuff_Blooding");
            if (gameObject != null && unit?.view != null)
            {
                gameObject.transform.parent = unit.view.camRotationFollower;
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localScale = Vector3.one;
                gameObject.transform.localRotation = Quaternion.identity;
            }

            SoundEffectPlayer.PlaySound("Buf/Effect_Bleeding");
        }

        public static void PreLoadBufIcons()
        {
            foreach (var baseGameIcon in Resources.LoadAll<Sprite>("Sprites/BufIconSheet/")
                         .Where(x => !BattleUnitBuf._bufIconDictionary.ContainsKey(x.name)))
                BattleUnitBuf._bufIconDictionary.Add(baseGameIcon.name, baseGameIcon);
            foreach (var artWork in ModParameters.ArtWorks.Where(x =>
                         !x.Key.Contains("Glow") && !x.Key.Contains("Default") &&
                         !BattleUnitBuf._bufIconDictionary.ContainsKey(x.Key)))
                BattleUnitBuf._bufIconDictionary.Add(artWork.Key, artWork.Value);
        }
    }
}