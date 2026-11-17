namespace Src.Core.Player.PlayerCore
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    using Src.Core.Objects;

    [DisallowMultipleComponent]
    public class PlayerControls : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private PlayerInput _input;
        private PlayerInterfaceController _controller;

        [Header("Movement")]
        [Min(0f)] [SerializeField] private float movementSpeed = 15f;
        [Min(1f)] [SerializeField] private float boostMultiplier = 3f;
        [Min(0f)] [SerializeField] private float panSpeed = 0.002f;

        [Header("Orbit")]
        [Min(0.1f)] [SerializeField] private float distance = 20f;
        [Min(0.1f)] [SerializeField] private float minDistance = 5f;
        [Min(0.1f)] [SerializeField] private float maxDistance = 80f;
        [Min(0f)] [SerializeField] private float rotationSpeed = 0.2f;
        [SerializeField] private float yaw;
        [SerializeField] private float pitch = 45f;
        [Range(1f, 89f)] [SerializeField] private float minPitch = 10f;
        [Range(1f, 89f)] [SerializeField] private float maxPitch = 80f;

        [Header("Zoom")]
        [Min(0f)] [SerializeField] private float zoomSpeed = 2f;

        private Vector2 moveInput;
        private Vector2 lookInput;
        private float zoomInput;
        private bool rotating;
        private bool panning;
        private bool boosting;

        private void Awake() => ApplyCameraPose();

        private void Update()
        {
            if (moveInput == Vector2.zero && lookInput == Vector2.zero && zoomInput == 0f)
                return;

            ApplyLook();
            ApplyMovement();
            ApplyZoom();
            ApplyCameraPose();

            lookInput = Vector2.zero;
            zoomInput = 0f;
        }

        public void OnMove(InputValue value) =>
            moveInput = Vector2.ClampMagnitude(value.Get<Vector2>(), 1f);

        public void OnLook(InputValue value) =>
            lookInput = value.Get<Vector2>();

        public void OnZoom(InputValue value) =>
            zoomInput = value.Get<float>();

        public void OnRotate(InputValue value) =>
            rotating = value.isPressed;

        public void OnPan(InputValue value) =>
            panning = value.isPressed;

        public void OnBoost(InputValue value) =>
            boosting = value.isPressed;

        public void OnWorldClick(InputValue value)
        {
            if (!value.isPressed)
                return;

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = playerCamera.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit))
                return;

            ITObject obj = hit.collider.GetComponent<ITObject>();
            _controller.ProcessClickOnObject(obj);
        }

        private void ApplyLook()
        {
            if (rotating)
            {
                yaw = Mathf.Repeat(yaw + lookInput.x * rotationSpeed, 360f);
                pitch = Mathf.Clamp(pitch - lookInput.y * rotationSpeed, minPitch, maxPitch);
            }
            else if (panning)
            {
                transform.position -= Quaternion.Euler(0f, yaw, 0f) *
                    new Vector3(lookInput.x, 0f, lookInput.y) * panSpeed * distance;
            }
        }

        private void ApplyMovement()
        {
            if (moveInput == Vector2.zero)
                return;

            float speed = movementSpeed * (boosting ? boostMultiplier : 1f);
            Vector3 direction = Quaternion.Euler(0f, yaw, 0f) *
                new Vector3(moveInput.x, 0f, moveInput.y);

            transform.position += direction * speed * Time.deltaTime;
        }

        private void ApplyZoom()
        {
            if (zoomInput == 0f)
                return;

            distance = Mathf.Clamp(distance - zoomInput / 120f * zoomSpeed,
                minDistance, maxDistance);
        }

        private void ApplyCameraPose()
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 position = transform.position - rotation * Vector3.forward * distance;
            playerCamera.transform.SetPositionAndRotation(position, rotation);
        }

        public void AttachController(PlayerInterfaceController controller)
        {
            _controller = controller;
        }
    }
}
