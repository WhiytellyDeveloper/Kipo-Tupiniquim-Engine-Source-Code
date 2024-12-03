using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection.Emit;
using MTM101BaldAPI.Reflection;
using UnityEngine.UI;
using TMPro;
using KipoTupiniquimEngine.Classes;
using MTM101BaldAPI;
using KipoTupiniquimEngine.Extenssions;

namespace KipoTupiniquimEngine.Patches
{
    public class HudPatches
    {
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.SetItemSelect))]
        internal class SetItemSelectPatch
        {
            [HarmonyPrefix]
            public static bool Block() { return false; }

            [HarmonyPostfix]
            public static void Override(HudManager __instance, int value, string key)
            {
                if (Singleton<CoreGameManager>.Instance.GetPlayer(0) != null)
                {
                    var itemBackgrounds = __instance.ReflectionGetVariable("itemBackgrounds") as RawImage[];
                    var previousSelectedItem = (int)__instance.ReflectionGetVariable("previousSelectedItem");
                    var itemTitle = (TMP_Text)__instance.ReflectionGetVariable("itemTitle");
                    var itm = Singleton<CoreGameManager>.Instance.GetPlayer(0).itm;

                    if (itemBackgrounds[value] != null && itm != null)
                    {
                        itemBackgrounds[previousSelectedItem].color = Color.white;

                        Color color = itm.items[itm.selectedItem].itemSpriteLarge.GetMostGenericFromSprite();

                        if (itm.items[itm.selectedItem].itemType == Items.None)
                            color = Color.red;
                        else
                            color -= new Color(0.15f, 0.15f, 0.15f, 0f);

                        itemBackgrounds[value].color = color;
                        __instance.ReflectionSetVariable("previousSelectedItem", value);
                        if (itemTitle != null)
                            itemTitle.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(key);
                    }
                }
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
