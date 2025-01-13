using HarmonyLib;
using KipoTupiniquimEngine.Classes.Extensions;
using KipoTupiniquimEngine.Extenssions;
using MTM101BaldAPI;
using MTM101BaldAPI.Reflection;
using System.Linq;
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
            Singleton<CoreGameManager>.Instance.GetHud(player).GetComponent<KipoHudExtenssion>().CollectPickup(sprite.sprite, animator.transform.localPosition / 1.3f, Vector3.zero);
        }
    }

    [HarmonyPatch]
    internal class PickupAnimPatch
    {
        [HarmonyPatch(typeof(ItemSlotsManager), nameof(ItemSlotsManager.CollectItem)), HarmonyPrefix]
        public static void CollectItemPrefix(ItemSlotsManager __instance, ItemObject item, int slot)
        {
            var hud = __instance.ReflectionGetVariable("hud") as HudManager;
            var icons = __instance.ReflectionGetVariable("itemSlider") as ItemSlider[];
            var maxItems = Singleton<CoreGameManager>.Instance.GetPlayer(hud.hudNum).itm.maxItem + 1;
            var list = GenericExtenssions.GenerateHighList(0, 1, maxItems);
            list.Reverse();

            if (item.addToInventory)
            {
                var slotTransform = GameObject.Find($"ItemSlot ({list[slot]})").transform;
                var position = new Vector3(-slotTransform.localPosition.x - 40, 155, 1);
                hud.GetComponent<KipoHudExtenssion>().CollectPickup(item.itemSpriteLarge, position, Vector3.zero, 0.5f);
            }
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
            if (__instance.stamina > 100)
            {
                float multiplier = StaminaMultiplierManager.Multiplier;
                float minStamina = 100 * (multiplier - 1);
                float maxStamina = 100 * multiplier;

                float normalizedStamina = (__instance.stamina - minStamina) / (maxStamina - minStamina);
                normalizedStamina = Mathf.Clamp(normalizedStamina, 0, 1);

                Singleton<CoreGameManager>.Instance.GetHud(__instance.pm.playerNumber).SetStaminaValue(normalizedStamina);
            }
        }
    }

    public static class StaminaMultiplierManager
    {
        public static float Multiplier { get; set; } = 1f;
    }

    [HarmonyPatch(typeof(Pickup))]
    public class PickupPatch
    {
        public static Pickup lastPíckupClicked;
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Pickup.Clicked))]
        public static void Clicked_Prefix(Pickup __instance) =>
            lastPíckupClicked = __instance;

    }

    //I just copied this from the pixelGuy in the animation mod, out of pure laziness in programming it :/
    [HarmonyPatch(typeof(ItemManager), "RemoveItem")]
    public class LastRemovedItemPatch
    {
        [HarmonyPrefix]
        public static void Prefix(ItemManager __instance, int val) =>
            lastRemovedItem = __instance.items[val];

        public static ItemObject lastRemovedItem;
    }

    [HarmonyPatch(typeof(ItemManager), "Update")]
    public class ItemManagerPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(ItemManager __instance)
        {
            return !(bool)__instance.ReflectionGetVariable("disabled");
        }
    }

    [HarmonyPatch(typeof(RoomFunctionContainer), nameof(RoomFunctionContainer.AddFunction))]
    public class FunctionAddPatch
    {
        [HarmonyPrefix]
        public static void Postfix(RoomFunctionContainer __instance, RoomFunction function)
        {
            if (Singleton<CoreGameManager>.Instance != null)
                function.Initialize((RoomController)__instance.ReflectionGetVariable("room"));
        }
    }

    [HarmonyPatch(typeof(Pickup), nameof(Pickup.Start))]
    public class PickupRemoverPatch
    {
        [HarmonyPrefix]
        public static void Postfix(Pickup __instance)
        {
            var pickup =__instance.gameObject.GetOrAddComponent<PickupExtensions>();
            pickup.Initialize(__instance, __instance.item);
        }
    }
}
