using UnityEngine;

namespace ASUL.FPC.Animations
{
    public class BodyDatabase : MonoBehaviour
    {
        public static BodyDatabase instance; // Синглтон, доступный в пределах сцены

        [SerializeField] private Animator animator;

        [SerializeField] private Transform body;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;
        [SerializeField] private Transform head;

        public Animator GetAnimator() => animator;
        public Transform GetBody() => body;
        public Transform GetLeftHand() => leftHand;
        public Transform GetRightHand() => rightHand;
        public Transform GetHead() => head;

        private void Awake()
        {
            instance = this;
        }
    }
}
