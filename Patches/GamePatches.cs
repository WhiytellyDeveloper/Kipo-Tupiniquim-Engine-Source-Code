using HarmonyLib;
using KipoTupiniquimEngine.Classes;
using KipoTupiniquimEngine.Extenssions;
using MTM101BaldAPI;
using MTM101BaldAPI.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace KipoTupiniquimEngine.Patches
{
    [HarmonyPatch(typeof(Notebook), nameof(Notebook.Clicked))]
    internal class NotebookAnimPatch
    {
        public static void Prefix(Notebook __instance, int player)
        {
            var sprite = __instance.ReflectionGetVariable("sprite") as SpriteRenderer;
            var animator = Singleton<CoreGameManager>.Instance.GetHud(player).ReflectionGetVariable("notebookAnimator") as Animator;
            Singleton<CoreGameManager>.Instance.GetHud(player).GetComponent<KipoHudExtenssion>().CollectPickup(sprite.sprite, animator.transform.localPosition / 1.3f);
        }
    }

    [HarmonyPatch(typeof(ItemSlotsManager), nameof(ItemSlotsManager.CollectItem))]
    internal class PickupAnimPatch
    {
        public static void Prefix(ItemSlotsManager __instance, ItemObject item, int slot)
        {
            var hud = __instance.ReflectionGetVariable("hud") as HudManager;
            var icons = __instance.ReflectionGetVariable("itemSlider") as ItemSlider[];

            //300 - 5
            //260 - 4

            if (item.addToInventory)
                Singleton<CoreGameManager>.Instance.GetHud(hud.hudNum).GetComponent<KipoHudExtenssion>().CollectPickup(item.itemSpriteLarge, new(GenericExtenssions.GenerateLowList(140, -40, icons.Length)[slot], 155, 1), 0.5f);
        }
    }

    [HarmonyPatch(typeof(Notebook), "Start")]
    internal class NotebookMultipleColorPatch
    {
        public static void Postfix(Notebook __instance)
        {
            var referenceSpriteRenderer = (SpriteRenderer)__instance.ReflectionGetVariable("sprite");
            var referenceSprite = Resources.FindObjectsOfTypeAll<Sprite>().First(x => x.name == "_nbGreen");

            Color hexaColor = referenceSprite.GetMiddlePixelColor();
            Color randomColor = new(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            referenceSpriteRenderer.sprite = referenceSprite.ChangeColorToDominant(hexaColor, randomColor);
        }
    }

    /*
    [HarmonyPatch(typeof(ItemManager), "Awake")]
    internal class AddQuartersManagerPatch
    {
        [HarmonyPostfix]
        public static void Add(ItemManager __instance) =>
            __instance.gameObject.GetOrAddComponent<KipoQuartersManager>().Initialize(__instance.gameObject.GetComponent<PlayerManager>().playerNumber);
    }

    [HarmonyPatch(typeof(Pickup), nameof(Pickup.Clicked))]
    internal class QuarterPatch
    {
        public static void Prefix(Pickup __instance, int player)
        {
            if (__instance.item.itemType == Items.Quarter)
                Singleton<CoreGameManager>.Instance.GetPlayer(player).gameObject.GetComponent<KipoQuartersManager>().AddQuarter(1);

        }
    }
    */

    [HarmonyPatch(typeof(PlayerMovement), nameof(PlayerMovement.StaminaUpdate))]
    public static class PlayerMovement_StaminaUpdate_Patch
    {
        static void Postfix(ref PlayerMovement __instance)
        {
            float multiplier = StaminaMultiplierManager.Multiplier;
            Singleton<CoreGameManager>.Instance.GetHud(__instance.pm.playerNumber).SetStaminaValue((__instance.stamina / multiplier) / (__instance.staminaMax * multiplier));
        }
    }

    public static class StaminaMultiplierManager
    {
        public static float Multiplier { get; set; } = 1f;
    }


}
