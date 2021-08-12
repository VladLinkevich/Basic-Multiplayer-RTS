using System;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Buildings
{
    public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject unitPrefab = null;

        #region Server

        [Command]
        private void CmdSpawnUnit()
        {
            GameObject unitInstance = Instantiate(unitPrefab, transform.position, Quaternion.identity);
            NetworkServer.Spawn(unitInstance, connectionToClient);
        }

        #endregion

        #region Client

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) {return;} 
            if (!hasAuthority) {return;}
            
            CmdSpawnUnit();
        }

        #endregion
    }
}
