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
            Destroy(this);
        }

        else
        {
            Instance = this;
        }

        localization = GetComponent<LeanLocalization>();
    }

    #endregion

}