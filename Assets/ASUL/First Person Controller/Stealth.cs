using ASUL.FPC.Animations;
using ASUL.InputSystem;
using ASUL.NPC.PlayerDetection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ASUL.FPC
{
    [RequireComponent(typeof(FirstPersonAnimator))]
    public class Stealth : MonoBehaviour
    {
        private InputAction crouchEvent;
        private Animator animator;

        [SerializeField] private CharacterController cc;
        public bool IsCrouch { get; private set; } = false;
        public bool IsCrouchLocked { get; set; } = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            if (InputManager.Instance != null)
            {
                crouchEvent = InputManager.Instance.actions.Player.Crouch;
            }
        }

        private void OnEnable()
        {
            if (crouchEvent != null)
            {
                crouchEvent.Enable();
                crouchEvent.performed += OnCrouch;
            }
        }

        private void OnDisable()
        {
            if (crouchEvent != null)
            {
                crouchEvent.performed -= OnCrouch;
                crouchEvent.Disable();
            }
        }

        private void OnCrouch(InputAction.CallbackContext ctx)
        {
            if (IsCrouchLocked)
                return;

            ToggleCrouch(!IsCrouch);
        }

        public void ToggleCrouch(bool value)
        {
            IsCrouch = value;

            if (IsCrouch)
                animator.SetTrigger("Stealth");
            else
                animator.SetTrigger("Normal");

            SetCouchCollider(value);
            SetVisibillityState();
        }

        private void SetCouchCollider(bool value)
        {
            if (value)
            {
                cc.center = new(cc.center.x, -0.38f, cc.center.z);
                cc.radius = 0.6f;
                cc.height = 1.32f;
            }
            else
            {
                cc.center = new(cc.center.x, 0, cc.center.z);
                cc.radius = 0.39f;
                cc.height = 2;
            }
        }

        private void SetVisibillityState()
        {
            if (PlayerVisibilityManager.PlayerVisibilityState == PlayerVisibilityState.Default)
            {
                PlayerVisibilityManager.PlayerVisibilityState = PlayerVisibilityState.Crouch;
            }
            else if (PlayerVisibilityManager.PlayerVisibilityState == PlayerVisibilityState.Crouch)
            {
                PlayerVisibilityManager.PlayerVisibilityState = PlayerVisibilityState.Default;
            }
        }
    }
}
