using System;
using System.Reflection;
using BepInEx;
using Harmony;
using UnityEngine;

namespace Hansen.Koikatu
{
    [BepInPlugin("hansen.koikatu.nodltimeout", "Hansen.NoDownloadTimeout by SSG", "1.0.0")]
    public class NoDownloadTimeout : BaseUnityPlugin
    {
        private void Start()
        {
            var harmony = HarmonyInstance.Create("hansen.koikatu.nodltimeout");
            harmony.PatchAll(typeof(NoDownloadTimeout).Assembly);

            Console.WriteLine("HANSEN> NoDownloadTimeout: Hi!");
        }
    }

    // ReSharper disable InconsistentNaming
    // ReSharper disable UnusedParameter.Local
    // ReSharper disable UnusedMember.Local
    [HarmonyPatch(typeof(DownloadScene))]
    [HarmonyPatch("Update")]
    [HarmonyPatch(new Type[] { })]
    internal class Patch
    {
        private static FieldInfo corGetInfoStartTime;
        private static FieldInfo corDLCntStartTime;

        private static bool Prepare(HarmonyInstance instance)
        {
            corGetInfoStartTime = AccessTools.Field(typeof(DownloadScene), "corGetInfoStartTime");
            corDLCntStartTime = AccessTools.Field(typeof(DownloadScene), "corDLCntStartTime");
            return true;
        }
        
        private static void Prefix(DownloadScene __instance)
        {
//            Console.WriteLine("HANSEN> DownloadScene Update1: " + corGetInfoStartTime.GetValue(__instance));
//            Console.WriteLine("HANSEN> DownloadScene Update2: " + corDLCntStartTime.GetValue(__instance));
            
            corGetInfoStartTime.SetValue(__instance, Time.realtimeSinceStartup);
            corDLCntStartTime.SetValue(__instance, Time.realtimeSinceStartup);
        }
    }
    // ReSharper restore InconsistentNaming
    // ReSharper restore UnusedParameter.Local
    // ReSharper restore UnusedMember.Local
}