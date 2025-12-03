using UnityEngine;
using UnityEngine.Rendering;

namespace ASUL.Dieplome.NeuroChip
{
    public class NeuroChipViewier : MonoBehaviour
    {
        private float visibility = 0f;

        [SerializeField] private float smoothSpeed = 10f;

        [SerializeField] private Volume volume;

        private float targetWeight = 0;

        public bool IsView { get; set; } = false;

        private void Update()
        {
            targetWeight = IsView ? 1f : 0f;

            visibility = Mathf.MoveTowards(visibility, targetWeight, Time.deltaTime * smoothSpeed);

            volume.weight = visibility;
        }
    }
}
