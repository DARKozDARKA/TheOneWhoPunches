using System;
using Mirror;
using UnityEngine;

namespace CodeBase.Characters.Player
{
    public class TriggerCollideServerDetector : NetworkBehaviour
    {
        public Action<GameObject> OnCollisionEntered;

        private void OnTriggerEnter(Collider other)
        {
            if (isServer == false)
                return;
            
            OnCollisionEntered?.Invoke(other.gameObject);
        }
    }
}