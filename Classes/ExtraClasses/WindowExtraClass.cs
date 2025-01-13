using MTM101BaldAPI.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace KipoTupiniquimEngine.Classes.ExtraClasses
{
    public class WindowExtraClass : MonoBehaviour, IItemAcceptor
    {
        public void InsertItem(PlayerManager player, EnvironmentController ec)
        {
        }

        public bool ItemFits(Items item)
        {
            return acceptablesItems.Contains(item) && !(bool)window.ReflectionGetVariable("broken");
        }

        public Window window;
        public static List<Items> acceptablesItems = [];
    }
}
