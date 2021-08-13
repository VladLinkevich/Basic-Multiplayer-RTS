using Mirror;
using UnityEngine;

namespace Combat
{
    public class Targetable : NetworkBehaviour
    {
        [SerializeField] private Transform AimAtPoint = null;

        public Transform GetAimAtPoint => AimAtPoint;
        
    }
}
