using ASUL.InputSystem;
using UnityEngine;

namespace ASUL.InitialScripts{
    public class PlayerSceneInitialyzer : MonoBehaviour
    {
        [SerializeField] private string defaultInputMap = "Player";
        [SerializeField] private CursorLockMode defaultCursorLockMode = CursorLockMode.None;

        private protected virtual void Start()
        {
            InputManager.Instance.ChangeInputMap(defaultInputMap, defaultCursorLockMode);
        }
    }

}