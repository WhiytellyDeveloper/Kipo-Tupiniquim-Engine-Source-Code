using HarmonyLib;
using KipoTupiniquimEngine.Classes.Extensions;
using MTM101BaldAPI;

namespace KipoTupiniquimEngine.Patches
{
    public class HudPatches
    {
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.SetItemSelect))]
        internal class SetItemSelectPatch
        {
            [HarmonyPostfix]
            public static void Post(HudManager __instance) =>
                __instance.gameObject.GetComponent<KipoHudExtenssion>().ColerfulInventory();
        }

        [HarmonyPatch(typeof(HudManager), "UpdateHudColor")]
        internal class HudColorPatch
        {
            [HarmonyPrefix]
            public static bool Override() {
                return false;
            }
        }

        [HarmonyPatch(typeof(HudManager), "Awake")]
        internal class AddExtraHudPatch
        {
            [HarmonyPostfix]
            public static void Add(HudManager __instance) =>
                __instance.gameObject.GetOrAddComponent<KipoHudExtenssion>();
        }

        [HarmonyPatch(typeof(ElevatorScreen), nameof(ElevatorScreen.StartGame))]
        internal class InitializeExtraHudPatch
        {
            [HarmonyPrefix]
            public static void Initialize()
            {
                if (!Singleton<CoreGameManager>.Instance.GetHud(0).gameObject.GetComponent<KipoHudExtenssion>().initialized)
                {
                    var ext = Singleton<CoreGameManager>.Instance.GetHud(0).gameObject.GetComponent<KipoHudExtenssion>();
                    ext.Initialize(Singleton<CoreGameManager>.Instance.GetHud(0).hudNum);
                }
            }
        }
    }
}
