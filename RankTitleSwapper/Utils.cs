using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RankTitleSwapper
{
    public static class PathUtils
    {
        public static string GameDirectory()
        {
            string path = Application.dataPath;
            if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                path = Utility.ParentDirectory(path, 2);
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                path = Utility.ParentDirectory(path, 1);
            }

            return path;
        }

        public static string ModDirectory()
        {
            return Path.Combine(GameDirectory(), @"BepInEx\UMM Mods");
        }
    }
}