using HarmonyLib;
using KipoTupiniquimEngine.Classes;
using KipoTupiniquimEngine.Extenssions;
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
}
