using UnityEngine;

namespace ASUL.FPC
{
    [RequireComponent(typeof(Collider))]
    public class StealthLockZone : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ToggleLock(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ToggleLock(false);
            }
        }


        private void ToggleLock(bool value)
        {
            Stealth[] stealths = FindObjectsByType<Stealth>(FindObjectsSortMode.None);

            foreach (Stealth stealth in stealths)
            {
                stealth.IsCrouchLocked = value;
            }
        }

        public static void ToggleStaticLock(bool value)
        {
            Stealth[] stealths = FindObjectsByType<Stealth>(FindObjectsSortMode.None);

            foreach (Stealth stealth in stealths)
            {
                stealth.IsCrouchLocked = value;
            }
        }
    }
}