using Mirror;
using UnityEngine;

namespace Combat
{
    public class Targeter : NetworkBehaviour
    {
        private Targetable _target;

        public Targetable Target => _target;
        
        #region Server

        [Command]
        public void CmdSetTarget(GameObject targetGameObject)
        {
            if (targetGameObject.TryGetComponent<Targetable>(out Targetable newTarget) == false) {return;}

            _target = newTarget;
        }
    
        [Server]
        public void ClearTarget()
        {
            _target = null;
        }

        #endregion
    }
}
