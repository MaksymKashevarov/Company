namespace Src.Core.Player.PlayerCore
{
    using UnityEngine;

    public class PlayerCore : MonoBehaviour
    {
        [SerializeField] private PlayerControls playerControls;
        private PlayerData playerData;

        private void Awake()
        {
            playerData = new PlayerData();
        }

    }

}