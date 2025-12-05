using ASUL.Dieplome.NeuroChip;
using UnityEngine;
using UnityEngine.Events;

namespace ASUL.NPC.PlayerDetection
{
    public class AIObserver : MonoBehaviour
    {
        [Header("Цель")]
        public Transform Target;

        [Header("Настройки видимости")]
        [SerializeField] private float viewRadius = 12f;
        [SerializeField] private float viewAngle = 60f;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private float visibilityValue = 5f;
        [SerializeField] private float unchargeValue = 0.5f;

        [Header("Частота сканирования")]
        public float scanFrequency = 10f;

        [Header("События")]
        public UnityEvent onTargetSpotted;
        public UnityEvent onTargetLost;
        public UnityEvent onAllyDetected;

        [Header("Debug")]
        public bool drawGizmos = true;

        public bool HasLineOfSight { get; private set; }
        public bool IsInFov { get; private set; }
        public bool IsInRange { get; private set; }

        public bool IsInFovIsVisible => IsInFov && PlayerIsVisible;

        public bool IsTargetDetectedIsVisible => IsTargetDetected && PlayerIsVisible;

        public bool IsTargetDetected => HasLineOfSight && IsInFov && IsInRange;

        private float _scanTimer;
        private bool _lastDetected;

        public Transform AllyWhoDetect { get; private set; }

        private bool PlayerIsVisible => !(PlayerVisibilityManager.PlayerVisibilityState == PlayerVisibilityState.NeurochipInvisible || PlayerVisibilityManager.PlayerVisibilityState == PlayerVisibilityState.ConsoleInvisible);

        private void Awake()
        {
            if (Target == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player) Target = player.transform;
            }
        }

        private void Update()
        {
            _scanTimer += Time.deltaTime;
            if (_scanTimer >= (1f / scanFrequency))
            {
                _scanTimer = 0f;
                Scan();

                FindAllyWhoDetect();

                if (IsTargetDetected)
                {

                    if (PlayerIsVisible)
                    {
                        PlayerVisibilityManager.OnVisibillityAdded.Invoke(visibilityValue);
                    }
                    else if (PlayerVisibilityManager.PlayerVisibilityState == PlayerVisibilityState.NeurochipInvisible)
                    {
                        NeuroChip.OnChargeChanged?.Invoke(-unchargeValue);
                    }
                }
                else
                {
                    PlayerVisibilityManager.OnVisibillityAdded.Invoke(-visibilityValue + (visibilityValue * 50f / 100f));
                }

                if (IsTargetDetected != _lastDetected)
                {
                    if (IsTargetDetected) onTargetSpotted?.Invoke();
                    else onTargetLost?.Invoke();
                    _lastDetected = IsTargetDetected;
                }

                if (AllyWhoDetect != null)
                {
                    onAllyDetected?.Invoke();
                }

            }

            if (IsTargetDetected != _lastDetected)
            {
                if (IsTargetDetected) onTargetSpotted?.Invoke();
                else onTargetLost?.Invoke();
                _lastDetected = IsTargetDetected;
            }
        }

        private void FindAllyWhoDetect()
        {
            AllyWhoDetect = null;

            Collider[] allies = Physics.OverlapSphere(transform.position, viewRadius);
            float closestDist = float.MaxValue;

            foreach (var col in allies)
            {
                AIObserver ally = col.GetComponent<AIObserver>();
                if (ally != null && ally != this && ally.IsTargetDetected)
                {
                    float dist = Vector3.Distance(transform.position, ally.transform.position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        AllyWhoDetect = ally.transform;
                    }
                }
            }
        }

        private void Scan()
        {
            if (Target == null)
            {
                ResetChecks();
                return;
            }

            Vector3 origin = transform.position;
            Vector3 toTarget = Target.position - origin;
            float dist = toTarget.magnitude;

            IsInRange = dist <= viewRadius;
            if (!IsInRange)
            {
                HasLineOfSight = false;
                IsInFov = false;
                return;
            }

            Vector3 forward = transform.forward;
            float angle = Vector3.Angle(forward, toTarget);
            IsInFov = angle <= viewAngle;


            if (!IsInFov)
            {
                HasLineOfSight = false;
                return;
            }

            Vector3 targetPoint = GetTargetAimPoint(Target);
            Vector3 dir = (targetPoint - origin).normalized;
            float rayDist = Vector3.Distance(origin, targetPoint);

            if (Physics.Raycast(origin, dir, out RaycastHit hit, rayDist, obstacleMask | targetMask, QueryTriggerInteraction.Ignore))
            {
                HasLineOfSight = hit.transform == Target;
            }
            else
            {
                HasLineOfSight = true;
            }
        }

        private Vector3 GetTargetAimPoint(Transform t)
        {
            if (t.TryGetComponent<Collider>(out var col))
            {
                var center = col.bounds.center;
                var top = center + Vector3.up * col.bounds.extents.y * 0.8f;
                return top;
            }
            return t.position + Vector3.up * 1.6f; 
        }

        private void ResetChecks()
        {
            IsInRange = false;
            IsInFov = false;
            HasLineOfSight = false;
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos) return;

            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, viewRadius);

            Vector3 origin = transform.position;
            Vector3 leftDir = Quaternion.Euler(0, -viewAngle, 0) * transform.forward;
            Vector3 rightDir = Quaternion.Euler(0, viewAngle, 0) * transform.forward;
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(origin, leftDir * viewRadius);
            Gizmos.DrawRay(origin, rightDir * viewRadius);

            if (Target != null)
            {
                Gizmos.color = IsTargetDetected ? Color.green : Color.red;
                Gizmos.DrawLine(origin, GetTargetAimPoint(Target));
            }
        }

    }
}