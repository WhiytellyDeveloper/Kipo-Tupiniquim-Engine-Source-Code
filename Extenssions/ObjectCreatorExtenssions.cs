using MTM101BaldAPI.ObjectCreation;
using MTM101BaldAPI.Registers;

namespace KipoTupiniquimEngine.Extenssions
{
    public static class ObjectCreatorExtenssions
    {
        public static ItemBuilder SetReferencialName(this ItemBuilder itmB, string referencialName)
        {
            itmB.SetNameAndDescription($"Itm_{referencialName}", $"Desc_{referencialName}");
            return itmB;
        }

        public static ItemObject ToItem(this Items itemEnum)
        {
            return ItemMetaStorage.Instance.FindByEnum(itemEnum).value;
        }

        public static NPC ToNPC(this Character npcEnum)
        {
            return NPCMetaStorage.Instance.Find(x => x.character == npcEnum).value;
        }
    }
}
