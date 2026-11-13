namespace Src.Core.Player.PlayerCore
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    [DisallowMultipleComponent]
    public class PlayerControls : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [Header("References")]
        [SerializeField] private InputActionAsset inputActions;
        [Tooltip("The camera orbiting this player rig. Uses a child Camera if left empty.")]
        [SerializeField] private Camera playerCamera;

        [Header("Movement")]
        [Min(0f)]
        [SerializeField] private float movementSpeed = 15f;
        [Min(1f)]
        [SerializeField] private float boostMultiplier = 3f;
        [Tooltip("Horizontal drag distance per mouse pixel, scaled by the orbit distance.")]
        [Min(0f)]
        [SerializeField] private float panSpeed = 0.002f;

        [Header("Orbit")]
        [Tooltip("Distance from this player rig's position, which is the virtual focus point.")]
        [Min(0.1f)]
        [SerializeField] private float distance = 20f;
        [Min(0.1f)]
        [SerializeField] private float minDistance = 5f;
        [Min(0.1f)]
        [SerializeField] private float maxDistance = 80f;
        [Tooltip("Degrees of rotation per mouse pixel while the right mouse button is held.")]
        [Min(0f)]
        [SerializeField] private float rotationSpeed = 0.2f;
        [SerializeField] private float yaw;
        [SerializeField] private float pitch = 45f;
        [Range(1f, 89f)]
        [SerializeField] private float minPitch = 10f;
        [Range(1f, 89f)]
        [SerializeField] private float maxPitch = 80f;

        [Header("Zoom")]
        [Tooltip("Orbit distance change per scroll input unit. Scroll up zooms in.")]
        [Min(0f)]
        [SerializeField] private float zoomSpeed = 2f;

        private InputActionAsset runtimeInputActions;
        private InputActionMap playerActionMap;
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction zoomAction;
        private InputAction rotateAction;
        private InputAction panAction;
        private InputAction boostAction;

        // Safe to call from PlayerCore.Awake or OnEnable, regardless of Awake order.
        public void Initialize()
        {
            if (runtimeInputActions != null)
            {
                return;
            }

            if (playerCamera == null)
            {
                Debug.LogAssertion("Error, No Camera, Fix it now!");
                return;
            }

            if (inputActions == null || playerCamera == null || playerCamera.transform == transform)
            {
                Debug.LogError("PlayerControls requires an InputActionAsset and a separate camera. " +
                    "Assign the input asset and a child Camera in the Inspector.", this);
                enabled = false;
                return;
            }
        }


    }
}
