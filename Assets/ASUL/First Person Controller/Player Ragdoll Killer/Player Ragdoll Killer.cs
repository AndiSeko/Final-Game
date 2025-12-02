using ASUL.Interactions;
using ASUL.NPC;
using UnityEngine;

namespace ASUL.FPC
{   
    public class PlayerRagdollKiller : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private FirstPersonController firstPersonController;
        [SerializeField] private Animator animator;

        [SerializeField] private Rigidbody[] bones;

        [SerializeField] private GameObject[] renderers;

        private void Awake()
        {
            foreach (var b in bones)
            {
                b.isKinematic = true;
            }

            NPCAttackMode.OnTargetElminated += Death;
        }

        private void Death()
        {
            CSetPreset.Cutscene();

            characterController.enabled = false;
            firstPersonController.enabled = false;
            animator.enabled = false;
            foreach (var b in bones)
            {
                b.isKinematic = false;
            }

            foreach (var r in renderers)
            {
                r.SetActive(false);
            }
        }
    }
}
