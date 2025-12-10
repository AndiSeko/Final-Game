using ASUL.Interactions;
using UnityEngine;

namespace ASUL.Dieplome.NeuroChip
{
    public class NeuroChipVisible : MonoBehaviour
    {
        [SerializeField] private RenderingLayerMask OutlineType;

        private MeshRenderer[] renderers;

        private void Awake()
        {
            UpdateRenderers();
        }

        public void ToggleVisible(bool value)
        {
            UpdateRenderers();

            foreach (var renderer in renderers)
            {
                if (value)
                {
                    renderer.renderingLayerMask |= (uint)OutlineType;
                }
                else
                {
                    renderer.renderingLayerMask &= ~(uint)OutlineType;
                }
            }
        }

        private void UpdateRenderers()
        {
            renderers = GetComponentsInChildren<MeshRenderer>();
        }

        
    }
}
