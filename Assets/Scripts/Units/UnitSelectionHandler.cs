using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Units
{
    public class UnitSelectionHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask LayerMask = new LayerMask();
        public List<Unit> SelectedUnits { get; } = new List<Unit>();

        private Camera _mainCamera;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                StartSelectionArea();
            } else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                ClearSelectionArea();
            }
        }

        private void StartSelectionArea()
        {
            foreach (Unit selectedUnit in SelectedUnits)
            {
                selectedUnit.Deselect();
            }

            SelectedUnits.Clear();
        }

        private void ClearSelectionArea()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask) == false) return;
            if (hit.collider.TryGetComponent<Unit>(out Unit unit) == false) return;
            if (!unit.hasAuthority) return;
            
            SelectedUnits.Add(unit);
            foreach (Unit selectedUnit in SelectedUnits)
            {
                selectedUnit.Select();
            }
        }
    }
}