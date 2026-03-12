using Lean.Localization;
using UnityEngine;


public class LanguageManager : MonoBehaviour
{

    #region Fields

    private LeanLocalization localization;

    #endregion


    #region Properties

    public static LanguageManager Instance { get; private set; }

    #endregion


    #region Methods

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string GetCurrentLanguage()
    {
        return localization.CurrentLanguage;
    }

    /// <summary>
    /// Change to the desired language.
    /// </summary>
    /// <param name="language"></param>
    public void SwitchLanguages(string language)
    {
        localization.CurrentLanguage = language;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);

        localization = GetComponent<LeanLocalization>();
    }

    #endregion

}