using UnityEngine;

namespace CodeBase.Characters.Player
{
    public class PlayerSkinChanger : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer _renderer;

        public void SetDamagedSkin() => 
            _renderer.material.color = Color.red;

        public void SetNormalSkin() => 
            _renderer.material.color = Color.white;
    }
}