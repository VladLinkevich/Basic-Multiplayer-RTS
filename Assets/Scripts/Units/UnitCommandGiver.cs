using System;
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

            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask) == false) { return; }

            TryMove(hit.point);
        }

        private void TryMove(Vector3 hitPoint)
        {
            foreach (Unit unit in UnitSelectionHandler.SelectedUnits)
            {
                unit.GetUnitMovement.CmdMove(hitPoint);
            }
        }
    }
}