using Mirror;
using UnityEngine;

namespace Units
{
    public class Unit : NetworkBehaviour
    {
        [SerializeField] private UnitMovement UnitMovement = null;
        [SerializeField] private GameObject SelectedHighlighting;

        public UnitMovement GetUnitMovement => UnitMovement;
        
        #region Client

        [Client]
        public void Select()
        {
            if (!hasAuthority) { return; }
            
            SelectedHighlighting.SetActive(true);
        }
        
        [Client]
        public void Deselect()
        {
            if (!hasAuthority) { return; }

            SelectedHighlighting.SetActive(false);
        }

        #endregion
    }
}
