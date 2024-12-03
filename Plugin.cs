using BepInEx;
using HarmonyLib;
using KipoTupiniquimEngine.Extenssions;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Registers;
using MTM101BaldAPI.SaveSystem;
using System.Collections;
using UnityEngine;

namespace KipoTupiniquimEngine
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("mtm101.rulerp.bbplus.baldidevapi", MTM101BaldiDevAPI.VersionNumber)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin _instance { get; private set; }
        public static AssetManager assetManager = new();

        private void Awake()
        {
            Harmony harmony = new(PluginInfo.PLUGIN_GUID);
            _instance = this;
            harmony.PatchAllConditionals();
            ModdedSaveGame.AddSaveHandler(Info);
            LoadingEvents.RegisterOnAssetsLoaded(Info, PreLoading(), false);
        }

        public IEnumerator PreLoading()
        {
            yield return 1;
            yield return "Loading Kipo Tupiniquim Sprites...";
            assetManager.Add<Sprite>("Hand", AssetLoader.SpriteFromMod(this, Vector2.zero, 1, "HandSelection.png"));

            assetManager.Add<Sprite>("ExitBut", AssetLoader.SpriteFromMod(this, Vector2.zero, 1, "Exit.png"));
            assetManager.Add<Sprite>("ExitransparentBut", AssetLoader.SpriteFromMod(this, Vector2.zero, 1, "ExitTransparent.png"));
        }
    }

    public static class PluginInfo
    {
        public const string PLUGIN_GUID = "whiytellydeveloper.plugin.mod.kipotupiniquimengine";
        public const string PLUGIN_NAME = "Kipo Tupiniquim Engine";
        public const string PLUGIN_VERSION = "1.0"; 
    }
}
