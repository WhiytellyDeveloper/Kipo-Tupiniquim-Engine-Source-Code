using HarmonyLib;
using MTM101BaldAPI.Reflection;
using System.Linq;
using UnityEngine;

namespace KipoTupiniquimEngine.Patches
{
    [HarmonyPatch(typeof(VentController), nameof(VentController.Initialize))]
    internal class VentsIconPatch
    {
        public static void Postfix(VentController __instance)
        {
            Debug.Log(Resources.FindObjectsOfTypeAll<MapIcon>().FirstOrDefault().name);
            var exitGrateTransform = __instance.ReflectionGetVariable("exitGrateTransform") as Transform;
            Singleton<BaseGameManager>.Instance.Ec.map.AddIcon(Resources.FindObjectsOfTypeAll<MapIcon>().LastOrDefault(), exitGrateTransform, Color.white);
        }
    }
}
