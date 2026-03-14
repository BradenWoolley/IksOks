using UnityEngine;
using UnityEngine.UI;


public class ThemeSelectionPopup : PopupBase
{

    #region Fields

    [Header("Player Groups")]
    [SerializeField]
    private ThemeSelectorGroup player1Group;

    [SerializeField]
    private ThemeSelectorGroup player2Group;

    [Header("Navigation")]
    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private Button startButton;

    #endregion


    #region Methods

    protected override void Awake()
    {
        base.Awake();
        startButton?.onClick.AddListener(OnStartClicked);
        cancelButton?.onClick.AddListener(OnCancelClicked);
    }

    protected override void OnBeforeShow()
    {
        player1Group.Initialize();
        player2Group.Initialize();
    }

    private void LoadGameScene()
    {
        OnHidden -= LoadGameScene;
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameScenes.GameScene);
    }

    private void OnCancelClicked()
    {
        AudioManager.Instance?.PlayButtonSFX();
        Hide();
    }

    private void OnDestroy()
    {
        startButton?.onClick.RemoveListener(OnStartClicked);
        cancelButton?.onClick.RemoveListener(OnCancelClicked);
    }

    private void OnStartClicked()
    {
        AudioManager.Instance?.PlayButtonSFX();
        ThemeManager.Instance.SetPlayer1Theme(player1Group.SelectedTheme);
        ThemeManager.Instance.SetPlayer2Theme(player2Group.SelectedTheme);
        Hide();
        OnHidden += LoadGameScene;
    }

    #endregion

}