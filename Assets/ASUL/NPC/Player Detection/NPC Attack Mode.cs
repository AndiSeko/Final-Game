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
        private Vector3 lastKnownPosition;
        private Coroutine searchCoroutine;
        public Animator animator;
        public NPCState state = NPCState.Idle;
        private bool lastSeenTarget = false;
        [SerializeField] private float eliminationDistance = 2.0f;
        [SerializeField] private float speedBonus = 2.0f;
        private float baseSpeed;
        private bool isEliminating = false;
        private bool isAggressive = false; 
        private float scanFrequency => observer.scanFrequency;
        public static Action OnTargetElminated;

        private void Update()
        {
            if (!AICommandCenter.PlayerIsDetected)
            {
                if (isAggressive)
                {
                    animator.SetTrigger("Normal");
                    isAggressive = false;
                    agent.speed = baseSpeed;
                }
                return;
            }

            _scanTimer += Time.deltaTime;
            if (_scanTimer >= (1f / scanFrequency))
            {
                _scanTimer = 0f;
                UpdateState();
                HandleSpeedByVisibility();
                FindTarget();
            }
        }

        public void HandleSpeedByVisibility()
        {
            if (AICommandCenter.PlayerIsDetected)
            {
                if (!isAggressive)
                {
                    animator.SetTrigger("Agressive");
                    isAggressive = true;
                }
                agent.speed = baseSpeed + speedBonus;
            }
        }



        private void UpdateState()
        {
            if (state == NPCState.End)
                return;

            if (observer.AllyWhoDetect != null && state != NPCState.Finder)
            {
                state = NPCState.Helper;
            }
            if (observer.AllyWhoDetect == null && state == NPCState.Helper)
            {
                state = NPCState.Idle;
                agent.ResetPath();
            }
            if (!observer.IsInFovIsVisible && lastSeenTarget && state != NPCState.Loser)
            {
                lastKnownPosition = observer.Target.position;
                state = NPCState.Loser;
                lastSeenTarget = false;
            }
            if ((observer.IsTargetDetectedIsVisible || observer.IsInFovIsVisible) && state != NPCState.End)
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
                    agent.ResetPath();
                    animator.SetBool("IsWalking", false);
                    break;
                case NPCState.End:
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
                            Vector3 targetPos = allyPos + offset;
                            if (Vector3.Distance(agent.destination, targetPos) > 0.5f)
                            {
                                agent.SetDestination(targetPos);
                            }
                            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                            {
                                animator.SetBool("IsWalking", false);
                            }
                            else
                            {
                                animator.SetBool("IsWalking", true);
                            }
                        }
                        else
                        {
                            animator.SetBool("IsWalking", false);
                        }
                        break;
                    }
                case NPCState.Loser:
                    if (observer.IsInFov)
                    {
                        agent.SetDestination(observer.Target.position);
                        animator.SetBool("IsWalking", true);
                    }
                    else
                    {
                        agent.SetDestination(lastKnownPosition);
                        if (agent.remainingDistance > agent.stoppingDistance || agent.pathPending)
                        {
                            animator.SetBool("IsWalking", true);
                        }
                        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending && searchCoroutine == null)
                        {
                            animator.SetBool("IsWalking", false);
                            searchCoroutine = StartCoroutine(SearchBehavior());
                        }
                    }
                    break;
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
            PlayerVisibilityManager.PlayerVisibilityState = PlayerVisibilityState.Dead;
            isAggressive = false; 
        }

        private void Awake()
        {
            observer = GetComponent<AIObserver>();
            agent = GetComponent<NavMeshAgent>();
            baseSpeed = agent.speed;
        }

        public void ResetOnElimination()
        {
            state = NPCState.End;
            agent.ResetPath();
            animator.SetBool("IsWalking", false);
            animator.SetTrigger("Normal");
            agent.speed = baseSpeed;
            if (searchCoroutine != null)
            {
                StopCoroutine(searchCoroutine);
                searchCoroutine = null;
            }
            isAggressive = false; 
        }

        private IEnumerator SearchBehavior()
        {
            yield return new WaitForSeconds(5f);
            searchCoroutine = null;
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