using System;
using System.Collections.Generic;
using Mirror;
using Networking;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Units
{
    public class UnitSelectionHandler : MonoBehaviour
    {
        [SerializeField] private RectTransform UnitSelectionArea = null;
        [SerializeField] private LayerMask LayerMask = new LayerMask();

        private Vector2 _startPosition;
        
        private RTSPlayer _player;
        private Camera _mainCamera;

        public List<Unit> SelectedUnits { get; } = new List<Unit>();

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            GetRTSPlayer();

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                StartSelectionArea();
            } else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                ClearSelectionArea();
            } else if (Mouse.current.leftButton.isPressed)
            {
                UpdateSelectionArea();
            }
        }

        private void UpdateSelectionArea()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            float areaWidth = mousePosition.x - _startPosition.x;
            float areaHeight = mousePosition.y - _startPosition.y;

            UnitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
            UnitSelectionArea.anchoredPosition =
                _startPosition + new Vector2(areaWidth / 2, areaHeight / 2);
        }

        private void StartSelectionArea()
        {
            if (Keyboard.current.leftShiftKey.isPressed == false)
            {
                ClearSelectedUnit();
            }

            UnitSelectionArea.gameObject.SetActive(true);
            _startPosition = Mouse.current.position.ReadValue();
        }

        private void ClearSelectionArea()
        {
            if (UnitSelectionArea.sizeDelta.magnitude == 0)
            {
                TapSelectArea();
            }
            else
            {
                MultiSelectArea();
            }

            DisableUnitSelectionArea();
        }

        private void MultiSelectArea()
        {
            Vector2 min = UnitSelectionArea.anchoredPosition - (UnitSelectionArea.sizeDelta / 2);
            Vector2 max = UnitSelectionArea.anchoredPosition + (UnitSelectionArea.sizeDelta / 2);

            foreach (Unit unit in _player.Units)
            {
                if (SelectedUnits.Contains(unit)) { continue; }
                Vector3 screenPosition = _mainCamera.WorldToScreenPoint(unit.transform.position);

                if (Collision(screenPosition, min, max))
                {
                    SelectUnit(unit);
                }
            }
        }

        private void TapSelectArea()
        {
            if (DetectUnit(out var unit)) return;
            if (CheckSelectedUnit(unit)) return;

            SelectUnit(unit);
        }

        private void ClearSelectedUnit()
        {
            foreach (Unit selectedUnit in SelectedUnits)
            {
                selectedUnit.Deselect();
            }

            SelectedUnits.Clear();
        }

        private void DisableUnitSelectionArea()
        {
            UnitSelectionArea.gameObject.SetActive(false);
            UnitSelectionArea.sizeDelta = Vector2.zero;
            UnitSelectionArea.anchoredPosition = Vector2.zero;
        }

        private bool DetectUnit(out Unit unit)
        {
            unit = null;
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask) == false) { return true; }
            return hit.collider.TryGetComponent<Unit>(out unit) == false;
        }

        private bool Collision(Vector3 screenPosition, Vector2 min, Vector2 max)
        {
            return screenPosition.x > min.x &&
                   screenPosition.x < max.x &&
                   screenPosition.y > min.y &&
                   screenPosition.y < max.y;
        }

        private void GetRTSPlayer()
        {
            if (_player == null &&
                NetworkClient.connection != null &&
                NetworkClient.connection.identity == true)
            {
                _player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
            }
        }

        private void SelectUnit(Unit unit)
        {
            SelectedUnits.Add(unit);
            unit.Select();
        }

        private bool CheckSelectedUnit(Unit unit) => !unit.hasAuthority || SelectedUnits.Contains(unit);
    }
}