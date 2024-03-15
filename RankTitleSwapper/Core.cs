using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UMM;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RankTitleSwapper
{
    [BepInDependency(UltraTweaker.UltraTweaker.GUID)]
    [BepInPlugin(GUID, Name, Version)]
    public class Core : BaseUnityPlugin
    {
        public const string Name = "RankTitleSwapper";
        public const string GUID = "waffle.ultrakill.rts";
        public const string Version = "1.1.0";

        public void Start()
        {
            UltraTweaker.UltraTweaker.AddAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
