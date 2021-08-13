using System;
using Combat;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public class UnitMovement : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent Agent = null;
        [SerializeField] private Targeter Targeter;
        [SerializeField] private float ChaseRange = 10;

        #region Server

        [ServerCallback]
        private void Update()
        {
            if (Targeter.Target != null)
            {
                ChaseToTarget();
            } else
            {
                MoveToPoint();
            }
        }

        [Command]
        public void CmdMove(Vector3 position)
        {
            if (IsValidPosition(position, out var hit) == false) { return; }
            Targeter.ClearTarget();
            Agent.SetDestination(hit.position);
        }

        #endregion

        private void ChaseToTarget()
        {
            if (IsChaseRange() == true)
            {
                Agent.SetDestination(Targeter.Target.transform.position);
            } else if (Agent.hasPath == true)
            {
                Agent.ResetPath();
            }
        }

        private void MoveToPoint()
        {
            if (!Agent.hasPath) { return; }
            if (Agent.remainingDistance > Agent.stoppingDistance) { return; }
            Agent.ResetPath();
        }

        private bool IsValidPosition(Vector3 position, out NavMeshHit hit) => 
            NavMesh.SamplePosition(position, out hit, 1f, NavMesh.AllAreas);

        private bool IsChaseRange() =>
            Vector3.Distance(Targeter.Target.transform.position, transform.position) > ChaseRange;
    }
}