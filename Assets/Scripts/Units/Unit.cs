using Mirror;
using UnityEngine;

namespace Units
{
    public class Unit : NetworkBehaviour
    {
        [SerializeField] private GameObject selectedHighlighting;

        #region Client

        [Client]
        public void Select()
        {
            if (!hasAuthority) { return; }
            
            selectedHighlighting.SetActive(true);
        }
        
        [Client]
        public void Deselect()
        {
            if (!hasAuthority) {return;}

            selectedHighlighting.SetActive(false);
        }

        #endregion
    }
}
