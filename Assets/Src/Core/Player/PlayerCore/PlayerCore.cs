namespace Src.Core.Player.PlayerCore
{
    using UnityEngine;

    [RequireComponent(typeof(PlayerControls))]
    public class PlayerCore : MonoBehaviour
    {
        [SerializeField] private PlayerControls playerControls;
        private PlayerData playerData;

        private void Awake()
        {
            playerData = new PlayerData();

            if (playerControls == null)
            {
                Debug.LogAssertion("Missing PlayerControls... Failure");
            }

        }

    }

}
