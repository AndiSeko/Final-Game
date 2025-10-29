using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


namespace ASUL.InputSystem 
{
    public class InputManager : MonoBehaviour
    {
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
            ChangeInputMap("Player");

        }

        private void OnDestroy()
        {
            if (Instance = this)
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

        public IEnumerable<string> GetAvailableMaps() => maps.Keys;
    }
}