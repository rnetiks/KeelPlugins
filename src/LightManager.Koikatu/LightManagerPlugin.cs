﻿using BepInEx;
using BepInEx.Harmony;
using HarmonyLib;
using KKAPI.Studio.SaveLoad;
using UnityEngine;

namespace KeelPlugins
{
    [BepInDependency(KKAPI.KoikatuAPI.GUID)]
    [BepInProcess(KoikatuConstants.StudioProcessName)]
    [BepInPlugin(GUID, "Light Manager", Version)]
    public class LightManagerPlugin : BaseUnityPlugin
    {
        public const string GUID = "keelhauled.lightmanager";
        public const string Version = "1.0.0";

        private Harmony harmony;
        private static GameObject bepinex;

        private void Awake()
        {
            bepinex = gameObject;
            harmony = new Harmony($"{GUID}.harmony");
            HarmonyWrapper.PatchAll(typeof(Hooks), harmony);
            StudioSaveLoadApi.RegisterExtraBehaviour<SceneDataController>(GUID);
        }

#if DEBUG
        private void OnDestroy()
        {
            harmony.UnpatchAll(typeof(Hooks));
        }
#endif

        private class Hooks
        {
            [HarmonyPrefix, HarmonyPatch(typeof(StudioScene), "Start")]
            public static void Entrypoint()
            {
                bepinex.GetOrAddComponent<LightManager>();
            }
        }
    }
}
