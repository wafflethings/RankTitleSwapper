using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMM;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RankTitleSwapper
{
    [BepInPlugin("waffle.ultrakill.rts", "RankTitleSwapper", "1.0.0")]
    public class Core : BaseUnityPlugin
    {
        public static Harmony harmony = new Harmony("waffle.ultrakill.rts");
        public static string ModPath = Path.Combine(Paths.PluginPath, "RankTitleSwapper");
        public static Dictionary<string, Sprite> Images = new Dictionary<string, Sprite>();
        public static string[] FileNames =
        {
            "RankU",
            "RankSSS",
            "RankSS",
            "RankS",
            "RankA",
            "RankB",
            "RankC",
            "RankD"
        };

        public void Start()
        {
            foreach(string name in FileNames)
            {
                Images.Add(name, LoadPNG(Path.Combine(ModPath, "Titles", name + ".png")));
            }

            harmony.PatchAll(typeof(Patches));
        }

        public static Sprite LoadPNG(string filePath)
        {
            Texture2D Tex = null;
            byte[] FileData;

            if (File.Exists(filePath))
            {
                FileData = File.ReadAllBytes(filePath);
                Tex = new Texture2D(2, 2);
                Tex.LoadImage(FileData);
            }

            return Sprite.Create(Tex, new Rect(0.0f, 0.0f, Tex.width, Tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        public class Patches
        {
            [HarmonyPatch(typeof(StyleHUD), nameof(StyleHUD.Start))]
            [HarmonyPostfix]
            public static void Swap(StyleHUD __instance)
            {
                foreach(StyleRank sr in __instance.ranks)
                {
                    sr.sprite = Images[sr.sprite.name];
                }
            }
        }
    }
}
