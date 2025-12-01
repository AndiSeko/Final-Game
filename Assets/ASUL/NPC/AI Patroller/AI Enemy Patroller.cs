using ASUL.NPC.PlayerDetection;
using UnityEngine;

namespace ASUL.NPC
{
    public class AIEnemyPatroller : AIPatroller
    {
        //private void Start()
        //{
        //    PlayerVisibilityManager.OnPlayerDetected += EndPatrolling;
        //}

        //private void OnDisable()
        //{
        //    PlayerVisibilityManager.OnPlayerDetected -= EndPatrolling;
        //}

        private void EndPatrolling()
        {
            agent.isStopped = true;
            animator.SetBool("IsWalking", false);
            this.enabled = false;
        }
    }
}
