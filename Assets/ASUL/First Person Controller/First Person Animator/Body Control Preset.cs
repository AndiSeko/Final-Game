using UnityEngine;

namespace ASUL.FPC.Animations
{
    [CreateAssetMenu(menuName = "ASUL/Body Control Preset", fileName = "New Body Preset")]
    public class BodyControlPreset : ScriptableObject
    {
        public enum Type { IKGoal, LookAt, BodyRotation }

        public Type type;
        public AvatarIKGoal goal = AvatarIKGoal.RightHand;

        [Range(0f, 1f)] public float weight = 1f;
        [Range(0f, 1f)] public float bodyWeight = 0.4f;
        [Range(0f, 1f)] public float headWeight = 1f;

        public bool smooth = true;
        [Range(1f, 30f)] public float smoothSpeed = 12f;

        [Range(0.05f, 0.5f)] public float lerpFactor = 0.2f;
    }
}