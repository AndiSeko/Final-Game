using UnityEngine;

namespace ASUL.FPC.Animations
{
    public class BodyDatabase : MonoBehaviour
    {
        public static BodyDatabase instance; // Синглтон, доступный в пределах сцены

        [SerializeField] private Animator animator;

        [SerializeField] private Transform body;

        [SerializeField] private Transform cameraTransform;

        [SerializeField] private Stealth stealth;

        public Animator Animator => animator;
        public Transform Body => body;
        public Transform Camera => cameraTransform;
        public Stealth Stealth { get { return stealth; } set { stealth = value; } }

        private void Awake()
        {
            instance = this;
        }
    }
}
