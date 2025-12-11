using ASUL.Interactions;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ASUL.Dieplome.NeuroChip
{
    public class NeuroChipVisible : MonoBehaviour
    {
        [SerializeField] private RenderingLayerMask OutlineType;

        private List<Renderer> renderers = new List<Renderer>();

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
            renderers.Clear();
            renderers.AddRange(GetComponentsInChildren<MeshRenderer>());
            renderers.AddRange(GetComponentsInChildren<SkinnedMeshRenderer>());
        }
        
    }
}
