using System;
using Mirror;
using UnityEngine;

namespace CodeBase.Characters.Player.Logic
{
    public class TriggerCollideLocalPlayerDetector : NetworkBehaviour
    {
        public Action<GameObject> OnCollisionEntered;

        private void OnTriggerEnter(Collider other)
        {
            if (isClient == false)
                return;
            
            if (isLocalPlayer == false)
                return;
            
            OnCollisionEntered?.Invoke(other.gameObject);
        }
    }
}