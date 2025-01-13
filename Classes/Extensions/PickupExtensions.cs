using KipoTupiniquimEngine.Extenssions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KipoTupiniquimEngine.Classes.Extensions
{
    public class PickupExtensions : MonoBehaviour
    {
        public static Dictionary<Items, Character> npcsItems = new Dictionary<Items, Character>() { { Items.Apple, Character.Baldi }, { Items.PrincipalWhistle, Character.Principal } };
        public Pickup pickup;

        public void Initialize(Pickup pickup, ItemObject originalItem)
        {
            foreach (var itemType in npcsItems.Keys.Where(itemType => itemType == originalItem.itemType))
            {
                if (Singleton<BaseGameManager>.Instance.Ec.npcsToSpawn.Count == 0)
                    return;

                var npcsList = Singleton<BaseGameManager>.Instance.Ec.npcsToSpawn;

                if (Singleton<CoreGameManager>.Instance.currentMode == Mode.Free && npcsItems[itemType] == Character.Baldi)
                {
                    Debug.Log("Removing Baldi Item");
                    ReplaceItem(pickup, originalItem, "Baldi");
                }

                if (npcsList.Exists(x => x.Character == npcsItems[itemType]))
                {
                    Debug.Log($"Removing {npcsItems[itemType]} Item");
                    ReplaceItem(pickup, originalItem, npcsItems[itemType].ToString());
                }
            }
        }

        private void ReplaceItem(Pickup pickup, ItemObject originalItem, string characterName)
        {
            var item = GetSubstituteItem(originalItem);
            Debug.Log($"Replacing {characterName} Item ({originalItem.item.name}) With {item.item.name}");
            pickup.AssignItem(item);
        }

        public ItemObject GetSubstituteItem(ItemObject baseItem)
        {
            var allItems = Resources.FindObjectsOfTypeAll<ItemObject>().ToList();
            var filteredList = allItems.Where(x => x.price == baseItem.price).ToList();
            if (filteredList.Count == 0) filteredList = allItems.Where(x => x.price > baseItem.price).ToList();
            filteredList.RemoveAll(x => x.itemType == baseItem.itemType);
            filteredList.RemoveAll(x => x.itemType == Items.BusPass || x.itemType == Items.None);
            if (filteredList.Count == 0) throw new InvalidOperationException("No valid substitute items available.");
            return filteredList.CatchRandomItem();
        }
    }
}
