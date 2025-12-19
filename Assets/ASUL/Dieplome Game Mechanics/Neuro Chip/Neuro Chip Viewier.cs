using ASUL.Canvas;
using MPUIKIT;
using UnityEngine;
using UnityEngine.Rendering;

namespace ASUL.Dieplome.NeuroChip
{
    public class NeuroChipViewer : CanvasGroupLauncher
    {
        private float visibility = 0f;

        [SerializeField] private float smoothSpeed = 10f;

        [SerializeField] private Volume volume;

        [SerializeField] private MPImage charge;

        private float targetWeight = 0;

        private float targetCharge = 0;

        public bool IsView { get; set; } = false;

        private void Update()
        {
            targetWeight = IsView ? 1f : 0f;

            visibility = Mathf.MoveTowards(visibility, targetWeight, Time.deltaTime * smoothSpeed);

            volume.weight = visibility;

            charge.fillAmount = Mathf.Lerp(charge.fillAmount, targetCharge, Time.deltaTime * smoothSpeed);
        }

        public void SetChargeView(float value)
        {
            targetCharge = Mathf.Clamp01(value);
        }
    }
}
