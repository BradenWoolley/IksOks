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
    private Slider bgmToggle;

    [SerializeField]
    private Slider sfxToggle;

    #endregion


    #region Methods

    public static float IsBGMEnabled()
    {
        //return PlayerPrefs.GetInt(BGM_KEY, 1) == 1;
        return PlayerPrefs.GetFloat(BGM_KEY, 1f);
    }

    public static float IsSFXEnabled()
    {
        //return PlayerPrefs.GetInt(SFX_KEY, 1) == 1;
        return PlayerPrefs.GetFloat(SFX_KEY, 1f);
    }

    protected override void Awake()
    {
        base.Awake();

        closeButton?.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlayButtonSFX();
            Hide();
        });

        bgmToggle?.onValueChanged.AddListener(OnBGMToggled);
        sfxToggle?.onValueChanged.AddListener(OnSFXToggled);
    }

    protected override void OnBeforeShow()
    {
        // Temporarily remove listeners to prevent triggering audio calls on load
        bgmToggle?.onValueChanged.RemoveListener(OnBGMToggled);
        sfxToggle?.onValueChanged.RemoveListener(OnSFXToggled);

        if (bgmToggle)
        {
            //bgmToggle.value = (PlayerPrefs.GetFloat(BGM_KEY, 1) == 1);
            bgmToggle.value = PlayerPrefs.GetFloat(BGM_KEY, 1f);
        }

        if (sfxToggle)
        {
            //sfxToggle.value = PlayerPrefs.GetInt(SFX_KEY, 1) == 1;
            sfxToggle.value = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        }

        bgmToggle?.onValueChanged.AddListener(OnBGMToggled);
        sfxToggle?.onValueChanged.AddListener(OnSFXToggled);
    }

    private void OnBGMToggled(float toggle)
    {
        PlayerPrefs.SetFloat(BGM_KEY, toggle /*? 1 : 0*/);
        PlayerPrefs.Save();
        if (toggle.Equals(1f))
        {
            AudioManager.Instance?.PlayBGM();
        }

        else
        {
            AudioManager.Instance?.StopBGM();
        }
    }

    private void OnSFXToggled(float toggle)
    {
        PlayerPrefs.SetFloat(SFX_KEY, toggle /*isOn ? 1 : 0*/);
        PlayerPrefs.Save();
        // AudioManager will read this key before playing any SFX.
    }

    #endregion

}