using HarmonyLib;
using KipoTupiniquimEngine.Classes;
using UnityEngine;

namespace KipoTupiniquimEngine.Patches
{
    [HarmonyPatch(typeof(MainMenu), "Start")]
    public class MainMenuPatches
    {
        public static void Prefix(MainMenu __instance)
        {
            var menu = new GameObject("KipoMainMenuExtension").AddComponent<KipoMainMenuExtenssions>();
            menu.Initialize();
        }
    }
}
