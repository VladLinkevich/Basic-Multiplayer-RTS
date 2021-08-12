using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Units
{
    public class UnitMovement : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent agent = null;

        private Camera _mainCamera;

        #region Server

        [Command]
        private void CmdMove(Vector3 position)
        {
            if (IsValidPosition(position, out var hit) == false) { return; }

            agent.SetDestination(hit.position);
        }

        #endregion

        #region Client

        public override void OnStartAuthority()
        {
            _mainCamera = Camera.main;
        }

        [ClientCallback]
        private void Update()
        {
            if (hasAuthority == false) { return; }
            if (PressRightMouseButton() == false) { return; }
            if (GetRaycastHit(out var hit) == false) { return; }
            
            CmdMove(hit.point);
        }

        #endregion

        private bool GetRaycastHit(out RaycastHit hit)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            return Physics.Raycast(ray, out hit, Mathf.Infinity);
        }

        private bool PressRightMouseButton() => Mouse.current.rightButton.wasPressedThisFrame;
        
        private bool IsValidPosition(Vector3 position, out NavMeshHit hit) => 
            NavMesh.SamplePosition(position, out hit, 1f, NavMesh.AllAreas);
    }
}