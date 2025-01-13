using UnityEngine;

namespace KipoTupiniquimEngine.Classes.ItemTypes
{
    public class RaycastItem : Item
    {
        public override bool Use(PlayerManager pm)
        {
            if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out this.hit, pm.pc.reach, pm.pc.ClickLayers))
                return OnSucess(hit, pm); 

            return OnFail();
        }

        public virtual bool OnSucess(RaycastHit hit, PlayerManager pm)
        {
            return OnSucess(hit, pm);
        }

        public virtual bool OnFail() 
        {
            Object.Destroy(gameObject);
            return false;
        }

        private RaycastHit hit;
    }
}
