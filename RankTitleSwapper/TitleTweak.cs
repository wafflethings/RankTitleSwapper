using HarmonyLib;
using UltraTweaker.Tweaks;
using UltraTweaker.Subsettings.Impl;
using UltraTweaker.UIElements.Impl;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Reflection;
using UltraTweaker;

namespace RankTitleSwapper
{
    [TweakMetadata("Swap Rank Titles", $"{Core.GUID}.rank_title_swapper", "Replace rank titles with ones of your choice.", $"{UltraTweaker.UltraTweaker.GUID}.hud")]
    public class TitleTweak : Tweak
    {
        private static string _modPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        private static string _titlePath = Path.Combine(_modPath, "Titles");

        public List<string> Paths
        {
            get
            {
                string[] files = Directory.GetFiles(_titlePath);

                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = files[i].Split(Path.DirectorySeparatorChar).Last();
                }

                return files.ToList();
            }
        }

        private List<Sprite> _default;
        private List<Sprite> _edited;

        public TitleTweak()
        {
            string modifiedPath = _titlePath;
            if (modifiedPath.Contains(@"AppData\Roaming"))
            {
                modifiedPath = modifiedPath.Substring(modifiedPath.IndexOf(@"AppData\Roaming") + @"AppData\Roaming".Length);
                modifiedPath = @"%appdata%\" + modifiedPath;
            } 
            else if (modifiedPath.Contains(PathUtils.GameDirectory()))
            {
                modifiedPath = modifiedPath.Replace(PathUtils.GameDirectory(), "ULTRAKILL");
            }
            
            Subsettings = new()
            {
                { "path", new CommentSubsetting(this, new Metadata("Path", "path", modifiedPath), new CommentSubsettingElement(), OpenFolder, "OPEN") },
                { "ranku", new StringSubsetting(this, new Metadata("ULTRAKILL", "ranku", "ULTRAKILL style rank."),
                    new DropdownStringSubsettingElement(Paths), "RankU.png") },
                { "ranksss", new StringSubsetting(this, new Metadata("SSShitstorm", "ranksss", "SSShitstorm style rank."),
                    new DropdownStringSubsettingElement(Paths), "RankSSS.png") },
                { "rankss", new StringSubsetting(this, new Metadata("SSadistic", "rankss", "SSadistic style rank."),
                    new DropdownStringSubsettingElement(Paths), "RankSS.png") },
                { "ranks", new StringSubsetting(this, new Metadata("Supreme", "ranks", "Supreme style rank."),
                    new DropdownStringSubsettingElement(Paths), "RankS.png") },
                { "ranka", new StringSubsetting(this, new Metadata("Anarchic", "ranka", "Anarchic style rank."),
                    new DropdownStringSubsettingElement(Paths), "RankA.png") },
                { "rankb", new StringSubsetting(this, new Metadata("Brutal", "rankb", "Brutal style rank."),
                    new DropdownStringSubsettingElement(Paths), "RankB.png") },
                { "rankc", new StringSubsetting(this, new Metadata("Chaotic", "rankc", "Chaotic style rank."),
                    new DropdownStringSubsettingElement(Paths), "RankC.png") },
                { "rankd", new StringSubsetting(this, new Metadata("Destructive", "rankd", "Destructive style rank."),
                    new DropdownStringSubsettingElement(Paths), "RankD.png") },
            };
        }
        
        private void OpenFolder()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = _titlePath,
                FileName = "explorer.exe"
            };
            Process.Start(startInfo);
        }

        public override void OnTweakEnabled()
        {
            base.OnTweakEnabled();
            SetValues();
            SetStyleHud(_edited);
        }

        public override void OnTweakDisabled()
        {
            base.OnTweakDisabled();
            SetStyleHud(_default);
        }

        public override void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            _default = StyleHUD.Instance?.ranks.Select(x => x.sprite).ToList();
            SetValues();
            SetStyleHud(_edited);
        }

        public override void OnSubsettingUpdate()
        {
            SetValues();
            SetStyleHud(_edited);
        }

        public static bool TryLoadPNG(string fileName, out Sprite sprite)
        {
            string filePath = Path.Combine(_titlePath, fileName);

            UnityEngine.Debug.Log("Loading " + filePath);
            if (File.Exists(filePath))
            {
                Texture2D tex = new(2, 2);
                tex.LoadImage(File.ReadAllBytes(filePath));
                sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

                return true;
            }

            sprite = null;
            return false;
        }

        public void SetValues()
        {
            if (_default == null)
            {
                _default = StyleHUD.Instance?.ranks.Select(x => x.sprite).ToList() ?? null;
            }

            if (_default == null)
            {
                return;
            }

            _edited = _default.ToList(); // dont want the same reference

            TitleTweak instance = GetInstance<TitleTweak>();

            if (TryLoadPNG(instance.Subsettings["rankd"].GetValue<string>(), out Sprite rankD)) 
            {
                _edited[0] = rankD;
            }

            if (TryLoadPNG(instance.Subsettings["rankc"].GetValue<string>(), out Sprite rankC))
            {
                _edited[1] = rankC;
            }

            if (TryLoadPNG(instance.Subsettings["rankb"].GetValue<string>(), out Sprite rankB))
            {
                _edited[2] = rankB;
            }

            if (TryLoadPNG(instance.Subsettings["ranka"].GetValue<string>(), out Sprite rankA))
            {
                _edited[3] = rankA;
            }

            if (TryLoadPNG(instance.Subsettings["ranks"].GetValue<string>(), out Sprite rankS))
            {
                _edited[4] = rankS;
            }

            if (TryLoadPNG(instance.Subsettings["rankss"].GetValue<string>(), out Sprite rankSs))
            {
                _edited[5] = rankSs;
            }

            if (TryLoadPNG(instance.Subsettings["ranksss"].GetValue<string>(), out Sprite rankSss))
            {
                _edited[6] = rankSss;
            }

            if (TryLoadPNG(instance.Subsettings["ranku"].GetValue<string>(), out Sprite rankU))
            {
                _edited[7] = rankU;
            }
        }

        public void SetStyleHud(List<Sprite> sprites)
        {
            if (sprites == null)
            {
                UnityEngine.Debug.LogWarning("Null list, returning.");
                return;
            }

            if (StyleHUD.Instance == null)
            {
                UnityEngine.Debug.LogWarning("Null StyleHUD, returning.");
                return;
            }

            for (int i = 0; i < StyleHUD.Instance.ranks.Count; i++)
            {
                StyleHUD.Instance.ranks[i].sprite = sprites[i];
            }

            StyleHUD.Instance.rankImage.sprite = sprites[StyleHUD.Instance.rankIndex];
        }
    }
}
