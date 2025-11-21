using UnityEngine;

namespace ASUL.FPC.Animations
{
    public class BodyControlRequest
    {
        public BodyControlPreset preset;
        public Transform target;
        public int priority = 100;

        public float currentWeight = 0f;
        public float targetWeight = 1f;

        public Vector3 smoothedPosition;
        public Quaternion smoothedRotation;

        public void UpdateWeight(float dt)
        {
            currentWeight = Mathf.MoveTowards(currentWeight, targetWeight, preset.smoothSpeed * dt);
        }

        public void SmoothTowardsTarget(float dt)
        {
            float factor = Mathf.Clamp01(preset.smoothSpeed * dt);
            smoothedPosition = Vector3.Lerp(smoothedPosition, target.position, factor);
            smoothedRotation = Quaternion.Slerp(smoothedRotation, target.rotation, factor);
        }

        public void Apply(Animator animator)
        {
            if (target == null || currentWeight < 0.01f) return;

            float smoothFactor = Mathf.Clamp01(preset.smoothSpeed * Time.deltaTime);
            smoothedPosition = Vector3.Lerp(smoothedPosition, target.position, smoothFactor);
            smoothedRotation = Quaternion.Slerp(smoothedRotation, target.rotation, smoothFactor);

            switch (preset.type)
            {
                case BodyControlPreset.Type.IKGoal:
                    animator.SetIKPosition(preset.goal, smoothedPosition);
                    animator.SetIKRotation(preset.goal, smoothedRotation);
                    animator.SetIKPositionWeight(preset.goal, currentWeight);
                    animator.SetIKRotationWeight(preset.goal, currentWeight);
                    break;

                case BodyControlPreset.Type.LookAt:
                    animator.SetLookAtWeight(currentWeight, preset.bodyWeight, preset.headWeight, preset.headWeight, 0.5f);
                    animator.SetLookAtPosition(smoothedPosition);
                    break;
            }
        }
    }
}