using UnityEngine;
using ASUL.InputSystem;

namespace ASUL.FPC
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour, IWASDMovable, IJumpable
    {

        #region Поля

        
        [Header("Базовые элементы")]       

        [Range(1f, 10f)]
        [SerializeField] private float speed;
        [Range(0f, 10f)]
        [SerializeField] private float sprintSpeed;

        [Header("Гравитация")]
        [SerializeField] private float gravity = -9.81f;

        [Header("Прыжок")]

        [SerializeField] private float jumpHeight = 2f;

        [Header("Повороты камеры")]
        [SerializeField] private float mouseSensitivity = 100f;

        private float xRotation = 0f;

        [Header("Сглаживание поворота камеры")]
        [SerializeField] private float lookSmoothTime = 0.05f;

        [Header("Покачивание камеры")]
        [SerializeField] private float bobFrequency = 2.0f;
        [SerializeField] private float bobAmplitude = 0.05f;
        [SerializeField] private float bobSmoothing = 8f;

        private float bobTimer;
        private Vector3 cameraDefaultLocalPos;

        private Vector2 currentLookVelocity;
        private Vector2 currentLook;
        private Vector2 targetLook;

        [Header("Динамика движения")]

        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 10f;
        private float currentSpeed;

        [Header("Параметры передвижения")]

        public bool isCanMove;
        public bool isCanSprint;
        public bool isCanJump;
        public bool isHasHeadBob;

        //Внешние полезные свойства
        public bool IsMoving {  get { return currentSpeed > 0.1f; } }

        // Поля для расчёта вектора

        private Vector3 velocity;

        // Необходимые объекты

        [Header("Необходимые объекты (characterController не обязателен)")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform cameraTransform;

        #endregion



        #region Стандартные методы Unity

        private void Awake()
        {
            InitialyzeFields();
        }

        private void Update()
        {
            ApplyGravity();
            Jump();
            Move();
            HandleLook();
            HandleHeadBob();
        }

        private void InitialyzeFields()
        {
            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }

            if (cameraTransform == null)
            {
                cameraTransform = Camera.main.transform;
            }
            cameraDefaultLocalPos = cameraTransform.localPosition;
        }

        #endregion



        #region Методы передвижения

        public Vector2 CalculateWASDMove()
        {
            if (!isCanMove)
                return Vector2.zero;

            Vector2 move = InputManager.Instance.actions.Player.Move.ReadValue<Vector2>();

            float targetSpeed = (InputManager.Instance.actions.Player.Sprint.ReadValue<float>() > 0.1f) && isCanSprint
        ? sprintSpeed + speed
        : speed;

            if (move.sqrMagnitude > 0.01f)
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
            else
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);


            return move.normalized * currentSpeed;
        }

        private void ApplyGravity()
        {
            if (characterController.isGrounded && velocity.y < 0f)
                velocity.y = -2f;

            velocity.y += gravity * Time.deltaTime;
        }

        public void Jump()
        {
            if (!isCanJump)
                return;

            if (characterController.isGrounded && InputManager.Instance.actions.Player.Jump.triggered)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        private void HandleLook()
        {
            Vector2 rawLook = InputManager.Instance.actions.Player.Look.ReadValue<Vector2>();

            targetLook = rawLook * 0.65f * mouseSensitivity;

            currentLook = Vector2.SmoothDamp(currentLook, targetLook, ref currentLookVelocity, lookSmoothTime);

            float mouseX = currentLook.x * Time.deltaTime;
            float mouseY = currentLook.y * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        private void HandleHeadBob()
        {
            if (!isHasHeadBob)
                return;

            if (IsMoving && characterController.isGrounded)
            {
                bobTimer += Time.deltaTime * bobFrequency;
                float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;

                Vector3 targetPos = cameraDefaultLocalPos + new Vector3(0f, bobOffset, 0f);
                cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, targetPos, Time.deltaTime * bobSmoothing);
            }
            else
            {
                bobTimer = 0f;
                cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraDefaultLocalPos, Time.deltaTime * bobSmoothing);
            }
        }

        public void Move()
        {
            Vector2 input = CalculateWASDMove();
            Vector3 direction = transform.TransformDirection(new Vector3(input.x, 0, input.y));

            Vector3 horizontal = direction * currentSpeed * Time.deltaTime;
            Vector3 vertical = new Vector3(0f, velocity.y * Time.deltaTime, 0f);

            characterController.Move(horizontal + vertical);
        }

        #endregion


    }
}
