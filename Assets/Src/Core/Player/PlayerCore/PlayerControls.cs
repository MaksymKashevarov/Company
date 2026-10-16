namespace Src.Core.Player.PlayerCore
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    [DisallowMultipleComponent]
    public class PlayerControls : MonoBehaviour
    {
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
                playerCamera = GetComponentInChildren<Camera>();
            }

            if (inputActions == null || playerCamera == null || playerCamera.transform == transform)
            {
                Debug.LogError("PlayerControls requires an InputActionAsset and a separate camera. " +
                    "Assign the input asset and a child Camera in the Inspector.", this);
                enabled = false;
                return;
            }

            ClampSettings();

            // Own the input state so enabling/disabling this component cannot affect UI input.
            runtimeInputActions = Instantiate(inputActions);
            playerActionMap = runtimeInputActions.FindActionMap("Player", true);
            moveAction = playerActionMap.FindAction("Move", true);
            lookAction = playerActionMap.FindAction("Look", true);
            zoomAction = playerActionMap.FindAction("Zoom", true);
            rotateAction = playerActionMap.FindAction("Rotate", true);
            panAction = playerActionMap.FindAction("Pan", true);
            boostAction = playerActionMap.FindAction("Boost", true);

            ApplyCameraPose();
        }

        private void OnEnable()
        {
            Initialize();
            playerActionMap?.Enable();
        }

        private void OnDisable()
        {
            playerActionMap?.Disable();
        }

        private void OnDestroy()
        {
            if (runtimeInputActions != null)
            {
                Destroy(runtimeInputActions);
            }
        }

        private void LateUpdate()
        {
            Vector2 mouseDelta = lookAction.ReadValue<Vector2>();

            // Mouse delta and scroll are already accumulated per frame; do not multiply by deltaTime.
            // Right-button rotation takes priority when both mouse buttons are held.
            if (rotateAction.IsPressed())
            {
                yaw = Mathf.Repeat(yaw + mouseDelta.x * rotationSpeed, 360f);
                pitch = Mathf.Clamp(pitch - mouseDelta.y * rotationSpeed, minPitch, maxPitch);
            }

            Quaternion horizontalRotation = Quaternion.Euler(0f, yaw, 0f);
            Vector2 movement = Vector2.ClampMagnitude(moveAction.ReadValue<Vector2>(), 1f);
            float speed = movementSpeed * (boostAction.IsPressed() ? boostMultiplier : 1f);
            transform.position += horizontalRotation * new Vector3(movement.x, 0f, movement.y)
                * speed * Time.deltaTime;

            if (panAction.IsPressed() && !rotateAction.IsPressed())
            {
                // Move opposite to the mouse so the world feels like it is being dragged.
                transform.position -= horizontalRotation * new Vector3(mouseDelta.x, 0f, mouseDelta.y)
                    * panSpeed * distance;
            }

            distance = Mathf.Clamp(distance - zoomAction.ReadValue<float>() * zoomSpeed,
                minDistance, maxDistance);

            ApplyCameraPose();
        }

        private void ApplyCameraPose()
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 position = transform.position - rotation * Vector3.forward * distance;
            playerCamera.transform.SetPositionAndRotation(position, rotation);
        }

        private void OnValidate()
        {
            ClampSettings();
        }

        private void ClampSettings()
        {
            movementSpeed = Mathf.Max(0f, movementSpeed);
            boostMultiplier = Mathf.Max(1f, boostMultiplier);
            panSpeed = Mathf.Max(0f, panSpeed);
            rotationSpeed = Mathf.Max(0f, rotationSpeed);
            zoomSpeed = Mathf.Max(0f, zoomSpeed);
            minDistance = Mathf.Max(0.1f, minDistance);
            maxDistance = Mathf.Max(minDistance, maxDistance);
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            minPitch = Mathf.Clamp(minPitch, 1f, 89f);
            maxPitch = Mathf.Clamp(maxPitch, minPitch, 89f);
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            yaw = Mathf.Repeat(yaw, 360f);
        }
    }
}
