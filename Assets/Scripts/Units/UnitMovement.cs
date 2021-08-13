using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public class UnitMovement : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent Agent = null;

        #region Server

        [Command]
        public void CmdMove(Vector3 position)
        {
            if (IsValidPosition(position, out var hit) == false) { return; }

            Agent.SetDestination(hit.position);
        }

        #endregion

        private bool IsValidPosition(Vector3 position, out NavMeshHit hit) => 
            NavMesh.SamplePosition(position, out hit, 1f, NavMesh.AllAreas);
    }
}