using ASUL.FPC.Animations;
using System.Collections.Generic;
using UnityEngine;

namespace ASUL.FPC.Animations
{
    public class BodyManipulator : MonoBehaviour
    {
        [SerializeField] private BodyControlPreset[] presets;
        [SerializeField] private Transform[] targets;
        [SerializeField] private int priority = 100;

        [SerializeField] private bool _isActive = false;
        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        private bool currentActiveState = false;
        private readonly List<BodyControlRequest> myRequests = new();

        private void Update()
        {
            if (currentActiveState != _isActive)
            {
                if (_isActive) Activate();
                else Deactivate();
                currentActiveState = _isActive;
            }
        }

        public void Activate()
        {
            if (myRequests.Count > 0) return;

            for (int i = 0; i < presets.Length && i < targets.Length; i++)
            {
                if (presets[i] == null || targets[i] == null) continue;

                var req = new BodyControlRequest
                {
                    preset = presets[i],
                    target = targets[i],
                    priority = priority,
                    currentWeight = 0f,
                    targetWeight = 1f
                };

                myRequests.Add(req);
                BodyControlSystem.Instance.Register(req);
            }
        }

        public void Deactivate()
        {
            foreach (var req in myRequests)
                req.targetWeight = 0f;

            myRequests.Clear();
        }

        private void OnDestroy()
        {
            foreach (var req in myRequests)
                BodyControlSystem.Instance.Unregister(req);
            myRequests.Clear();
        }
    }
}