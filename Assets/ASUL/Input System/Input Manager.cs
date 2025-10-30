using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace ASUL.InputSystem 
{
    public class InputManager : MonoBehaviour
    {
        // По желанию можно изменить 
        private string defaultInputMap = "Player";
        private CursorLockMode defaultCursorLockMode = CursorLockMode.Locked;

        public static InputManager Instance { get; private set; }

        public InputSystem_Actions actions;

        private Dictionary<string, InputActionMap> maps;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            if (Instance != null) return;

            GameObject go = new("Input Manager");

            Instance = go.AddComponent<InputManager>();

            DontDestroyOnLoad(go);
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            actions = new();
            actions.Enable();

            maps = new Dictionary<string, InputActionMap>();
            foreach (var map in actions.asset.actionMaps)
            {
                maps[map.name] = map;
            }

            ChangeInputMap(defaultInputMap, defaultCursorLockMode);

        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                actions.Disable();
                actions = null;
                Instance = null;
            }
        }

        public void ChangeInputMap(string mapName)
        {
            if (!maps.ContainsKey(mapName))
            {
                Debug.LogWarning($"Input map '{mapName}' not found!");
                return;
            }

            foreach (var map in maps.Values)
                map.Disable();

            maps[mapName].Enable();
        }

        public void ChangeInputMap(string mapName, CursorLockMode cursorLockMode)
        {
            if (!maps.ContainsKey(mapName))
            {
                Debug.LogWarning($"Input map '{mapName}' не найден!");
                return;
            }

            foreach (var map in maps.Values)
                map.Disable();

            maps[mapName].Enable();

            Cursor.lockState = cursorLockMode;

            Cursor.visible = cursorLockMode != CursorLockMode.Locked;
        }

        public IEnumerable<string> GetAvailableMaps() => maps.Keys;
    }
}