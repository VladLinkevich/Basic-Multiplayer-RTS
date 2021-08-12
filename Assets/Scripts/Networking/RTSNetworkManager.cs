using Mirror;
using UnityEngine;

namespace Networking
{
    public class RTSNetworkManager : NetworkManager
    {
        [SerializeField] private GameObject unitSpawnerPrefab = null;
        
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            base.OnServerAddPlayer(conn);

            var identityTransform = conn.identity.transform;
            GameObject unitSpawnerInstance = Instantiate(
                unitSpawnerPrefab,
                identityTransform.position,
                identityTransform.rotation);
            
            NetworkServer.Spawn(unitSpawnerInstance, conn);
        }
    }
}
