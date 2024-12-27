using BepInEx;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Registers;
using MTM101BaldAPI.SaveSystem;
using System.Collections;
using System.Linq;
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
            yield return "Loading Kipo Tupiniquim Assets...";
            assetManager.Add<Sprite>("Hand", AssetLoader.SpriteFromMod(this, Vector2.zero, 1, "HandSelection.png"));

            assetManager.Add<Sprite>("ExitBut", AssetLoader.SpriteFromMod(this, Vector2.zero, 1, "Exit.png"));
            assetManager.Add<Sprite>("ExitransparentBut", AssetLoader.SpriteFromMod(this, Vector2.zero, 1, "ExitTransparent.png"));
            assetManager.Add<Sprite>("AchievementsBut", AssetLoader.SpriteFromMod(this, Vector2.zero, 1, "Achiviments.png"));
            assetManager.Add<Sprite>("AchievementsTransparentBut", AssetLoader.SpriteFromMod(this, Vector2.zero, 1, "AchivimentsTransparent.png"));

            assetManager.Add<Sprite>("1StaminometerBG", Resources.FindObjectsOfTypeAll<Sprite>().First(x => x.name == "StaminometerSheet_BG"));
            assetManager.Add<Sprite>("2StaminometerBG", AssetLoader.SpriteFromMod(this, Vector2.zero, 1, "StaminometerBGs/2StaminometerSheet_BG.png"));
            assetManager.Add<Sprite>("3StaminometerBG", AssetLoader.SpriteFromMod(this, Vector2.zero, 1, "StaminometerBGs/3StaminometerSheet_BG.png"));
            assetManager.Add<Sprite>("4StaminometerBG", AssetLoader.SpriteFromMod(this, Vector2.zero, 1, "StaminometerBGs/4StaminometerSheet_BG.png"));
            assetManager.Add<Sprite>("5StaminometerBG", AssetLoader.SpriteFromMod(this, Vector2.zero, 1, "StaminometerBGs/5StaminometerSheet_BG.png"));
            assetManager.Add<Sprite>("6StaminometerBG", AssetLoader.SpriteFromMod(this, Vector2.zero, 1, "StaminometerBGs/6StaminometerSheet_BG.png"));
            assetManager.Add<Sprite>("7StaminometerBG", AssetLoader.SpriteFromMod(this, Vector2.zero, 1, "StaminometerBGs/7StaminometerSheet_BG.png"));
            assetManager.Add<Sprite>("MaxStaminometerBG", AssetLoader.SpriteFromMod(this, Vector2.zero, 1, "StaminometerBGs/MaxStaminometerSheet_BG.png"));

            assetManager.Add<SoundObject>("ClockSpin", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "ClockSpin.wav"), "", SoundType.Effect, Color.white));
            assetManager.Get<SoundObject>("ClockSpin").subtitle = false; //WHY???
        }
    }

    public static class PluginInfo
    {
        public const string PLUGIN_GUID = "whiytellydeveloper.plugin.mod.kipotupiniquimengine";
        public const string PLUGIN_NAME = "Kipo Tupiniquim Engine";
        public const string PLUGIN_VERSION = "1.0"; 
    }
}
