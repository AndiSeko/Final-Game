using ASUL.Console;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace ASUL.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }

        public string CurrentLanguageCode { get; private set; } = "en";

        public static System.Action<string> OnLanguageChanged;

        public Locale CurrentLocale => LocalizationSettings.SelectedLocale;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            StartCoroutine(InitializeLocalization());
        }

        private IEnumerator InitializeLocalization()
        {
            yield return LocalizationSettings.InitializationOperation;

            UpdateCurrentLanguageCode();

            LocalizationSettings.SelectedLocaleChanged += OnUnityLocaleChanged;
        }

        private void OnUnityLocaleChanged(Locale newLocale)
        {
            UpdateCurrentLanguageCode();
        }

        private void UpdateCurrentLanguageCode()
        {
            var selected = LocalizationSettings.SelectedLocale;
            if (selected == null)
            {
                Debug.LogWarning("Выбранная локализация равна null!");
                return;
            }

            string newCode = selected.Identifier.Code;

            if (newCode != CurrentLanguageCode)
            {
                CurrentLanguageCode = newCode;
                OnLanguageChanged?.Invoke(newCode);
            }
        }
        public void SetLanguage(string languageCode)
        {
            Locale targetLocale = LocalizationSettings.AvailableLocales.Locales
                .Find(l => l.Identifier.Code.StartsWith(languageCode, System.StringComparison.OrdinalIgnoreCase));

            if (targetLocale != null)
            {
                LocalizationSettings.SelectedLocale = targetLocale;
            }
            else
            {
                Debug.LogWarning($"Локализация '{languageCode}' не найдена!");
            }
        }

        public void SetLocale(Locale locale)
        {
            if (locale != null && LocalizationSettings.AvailableLocales.Locales.Contains(locale))
            {
                LocalizationSettings.SelectedLocale = locale;
            }
        }


        public void LoadSavedLanguage()
        {
            if (PlayerPrefs.HasKey("Localization"))
            {
                SetLanguage(PlayerPrefs.GetString("Localization"));
            }
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

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                SetLanguage(CurrentLanguageCode.StartsWith("ru") ? "en" : "ru");
            }
        }
#endif
    }
}