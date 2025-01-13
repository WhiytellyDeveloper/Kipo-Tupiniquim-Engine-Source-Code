
namespace KipoTupiniquimEngine.Classes.ItemTypes
{
    public class MultipleItem : Item
    {
        public override bool Use(PlayerManager pm)
        {
            if (nextItem == null)
                return OnUse(pm, true);

            pm.itm.SetItem(nextItem, pm.itm.selectedItem);
            return OnUse(pm, false);
        }

        public virtual bool OnUse(PlayerManager pm, bool lastItem)
        {
            return OnUse(pm, lastItem);
        }

        public ItemObject nextItem;
    }
}
