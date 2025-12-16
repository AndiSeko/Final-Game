using System;
using UnityEngine;

namespace ASUL.NPC.PlayerDetection 
{
    public class PlayerVisibilityManager : MonoBehaviour
    {
        public static Action OnPlayerDetected;

        public static PlayerVisibilityState PlayerVisibilityState = PlayerVisibilityState.Default;
        
        [SerializeField] private PlayerVisibilityState _defaultPlayerVisibilityState = PlayerVisibilityState.Default;

        [SerializeField] private PlayerVisibilityViewier viewier;

        [SerializeField] private float decayPerSecond = 5f;

        private float _visibillityProgress;

        public static Action<float> OnVisibillityAdded;

        private void AddVisibilityProgress(float value)
        {
            float totalValue = CalculateTotalValue(value);

            if (_visibillityProgress + totalValue >= 100f)
            {
                if (_visibillityProgress == 100f)
                    return;

                _visibillityProgress = 100;
                AICommandCenter.PlayerIsDetected = true;
                OnPlayerDetected?.Invoke();

                Debug.Log("Вас обнаружили!");

                return;
            }
            if (_visibillityProgress + totalValue <= 0)
            {
                _visibillityProgress = 0;

                if (viewier != null)
                {
                    viewier.SetVolumeValue(_visibillityProgress);
                }

                return;
            }

            _visibillityProgress += totalValue;

            if (viewier != null)
            {
                viewier.SetVolumeValue(_visibillityProgress);
            }

            Debug.Log(_visibillityProgress);
        }

        private float CalculateTotalValue(float value)
        {
            if (value < 0)
            {
                if (AICommandCenter.PlayerIsDetected)
                {
                    return 0;
                }

                return value;
            }

            float v = 0f;

            switch (PlayerVisibilityState)
            {
                case PlayerVisibilityState.Default:
                    v = value; break;
                case PlayerVisibilityState.Crouch:
                    v = value - (value * 25f / 100f); break;
            }

            return v;
        }

        private void Update()
        {
            if (_visibillityProgress > 0f)
            {
                AddVisibilityProgress(-decayPerSecond * Time.deltaTime);
            }
        }

        private void Awake()
        {
            PlayerVisibilityState = _defaultPlayerVisibilityState;
            OnVisibillityAdded += AddVisibilityProgress;
        }

        private void OnDisable()
        {
            OnVisibillityAdded -= AddVisibilityProgress;
        }


    }

    public enum PlayerVisibilityState
    {
        Default,
        Crouch,
        NeurochipInvisible,
        ConsoleInvisible,
        Dead
    }
}