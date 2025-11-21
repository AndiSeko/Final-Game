using UnityEngine;

namespace ASUL.FPC.Animations
{
    public class BodyDatabase : MonoBehaviour
    {
        public static BodyDatabase instance; // Синглтон, доступный в пределах сцены

        [SerializeField] private Animator animator;

        [SerializeField] private Transform body;

        public Animator Animator => animator;
        public Transform Body => body;

        private void Awake()
        {
            instance = this;
        }
    }
}
