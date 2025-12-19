using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ASUL.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMPLocalizator : MonoBehaviour
    {
        private TextMeshProUGUI m_TextMeshPro;
        [SerializeField] private SerializableDictionary<Localization, string> localizationValues;


        private void Awake()
        {
            m_TextMeshPro = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            LocalizationManager.OnLocalizationChanged += SetLocalization;
        }

        private void OnDisable()
        {
            LocalizationManager.OnLocalizationChanged -= SetLocalization;
        }


        private void Start()
        {
            SetLocalization(LocalizationManager.Instance.Localization);
        }

        private void SetLocalization(Localization localization)
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