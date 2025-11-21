using UnityEngine;
using System.Collections.Generic;

namespace ASUL.FPC.Animations
{
    public class BodyControlSystem : MonoBehaviour
    {
        public static BodyControlSystem Instance { get; private set; }

        private readonly List<BodyControlRequest> requests = new();
        private Animator animator;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            animator = GetComponent<Animator>();
        }

        public void Register(BodyControlRequest req) => requests.Add(req);

        public void Unregister(BodyControlRequest req) => requests.Remove(req);

        public void UnregisterAllFrom(MonoBehaviour owner)
        {
            requests.RemoveAll(r => r.target != null && r.target.IsChildOf(owner.transform));
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (requests.Count == 0) return;

            requests.Sort((a, b) => b.priority.CompareTo(a.priority));

            float dt = Time.deltaTime;

            foreach (var req in requests)
            {
                req.UpdateWeight(dt);
                req.Apply(animator);
            }

            requests.RemoveAll(r => r.currentWeight <= 0.01f && r.targetWeight <= 0.01f);
        }
    }
}