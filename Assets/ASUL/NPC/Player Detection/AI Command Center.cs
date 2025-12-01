using UnityEngine;
using System.Collections.Generic;

namespace ASUL.NPC
{
    public class AICommandCenter : MonoBehaviour
    {


        [Header("Настройки")]
        [SerializeField] private float updateFrequency = 0.5f;

        [SerializeField] private NPCAttackMode[] registeredNPCs;
        private float updateTimer;

        private void Update()
        {
            updateTimer += Time.deltaTime;
            if (updateTimer >= updateFrequency)
            {
                updateTimer = 0f;
                UpdateNPCOrders();
            }
        }

        private void UpdateNPCOrders()
        {
            int detectingCount = 0;
            foreach (var npc in registeredNPCs)
            {
                if (npc.DetectsPlayer())
                {
                    detectingCount++;
                }
            }

            foreach (var npc in registeredNPCs)
            {
                if (npc.DetectsPlayer())
                {
                    if (detectingCount == 1)
                    {
                        npc.SetMode(NPCAttackMode.NPCMode.Attack);
                    }
                    else
                    {
                        npc.SetMode(NPCAttackMode.NPCMode.Approach);
                    }
                }
                else
                {
                    npc.SetMode(NPCAttackMode.NPCMode.Idle);
                }
            }
        }
    }
}