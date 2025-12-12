using System.Collections.Generic;
using UnityEngine;

namespace ASUL.DNS
{
    public class DialogueGroupSO : ScriptableObject
    {
        [field: SerializeField] public string GroupName { get; set; }

        public void Initialize(string groupName)
        {
            GroupName = groupName;
        }
    }
}