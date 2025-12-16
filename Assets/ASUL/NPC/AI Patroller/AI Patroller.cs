using ASUL.NPC.PlayerDetection;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace ASUL.NPC
{
    public class AIPatroller : MonoBehaviour
    {
        [SerializeField] private Transform dotA;
        [SerializeField] private Transform dotB;
        [SerializeField] private float waitTime = 2f;

        private protected NavMeshAgent agent;
        private Transform currentTarget;
        private bool isWaiting = false;

        public Animator animator;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            currentTarget = dotA;
            agent.SetDestination(currentTarget.position);

            if (animator != null)
                animator.SetBool("IsWalking", !isWaiting);

            PlayerVisibilityManager.OnPlayerDetected += Disable;
        }

        private void OnEnable()
        {
            currentTarget = dotA;
            agent.SetDestination(currentTarget.position);

            if (animator != null)
                animator.SetBool("IsWalking", !isWaiting);
        }

        private void OnDisable()
        {
            PlayerVisibilityManager.OnPlayerDetected -= Disable;
        }

        private void Disable()
        {
            agent.ResetPath();
            animator.SetBool("IsWalking", false);
            enabled = false;
        }

        private void Update()
        {
            if (isWaiting) return;

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    StartCoroutine(WaitAndSwitchTarget());
                }
            }
        }

        private IEnumerator WaitAndSwitchTarget()
        {
            isWaiting = true;

            if (animator != null)
                animator.SetBool("IsWalking", !isWaiting);

            yield return new WaitForSeconds(waitTime);

            currentTarget = currentTarget == dotA ? dotB : dotA;
            agent.SetDestination(currentTarget.position);

            isWaiting = false;

            if (animator != null)
                animator.SetBool("IsWalking", !isWaiting);
        }
    }
}
