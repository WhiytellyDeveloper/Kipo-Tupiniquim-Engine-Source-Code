using System.Collections.Generic;
using UnityEngine;

namespace KipoTupiniquimEngine.Classes.ItemTypes
{
    public class NPCItem : Item
    {
        public override bool Use(PlayerManager pm)
        {
            LayerMask clickMask = new LayerMask() { value = 131073 };
            if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out this.hit, pm.pc.reach, clickMask))
            {
                var npc = hit.transform.GetComponent<NPC>();

                if (npc)
                {
                    if (!proibithedCharacters.Contains(npc.Character))
                    return OnHitNPC(npc, pm);
                }
            }

            return OnFail();
        }

        public virtual bool OnFail()
        {
            Object.Destroy(gameObject);
            return false;
        }

        private RaycastHit hit;

        public virtual bool OnHitNPC(NPC npc, PlayerManager pm)
        {
            return OnHitNPC(npc, pm);
        }

        public NPC npc;
        public List<Character> proibithedCharacters = [Character.Chalkles];
    }
}
