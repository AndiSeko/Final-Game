using UnityEngine;


namespace ASUL.FPC.Animations
{
    [RequireComponent(typeof(Collider))]
    public class ManipulatorTrigger : MonoBehaviour
    {
        [SerializeField] private BodyManipulator _bodyManipulator;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) _bodyManipulator.IsActive = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) _bodyManipulator.IsActive = false;
        }
    }
}