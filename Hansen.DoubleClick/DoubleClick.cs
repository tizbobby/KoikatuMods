using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using Harmony;
using IllusionUtility.GetUtility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.PointerEventData.InputButton;

namespace Hansen.Koikatu
{
    [BepInPlugin("hansen.koikatu.doubleclick", "Hansen.DoubleClick by SSG", "1.0.0")]
    public class DoubleClick : BaseUnityPlugin
    {
        private void Start()
        {
            var harmony = HarmonyInstance.Create("hansen.koikatu.nodltimeout");
            harmony.PatchAll(typeof(DoubleClick).Assembly);

            Console.WriteLine("HANSEN> DoubleClick: Hi!");
        }
    }
    
    // ReSharper disable InconsistentNaming
    // ReSharper disable UnusedParameter.Local
    // ReSharper disable UnusedMember.Local
    // ReSharper disable SuggestBaseTypeForParameter

    [HarmonyPatch(typeof(DownloadScene))]
    [HarmonyPatch("Update")]
    [HarmonyPatch(new Type[] {})]
    internal static class PatchStart
    {
        private const float interval = 0.5f;
        private static byte clicks = 0;
        private static Coroutine doubleTapCoroutine;

        private static void Postfix(DownloadScene __instance)
        {
            //BepInLogger.Log("DownloadScene Update", true);
            if (!Input.GetMouseButtonDown(0)) return;
            
            clicks++;
            if (clicks == 1)
            {
                BepInLogger.Log("Single-clicked", true);
                doubleTapCoroutine = __instance.StartCoroutine(DoubleTapInterval());
            }
            else if (clicks > 1)
            {
                // IDEA: should it set to 0 or a lower number like -3 and don't StopCoroutine, that way rapid clicks
                // don't double click more than once but if it reset it still works fine..
                clicks = 0; 
                __instance.StopCoroutine(doubleTapCoroutine);
                if (__instance.btnDownload.interactable)
                {
                    BepInLogger.Log("Double-clicked, DLing", true);
                    __instance.btnDownload.OnPointerClick(new PointerEventData(null));
                }
                else
                {
                    BepInLogger.Log("Double-clicked, but there is nothing to download", true);
                }
            }
        }
        
        private static IEnumerator DoubleTapInterval()
        {  
            yield return new WaitForSeconds(interval);
            clicks = 0;
            BepInLogger.Log("Double-click expired", true);
        }
    }
    
//    [HarmonyPatch(typeof(DownloadScene))]
//    [HarmonyPatch("SetButtonClickHandler")]
//    [HarmonyPatch(new[] { typeof(Button), typeof(int)})]
//    internal static class PatchHandler
//    {
//        private const float interval = 0.5f;
//        private static byte clicks = 0;
//        
//        private static void Postfix(DownloadScene __instance, Button clickObj, int index)
//        {
//            void LocalClicked() => DownloadClicked(__instance);
//
//            clickObj.onClick.AddListener(LocalClicked);
//        }
//
//        private static void DownloadClicked(DownloadScene __instance)
//        {
//            clicks++;
//            if (clicks == 1)
//            {
//                BepInLogger.Log("Single-clicked", true);
//                __instance.StartCoroutine(DoubleTapInterval());
//            }
//            else if (clicks > 1)
//            {
//                BepInLogger.Log("Double-clicked", true);
//                clicks = 0;
//                //if (__instance.btnDownload.interactable) __instance.btnDownload.OnPointerClick(new PointerEventData(null));
//            }
//        }
//
//        private static IEnumerator DoubleTapInterval()
//        {  
//            yield return new WaitForSeconds(interval);
//            clicks = 0;
//            BepInLogger.Log("Double-click expired", true);
//        }
//    }
//
//    public class ClickAction : MonoBehaviour, IPointerClickHandler
//    { 
//        public void OnPointerClick(PointerEventData eventData)
//        {
//            BepInLogger.Log("OnPointerClick", true);
//        }
//    }
    // ReSharper restore SuggestBaseTypeForParameter
    // ReSharper restore InconsistentNaming
    // ReSharper restore UnusedParameter.Local
    // ReSharper restore UnusedMember.Local
}