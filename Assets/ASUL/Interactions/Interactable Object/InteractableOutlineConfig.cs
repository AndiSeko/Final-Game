using UnityEngine;

namespace ASUL.Interactions
{
    [CreateAssetMenu(fileName = "InteractableConfig", menuName = "ASUL/InteractableConfig")]
    public class InteractableOutlineConfig : ScriptableObject
    {
        [Range(0f, 10f)]
        public float OutlineWidth = 1f;

        public Color OutlineColor = Color.white;
    }
}