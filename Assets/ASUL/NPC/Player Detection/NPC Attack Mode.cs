using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using ASUL.NPC.PlayerDetection;


namespace ASUL.NPC
{
    public class NPCAttackMode : MonoBehaviour
    {
        [Header("Компоненты")]
        [SerializeField] private AIObserver observer;
        [SerializeField] private NavMeshAgent agent;

        [Header("Настройки")]
        [SerializeField] private float safeDistance = 5f;
        [SerializeField] private float attackDistance = 1f;
        [SerializeField] private float attackCooldown = 2f;

        [Header("События")]
        public UnityEvent onPlayerEliminated;

        public enum NPCMode { Idle, Approach, Attack }
        public NPCMode CurrentMode { get; private set; } = NPCMode.Idle;

        private Transform target;
        private float lastAttackTime;
        private AIPatroller patroller;

        private void Awake()
        {
            if (observer == null) observer = GetComponent<AIObserver>();
            if (agent == null) agent = GetComponent<NavMeshAgent>();
            patroller = GetComponent<AIPatroller>();
            target = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        private void Update()
        {
            if (target == null) return;

            switch (CurrentMode)
            {
                case NPCMode.Approach:
                    ApproachTarget();
                    break;
                case NPCMode.Attack:
                    AttackTarget();
                    break;
                case NPCMode.Idle:
                    break;
            }
        }

        public void SetMode(NPCMode mode)
        {
            if (CurrentMode == mode) return;

            CurrentMode = mode;

            if (mode == NPCMode.Idle)
            {
                agent.ResetPath();
                if (patroller != null)
                {
                    patroller.enabled = true;
                }
            }
            else
            {
                if (patroller != null)
                {
                    patroller.enabled = false; 
                }
                agent.ResetPath();
                agent.isStopped = false;
            }
        }

        public bool DetectsPlayer()
        {
            return observer != null && observer.IsTargetDetected;
        }

        private void ApproachTarget()
        {
            if (agent == null || !agent.isOnNavMesh) return;

            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > safeDistance)
            {
                agent.SetDestination(target.position);
            }
            else
            {
                agent.ResetPath();
            }
        }

        private void AttackTarget()
        {
            if (agent == null || !agent.isOnNavMesh) return;

            float distance = Vector3.Distance(transform.position, target.position);

            if (distance > attackDistance)
            {
                agent.SetDestination(target.position);
            }
            else
            {
                agent.ResetPath();
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    EliminatePlayer();
                    lastAttackTime = Time.time;
                }
            }
        }

        private void EliminatePlayer()
        {
            PlayerVisibilityManager.OnVisibillityAdded?.Invoke(-100f);
            onPlayerEliminated?.Invoke();
            Debug.Log("Игрок ликвидирован NPC!");

        }
    }
}