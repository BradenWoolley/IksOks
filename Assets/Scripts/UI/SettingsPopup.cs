using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;


public class SettingsPopup : PopupBase
{

    #region Consts

    private const string BGM_KEY = "settings_bgm";

    private const string SFX_KEY = "settings_sfx";

    #endregion


    #region Fields

    [Header("Close Button")]
    [SerializeField]
    private Button closeButton;

    [Header("Toggles")]
    [SerializeField]
    private LeanToggle bgmToggle;

    [SerializeField]
    private LeanToggle sfxToggle;

    #endregion


    #region Methods

    public static bool IsBGMEnabled()
    {
        return PlayerPrefs.GetInt(BGM_KEY, 1) == 1;
    }

    public static bool IsSFXEnabled()
    {
        return PlayerPrefs.GetInt(SFX_KEY, 1) == 1;
    }

    protected override void Awake()
    {
        base.Awake();

        closeButton?.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlayButtonSFX();
            Hide();
        });
    }

    protected override void OnBeforeShow()
    {
        bgmToggle.On = PlayerPrefs.GetInt(BGM_KEY, 1) == 1;
        sfxToggle.On = PlayerPrefs.GetInt(SFX_KEY, 1) == 1;
    }

    public void OnBGMToggled(bool isOn)
    {
        PlayerPrefs.SetInt(BGM_KEY, isOn ? 1 : 0);
        PlayerPrefs.Save();

        if (isOn)
        {
            AudioManager.Instance?.PlayBGM();
        }

        else
        {
            AudioManager.Instance?.StopBGM();
        }
    }

    public void OnSFXToggled(bool isOn)
    {
        PlayerPrefs.SetInt(SFX_KEY, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    #endregion

}