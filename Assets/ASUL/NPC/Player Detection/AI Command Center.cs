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
        }

        private void Check()
        {
            if (catalog.ContainsKey(NPCAttackMode.NPCState.End))
            {
                foreach (NPCAttackMode npc in NPCs)
                {
                    npc.state = NPCAttackMode.NPCState.End;
                    npc.ResetOnElimination(); 
                }
                NPCAttackMode.OnTargetElminated?.Invoke(); 
                idleTimer = 0f;
                IsElminated = true;
                return;
            }

            if (!catalog.ContainsKey(NPCAttackMode.NPCState.Finder) && PlayerVisibilityManager.PlayerVisibilityState == PlayerVisibilityState.Detected)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleDelay)
                {
                    PlayerVisibilityManager.PlayerVisibilityState = PlayerVisibilityState.Default;
                    PlayerVisibilityManager.OnVisibillityAdded?.Invoke(-100);
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