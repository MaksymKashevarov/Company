namespace Src.Core.Player.PlayerCore
{
    using Src.Core.UI;
    using UnityEngine;

    [RequireComponent(typeof(PlayerControls))]
    public class PlayerCore : MonoBehaviour
    {
        [SerializeField] private PlayerControls playerControls;
        [SerializeField] private Canvas _canvas; //Temporary
        [SerializeField] private PlayerInterfaceController _controller;
        [SerializeField] private ActionSelector _selector; //Temporary
        private PlayerData playerData;

        private void Awake()
        {
            playerData = new PlayerData();

            if (playerControls == null)
            {
                Debug.LogAssertion("Missing PlayerControls... Failure");
                return;
            }
            if (_controller == null)
            {
                Debug.LogAssertion("Missing Controller.... Failure!");
                return;
            }
            _controller.BindComponents(_canvas, _selector);

        }

    }

}
