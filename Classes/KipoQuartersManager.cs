using MTM101BaldAPI.Registers;
using UnityEngine;

namespace KipoTupiniquimEngine.Classes
{
    public class KipoQuartersManager : MonoBehaviour
    {
        public void Initialize(int player)
        {
            playerNum = player;

            ItemMetaStorage.Instance.FindByEnum(Items.Quarter).value.addToInventory = false;
            Destroy(ItemMetaStorage.Instance.FindByEnum(Items.Quarter).value.item.GetComponent<ITM_Quarter>());
        }

        public void AddQuarter(int amount)
        {
            if (quarters != quartersMax)
            {
                quarters += amount;
                UpdateText();
            }
        }

        public void UpdateText() =>
            Singleton<CoreGameManager>.Instance.GetHud(playerNum).gameObject.GetComponent<KipoHudExtenssion>().UpdateQuartersText(quarters, quartersMax);

        public int quarters, quartersMax = 5, playerNum;
    }
}
