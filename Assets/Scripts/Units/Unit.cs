using System;
using Combat;
using Mirror;
using UnityEngine;

namespace Units
{
    public class Unit : NetworkBehaviour
    {
        [SerializeField] private UnitMovement UnitMovement = null;
        [SerializeField] private Targeter Targeter = null;
        [SerializeField] private GameObject SelectedHighlighting;

        public static event Action<Unit> ServerOnUnitSpawned;
        public static event Action<Unit> ServerOnUnitDespawned;
        
        public static event Action<Unit> AuthorityOnUnitSpawned;
        public static event Action<Unit> AuthorityOnUnitDespawned;
        
        public UnitMovement GetUnitMovement => UnitMovement;
        public Targeter GetTargeter => Targeter;
        
        #region Server

        public override void OnStartServer()
        {
            ServerOnUnitSpawned?.Invoke(this);
        }

        public override void OnStopServer()
        {
            ServerOnUnitDespawned?.Invoke(this);
        }

        #endregion
        
        #region Client

        public override void OnStartClient()
        {
            if (isClientOnly == false || hasAuthority == false) { return; }
            
            AuthorityOnUnitSpawned?.Invoke(this);
        }
        
        public override void OnStopClient()
        {
            if (isClientOnly == false || hasAuthority == false) { return; }

            AuthorityOnUnitDespawned?.Invoke(this);
        }

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
