using System;
using UnityEngine;

namespace ASUL.EventSystem
{
    [CreateAssetMenu(fileName = "EventFile", menuName = "ASUP/EventFile"), Serializable]
    public class EventFile : ScriptableObject
    {
        public EventStep[] events;

        public void LaunchEvent()
        {
            EventManager.LaunchEvent(events);
        }
    }
}
