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
        playButton?.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlayButtonSFX();
            themeSelectionPopup?.Show();
        });

        statsButton?.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlayButtonSFX();
            statsPopup?.Show();
        });

        settingsButton?.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlayButtonSFX();
            settingsPopup?.Show();
        });

        exitButton?.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlayButtonSFX();
            exitConfirmPopup?.Show();
        });
    }

    private void Start()
    {
        AudioManager.Instance?.PlayBGM();
    }

    #endregion

}