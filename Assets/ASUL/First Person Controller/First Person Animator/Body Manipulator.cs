using System;
using UnityEngine;

namespace ASUL.FPC.Animations
{
    public class BodyManipulator : MonoBehaviour
    {
        private Animator animator;
        [field: SerializeField] public bool IsActive { get; set; } = true;

        [SerializeField] private BodyManipulatorConnection[] connections;

        private void Start()
        {
            animator = BodyDatabase.instance.GetAnimator();
            FirstPersonAnimator.OnExternalKinemation += IK;
        }

        private void IK(Animator animator)
        {
            if (!IsActive || animator == null) return;

            foreach (var connection in connections)
            {
                BodyConnector.Connect(connection, animator);
            }
        }

        private void OnDestroy()
        {
            FirstPersonAnimator.OnExternalKinemation -= IK;
        }
    }

    public static class BodyConnector
    {
        public static void Connect(BodyManipulatorConnection connection, Animator animator)
        {
            if (connection.Target == null) return;

            switch (connection.Type)
            {
                case BodyManipulatorConnection.ConnectionType.IKGoal:
                    animator.SetIKPositionWeight(connection.Goal, connection.Weight);
                    animator.SetIKRotationWeight(connection.Goal, connection.Weight);

                    animator.SetIKPosition(connection.Goal, connection.Target.position);
                    animator.SetIKRotation(connection.Goal, connection.Target.rotation);
                    break;

                case BodyManipulatorConnection.ConnectionType.Head:
                    animator.SetLookAtWeight(connection.Weight);
                    animator.SetLookAtPosition(connection.Target.position);
                    break;

                case BodyManipulatorConnection.ConnectionType.Body:
                    Transform body = BodyDatabase.instance.GetBody();
                    if (body != null)
                    {
                        body.rotation = Quaternion.RotateTowards(
                            body.rotation,
                            Quaternion.LookRotation(connection.Target.position - body.position),
                            5f
                        );
                    }
                    break;
            }
        }
    }

    [Serializable]
    public class BodyManipulatorConnection
    {
        public enum ConnectionType
        {
            IKGoal,  
            Head,
            Body 
        }

        public ConnectionType Type;
        public AvatarIKGoal Goal; 
        public Transform Target;
        [Range(0f, 1f)] public float Weight = 1f;
    }
}
