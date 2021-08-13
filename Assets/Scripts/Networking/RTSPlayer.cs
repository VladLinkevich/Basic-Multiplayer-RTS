using System.Collections.Generic;
using Mirror;
using Units;
using UnityEngine;

namespace Networking
{
    public class RTSPlayer : NetworkBehaviour
    {
        private List<Unit> _units = new List<Unit>();

        public List<Unit> Units => _units;

        #region Server
        
        public override void OnStartServer()
        {
            Unit.ServerOnUnitSpawned += Unit_ServerOnUnitSpawned;
            Unit.ServerOnUnitDespawned += Unit_ServerOnUnitDespawned;
        }

        public override void OnStopServer()
        {
            Unit.ServerOnUnitSpawned -= Unit_ServerOnUnitSpawned;
            Unit.ServerOnUnitDespawned -= Unit_ServerOnUnitDespawned;
        }

        private void Unit_ServerOnUnitSpawned(Unit unit)
        {
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }
            
            _units.Add(unit);
        }

        private void Unit_ServerOnUnitDespawned(Unit unit)
        {
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

            _units.Remove(unit);
        }

        #endregion
        
        #region Client

        public override void OnStartClient()
        {
            if (isClientOnly == false) { return; }
            
            Unit.AuthorityOnUnitSpawned += Unit_AuthorityOnUnitSpawned;
            Unit.AuthorityOnUnitDespawned += Unit_AuthorityOnUnitDespawned;
        }

        public override void OnStopClient()
        {
            if (isClientOnly == false) { return; }
            
            Unit.AuthorityOnUnitSpawned -= Unit_AuthorityOnUnitSpawned;
            Unit.AuthorityOnUnitDespawned -= Unit_AuthorityOnUnitDespawned;
        }

        private void Unit_AuthorityOnUnitSpawned(Unit unit)
        { 
            if (hasAuthority == false) { return; }
            _units.Add(unit);
        }

        private void Unit_AuthorityOnUnitDespawned(Unit unit)
        {
            if (hasAuthority == false) { return; }
            _units.Remove(unit);
        }

        #endregion
    }
}