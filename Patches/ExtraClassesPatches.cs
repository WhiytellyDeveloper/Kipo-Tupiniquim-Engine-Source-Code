using HarmonyLib;
using KipoTupiniquimEngine.Classes.ExtraClasses;
using MTM101BaldAPI;
using MTM101BaldAPI.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace KipoTupiniquimEngine.Patches
{
    [HarmonyPatch(typeof(Window), nameof(Window.Initialize))]
    internal class WindowExtraClassPatch
    {
        public static void Prefix(Window __instance)
        {
            var window = __instance.gameObject.GetOrAddComponent<WindowExtraClass>();
            window.window = __instance;
        }
    }

    [HarmonyPatch(typeof(SwingDoor), nameof(SwingDoor.ItemFits))]
    public class SwingDoorExtraClassPatch
    {
        public static bool Prefix() { return false; }

        public static void Postfix(SwingDoor __instance, ref bool __result, Items item)
        {
            if (__instance.gameObject.GetComponent<CoinDoor>() && item == Items.Quarter)
            {
                __result = true; 
                return;
            }

            __result = AcceptableItems.Contains(item) && !__instance.locked && (bool)__instance.ReflectionGetVariable("acceptsLockItem");
        }

        public static List<Items> AcceptableItems =  [Items.DoorLock];
    }


    [HarmonyPatch(typeof(SwingDoor), nameof(SwingDoor.InsertItem))]
    public class SwingDoorExtraClassPatchBugFix
    {
        public static bool Prefix(SwingDoor __instance, PlayerManager player, EnvironmentController ec)
        {
            var coinComponent = __instance.gameObject.GetComponent<CoinDoor>();
            if (coinComponent != null)
            {
                coinComponent.InsertItem(player, ec);
                return false;
            }
            return true;
        }
    }
}
