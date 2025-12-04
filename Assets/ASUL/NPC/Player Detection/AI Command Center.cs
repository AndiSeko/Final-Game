using UnityEngine;
using System.Collections.Generic;
using ASUL.NPC.PlayerDetection;

namespace ASUL.NPC
{
    public class AICommandCenter : MonoBehaviour
    {
        [SerializeField] private NPCAttackMode[] NPCs;
        [SerializeField] private float idleDelay = 10f;
        private Dictionary<NPCAttackMode.NPCState, int> catalog;
        private float idleTimer = 0f;
        public static bool PlayerIsDetected = false;
        private bool IsElminated = false;

        private void Update()
        {
            UpdateCatalog();
            if (IsElminated)
                return;
            Check();
        }

        private void UpdateCatalog()
        {
            catalog = new Dictionary<NPCAttackMode.NPCState, int>();
            foreach (NPCAttackMode npc in NPCs)
            {
                if (!catalog.ContainsKey(npc.state))
                {
                    catalog[npc.state] = 1;
                }
                else
                {
                    catalog[npc.state]++;
                }
            }

            ClearLosers();
        }

        private void ClearLosers()
        {
            List<NPCAttackMode> losers = new List<NPCAttackMode>();

            foreach (NPCAttackMode npc in NPCs)
            {
                if (npc.state == NPCAttackMode.NPCState.Loser)
                    losers.Add(npc);

                if (!catalog.ContainsKey(npc.state))
                    catalog[npc.state] = 1;
                else
                    catalog[npc.state]++;
            }

            if (losers.Count > 1)
            {
                NPCAttackMode realLoser = losers[0];

                for (int i = 1; i < losers.Count; i++)
                {
                    losers[i].state = NPCAttackMode.NPCState.Idle;
                }
            }
        }

        private void Check()
        {
            if (catalog.ContainsKey(NPCAttackMode.NPCState.End))
            {
                foreach (NPCAttackMode npc in NPCs)
                {
                    npc.state = NPCAttackMode.NPCState.End;
                    npc.ResetOnElimination();
                    npc.animator.SetBool("IsWalking", false);
                    npc.animator.SetTrigger("Normal");
                    print("Он должен работать!!!");
                }
                PlayerIsDetected = false;
                PlayerVisibilityManager.OnVisibillityAdded?.Invoke(-100f);
                NPCAttackMode.OnTargetElminated?.Invoke();
                idleTimer = 0f;
                IsElminated = true;
                return;
            }

            if (!catalog.ContainsKey(NPCAttackMode.NPCState.Finder) && PlayerIsDetected)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleDelay)
                {
                    PlayerIsDetected = false;
                    PlayerVisibilityManager.OnVisibillityAdded?.Invoke(-100f);
                    foreach (NPCAttackMode npc in NPCs)
                    {
                        npc.state = NPCAttackMode.NPCState.Idle;
                        npc.HandleSpeedByVisibility();
                        AIPatroller pat = npc.GetComponent<AIPatroller>();
                        if (pat != null)
                        {
                            pat.enabled = true;
                        }
                    }
                    print("Вас потеряли");
                    idleTimer = 0f;
                }
            }
            else
            {
                idleTimer = 0f;
            }
        }
    }
}