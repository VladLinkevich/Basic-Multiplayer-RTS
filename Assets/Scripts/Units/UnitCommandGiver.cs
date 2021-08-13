using System;
using Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Units
{
    public class UnitCommandGiver : MonoBehaviour
    {
        [SerializeField] private UnitSelectionHandler UnitSelectionHandler = null;
        [SerializeField] private LayerMask LayerMask;
        
        private Camera _mainCamera;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current.rightButton.wasPressedThisFrame == false) { return; }
            if (GetRaycastHit(out var hit) == false) return;
            if (TrySetTarget(hit)) return;
            TryMove(hit.point);
        }

        private void TryMove(Vector3 hitPoint)
        {
            foreach (Unit unit in UnitSelectionHandler.SelectedUnits)
            {
                unit.GetUnitMovement.CmdMove(hitPoint);
            }
        }

        private bool TrySetTarget(RaycastHit hit)
        {
            if (hit.collider.TryGetComponent<Targetable>(out Targetable target))
            {
                if (target.hasAuthority)
                {
                    TryMove(hit.point);
                    return true;
                }

                SetTarget(target);
                return true;
            }

            return false;
        }

        private void SetTarget(Targetable target)
        {
            foreach (Unit unit in UnitSelectionHandler.SelectedUnits)
            {
                unit.GetTargeter.CmdSetTarget(target.gameObject);
            }
        }

        private bool GetRaycastHit(out RaycastHit hit)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            return Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask);
        }
    }
}