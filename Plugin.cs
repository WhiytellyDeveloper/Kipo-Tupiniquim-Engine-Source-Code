using BepInEx;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Registers;
using MTM101BaldAPI.SaveSystem;
using System.Collections;
using UnityEngine;

namespace KipoTupiniquimEngine // Rename the namespace!
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("mtm101.rulerp.bbplus.baldidevapi", MTM101BaldiDevAPI.VersionNumber)] // Replace with an older version if not using most of the API's newer functions and variables and stuff...
    [BepInProcess("BALDI.exe")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; } // Remove it if necessary
        public static AssetManager assetMan = new AssetManager();

        private void Awake()
        {
            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            Instance = this;
            harmony.PatchAllConditionals();

            LoadingEvents.RegisterOnLoadingScreenStart(Info, StartLoad());
            LoadingEvents.RegisterOnAssetsLoaded(Info, PreLoad(), false); // IMPORTANT!!
            LoadingEvents.RegisterOnAssetsLoaded(Info, PostLoad(), true);

            ModdedSaveGame.AddSaveHandler(Info); // IMPORTANT!!
        }

        IEnumerator StartLoad() // Remove it if necessary
        {
            yield return 1;
            yield return "Loading screen start message";
        }

        IEnumerator PreLoad() // IMPORTANT!!
        {
            yield return 1;
            yield return "Preload message";
        }

        IEnumerator PostLoad() // Remove it if necessary
        {
            yield return 1;
            yield return "Postload message";
        }
    }

    public static class PluginInfo
    {
        public const string PLUGIN_GUID = "username.bbplus.modname"; // Example: verycoolmodder.bbplus.templatemod, all lowercase is recommended! Don't change it after publishing your mod!
        public const string PLUGIN_NAME = "Mod Template"; // This needs to be changed to your mod's name, make sure that it's not the entire GUID!
        public const string PLUGIN_VERSION = "0.0.0.0"; // It's likely gonna be 1.0.0.0 or 1.0.0, but very small mods will tend to use 1.0. REMEMBER TO UPDATE IT EVERY PATCH RELEASE!!
    }
}
