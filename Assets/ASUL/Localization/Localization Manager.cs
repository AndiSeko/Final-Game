using System;
using UnityEngine;

namespace ASUL.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }
        public Localization Localization { get => _localization; set { SetLocalization(value); } }

        private Localization _localization = Localization.en;

        public static Action<Localization> OnLocalizationChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            string localization = PlayerPrefs.GetString("Localization", "en");

            if (System.Enum.TryParse(localization, out Localization parsedLocalization))
            {
                Localization = parsedLocalization;
            }
            else
            {
                Localization = Localization.en;
            }
        }

        public void SetLocalization(Localization language)
        {
            _localization = language;
            PlayerPrefs.SetString("Localization", language.ToString());
            OnLocalizationChanged?.Invoke(language);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoCreate()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject(nameof(LocalizationManager));
                go.AddComponent<LocalizationManager>();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (Localization == Localization.ru)
                    Localization = Localization.en;
                else
                    Localization = Localization.ru;
            }
        }
    }

    public enum Localization
    {
        ru,
        en
    }
}
