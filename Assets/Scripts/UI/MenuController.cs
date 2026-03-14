using UnityEngine;
using UnityEngine.UI;


public class MenuController : MonoBehaviour
{

    #region Fields

    [Header("Buttons")]
    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button settingsButton;

    [SerializeField]
    private Button statsButton;

    [Header("Popups")]
    [SerializeField]
    private ExitConfirmPopup exitConfirmPopup;

    [SerializeField]
    private SettingsPopup settingsPopup;

    [SerializeField]
    private StatsPopup statsPopup;

    [SerializeField]
    private ThemeSelectionPopup themeSelectionPopup;

    #endregion


    #region Methods

    private void Awake()
    {
        exitButton?.onClick.AddListener(OnExitClicked);
        playButton?.onClick.AddListener(OnPlayClicked);
        settingsButton?.onClick.AddListener(OnSettingsClicked);
        statsButton?.onClick.AddListener(OnStatsClicked);
    }

    private void OnDestroy()
    {
        exitButton?.onClick.RemoveListener(OnExitClicked);
        playButton?.onClick.RemoveListener(OnPlayClicked);
        settingsButton?.onClick.RemoveListener(OnSettingsClicked);
        statsButton?.onClick.RemoveListener(OnStatsClicked);
    }

    private void OnExitClicked()
    {
        AudioManager.Instance?.PlayButtonSFX();
        exitConfirmPopup?.Show();
    }

    private void OnPlayClicked()
    {
        AudioManager.Instance?.PlayButtonSFX();
        themeSelectionPopup?.Show();
    }

    private void OnSettingsClicked()
    {
        AudioManager.Instance?.PlayButtonSFX();
        settingsPopup?.Show();
    }

    private void OnStatsClicked()
    {
        AudioManager.Instance?.PlayButtonSFX();
        statsPopup?.Show();
    }

    private void Start()
    {
        AudioManager.Instance?.PlayBGM();
    }

    #endregion

}