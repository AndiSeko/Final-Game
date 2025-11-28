using UnityEngine;
using UnityEngine.Rendering;

namespace ASUL.NPC.PlayerDetection
{
    public class PlayerVisibilityViewier : MonoBehaviour
    {
        [SerializeField] private Volume volume;

        [SerializeField] private float smoothSpeed = 10f;

        private float targetWeight;
        private float currentWeight;

        private void Update()
        {
            currentWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * smoothSpeed);
            volume.weight = currentWeight;
        }

        public void SetVolumeValue(float value)
        {
            float v = 0.5f + ((value / 100f) / 2);

            v = value <= 0 ? 0 : v;

            targetWeight = Mathf.Clamp01(v);
        }
    }
}
