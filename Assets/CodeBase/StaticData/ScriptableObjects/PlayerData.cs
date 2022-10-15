using UnityEngine;

namespace CodeBase.StaticData.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "StaticData/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public float Speed;
        public float DashDistance;
        public float DashSpeedModifier;
        public float PlayerInvincibleTime;

        private void OnValidate()
        {
            if (Speed < 0)
            {
                Debug.LogError("Speed can't be negative >_<");
                Speed = 0;
            }
            if (DashDistance < 0)
            {
                Debug.LogError("Dash Distance can't be negative (ノಠ益ಠ)ノ彡┻━┻");
                DashDistance = 0;
            }
            if (PlayerInvincibleTime < 0)
            {
                Debug.LogError("Player Invincible Time can't be negative, at least not in this universe");
                PlayerInvincibleTime = 0;
            }
        }
    }
}