using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using HarmonyLib;
using LOR_XML;
using ModJam_Ka21341.BLL;
using UnityEngine;

namespace ModJam_Ka21341.UtilKa21341
{
    public static class LocalizeUtil
    {
        public static void AddLocalize()
        {
            try
            {
                var dictionary =
                    typeof(BattleEffectTextsXmlList).GetField("_dictionary", AccessTools.all)
                            ?.GetValue(Singleton<BattleEffectTextsXmlList>.Instance) as
                        Dictionary<string, BattleEffectText>;
                var files = new DirectoryInfo(ModParameters.Path + "/Localize/" + "en" + "/EffectTexts")
                    .GetFiles();
                foreach (var t in files)
                    using (var stringReader = new StringReader(File.ReadAllText(t.FullName)))
                    {
                        var battleEffectTextRoot =
                            (BattleEffectTextRoot)new XmlSerializer(typeof(BattleEffectTextRoot))
                                .Deserialize(stringReader);
                        foreach (var battleEffectText in battleEffectTextRoot.effectTextList)
                        {
                            dictionary.Remove(battleEffectText.ID);
                            dictionary?.Add(battleEffectText.ID, battleEffectText);
                        }
                    }
            }
            catch (Exception)
            {
                Debug.LogError("Error loading Effect Texts");
            }
        }
    }
}