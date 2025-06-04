using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Localization;

public class LanguageDropDown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdownLanguage;
    
    IEnumerator Start()
    {
        // Wait for the localization system to initialize.
        yield return LocalizationSettings.InitializationOperation;

        // Generate list of available Locales
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        int selectedlanguage = 0;

        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            Locale locale = LocalizationSettings.AvailableLocales.Locales[i];
            options.Add(new TMP_Dropdown.OptionData(locale.name));

            if (locale == LocalizationSettings.SelectedLocale) 
            {
                selectedlanguage = i;
            }
        }

        dropdownLanguage.options = options;
        dropdownLanguage.value = selectedlanguage;
        dropdownLanguage.onValueChanged.AddListener(LocaleSelected);
    }
    static void LocaleSelected(int index)
    {
        GameManager.Instance.ChangeLanguage(index);
    }
}
