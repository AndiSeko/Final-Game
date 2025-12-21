using TMPro;
using UnityEngine;

namespace ASUL.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMPLocalizator : MonoBehaviour
    {
        private TextMeshProUGUI m_TextMeshPro;
        [SerializeField] private SerializableDictionary<string, string> localizationValues;


        private void Awake()
        {
            m_TextMeshPro = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            LocalizationManager.OnLanguageChanged += SetLocalization;
        }

        private void OnDisable()
        {
            LocalizationManager.OnLanguageChanged -= SetLocalization;
        }


        private void Start()
        {
            SetLocalization(LocalizationManager.Instance.CurrentLanguageCode);
        }

        private void SetLocalization(string localization)
        {
            if (localizationValues[localization] != null)
            {
                m_TextMeshPro.text = localizationValues[localization];
            }
            else
            {
                m_TextMeshPro.text = "[Localization Error]";
            }
        }
    }
}