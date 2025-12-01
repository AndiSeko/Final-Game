using ASUL.NPC.PlayerDetection;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


namespace ASUL.NPC
{
    [RequireComponent(typeof(AIObserver), typeof(NavMeshAgent))]
    public class NPCAttackMode : MonoBehaviour
    {
        private AIObserver observer;
        private NavMeshAgent agent;
        private float _scanTimer;

        [SerializeField] private Animator animator;

        public NPCState state = NPCState.Idle;

        private bool lastSeenTarget = false;

        [SerializeField] private float eliminationDistance = 2.0f;
        [SerializeField] private float speedBonus = 2.0f; 

        private float baseSpeed;

        private bool isEliminating = false;

        private float scanFrequency => observer.scanFrequency;

        public static Action OnTargetElminated;

        private void Update()
        {
            if (PlayerVisibilityManager.PlayerVisibilityState != PlayerVisibilityState.Detected)
                return;

            _scanTimer += Time.deltaTime;
            if (_scanTimer >= (1f / scanFrequency))
            {
                _scanTimer = 0f;

                UpdateState();
                HandleSpeedByVisibility();

                FindTarget();
            }
        }

        private void HandleSpeedByVisibility()
        {
            if (PlayerVisibilityManager.PlayerVisibilityState == PlayerVisibilityState.Detected)
            {
                animator.SetTrigger("Agressive");
                agent.speed = baseSpeed + speedBonus;
            }
            else if (PlayerVisibilityManager.PlayerVisibilityState == PlayerVisibilityState.Default)
            {
                animator.SetTrigger("Normal");
                agent.speed = baseSpeed;
            }
        }

        private void UpdateState()
        {
            if (observer.AllyWhoDetect != null && state != NPCState.Finder)
            {
                state = NPCState.Helper;
            }

            if (observer.AllyWhoDetect == null && state == NPCState.Helper)
            {
                state = NPCState.Idle;
                agent.ResetPath();
            }


            if (!observer.IsInFov && lastSeenTarget)
            {
                state = NPCState.Loser;
                lastSeenTarget = false;
            }

            if ((observer.IsTargetDetected || observer.IsInFov) && state != NPCState.End)
            {
                state = NPCState.Finder;
                lastSeenTarget = true;
            }
        }

        private void FindTarget()
        {
            switch (state)
            {
                case NPCState.Idle:
                case NPCState.Loser:
                    agent.ResetPath();
                    animator.SetBool("IsWalking", false);
                    break;
                case NPCState.Finder:
                    agent.SetDestination(observer.Target.position);
                    animator.SetBool("IsWalking", true);
                    CheckEliminationDistance();
                    break;
                case NPCState.Helper:
                    {
                        if (observer.AllyWhoDetect != null)
                        {
                            Vector3 allyPos = observer.AllyWhoDetect.position;

                            Vector3 offset = -observer.AllyWhoDetect.forward * 2.0f;

                            agent.SetDestination(allyPos + offset);
                        }
                        animator.SetBool("IsWalking", true);
                        break;
                    }
                case NPCState.End:
                    agent.ResetPath(); animator.SetBool("IsWalking", false); break;

            }
        }

        private void CheckEliminationDistance()
        {
            if (observer.Target == null || isEliminating) return;

            float dist = Vector3.Distance(transform.position, observer.Target.position);
            if (dist <= eliminationDistance)
            {
                StartCoroutine(EliminateTarget());
                isEliminating = true;
            }
        }

        private IEnumerator EliminateTarget()
        {
            state = NPCState.End;
            agent.ResetPath();
            animator.SetTrigger("Fire");
            yield return new WaitForSeconds(0.1f);
            print("Ñìýðòü(((");
            isEliminating = false;

            agent.speed = baseSpeed;

            PlayerVisibilityManager.PlayerVisibilityState = PlayerVisibilityState.Invisible;
        }


        private void Awake()
        {
            observer = GetComponent<AIObserver>();
            agent = GetComponent<NavMeshAgent>();
            baseSpeed = agent.speed;
        }

        //private void OnDrawGizmos()
        //{
        //    if (!Application.isPlaying) return;

        //    Color stateColor = Color.white;
        //    switch (state)
        //    {
        //        case NPCState.Finder: stateColor = Color.green; break;
        //        case NPCState.Loser: stateColor = Color.red; break;
        //        case NPCState.Helper: stateColor = Color.blue; break;
        //        case NPCState.Idle: stateColor = Color.gray; break;
        //    }

        //    Gizmos.color = stateColor;
        //    Gizmos.DrawWireSphere(transform.position, 1.0f);

        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawWireSphere(transform.position, eliminationDistance);

        //    UnityEditor.Handles.color = stateColor;
        //    UnityEditor.Handles.Label(transform.position + Vector3.up * 2f, state.ToString());
        //}

        public enum NPCState
        {
            Finder, 
            Loser, 
            Helper,
            Idle,
            End
        }
    }

    
}