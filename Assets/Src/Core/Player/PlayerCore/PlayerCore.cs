namespace Src.Core.Player.PlayerCore
{
    using UnityEngine;

    public class PlayerCore : MonoBehaviour
    {
        private PlayerData playerData;

        private void Awake()
        {
            playerData = new PlayerData();
        }

    }

}