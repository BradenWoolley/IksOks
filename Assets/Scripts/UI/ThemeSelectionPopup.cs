using UnityEngine;
using UnityEngine.UI;


public class ThemeSelectionPopup : PopupBase
{

    #region Fields

    [Header("Navigation")]
    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private Button startButton;

    [Header("Themes")]
    [SerializeField]
    private ThemeData[] availableThemes;

    [Header("Theme Buttons")]
    [SerializeField]
    private Button[] themeButtons;

    private int selectedThemeIndex = 0;

    private Image[] themePreviews;

    #endregion


    #region Methods

    protected override void Awake()
    {
        base.Awake();

        themePreviews = new Image[themeButtons.Length];

        for (int i = 0; i < themeButtons.Length; i++)
        {
            int index = i;
            themeButtons[i]?.onClick.AddListener(() => SelectTheme(index));
            themePreviews[i] = themeButtons[i].transform.GetChild(0).GetComponent<Image>();
        }

        startButton?.onClick.AddListener(OnStartClicked);
        cancelButton?.onClick.AddListener(OnCancelClicked);
    }

    protected override void OnBeforeShow()
    {
        for (int i = 0; i < themePreviews.Length; i++)
        {
            if (themePreviews[i] == null)
            {
                continue;
            }

            themePreviews[i].sprite = availableThemes[i].PreviewImage;
        }

        SelectTheme(0);
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

        for (int i = 0; i < themeButtons.Length; i++)
        {
            themeButtons[i]?.onClick.RemoveAllListeners();
        }
    }

    private void OnStartClicked()
    {
        AudioManager.Instance?.PlayButtonSFX();
        ThemeManager.Instance.SetTheme(availableThemes[selectedThemeIndex]);
        Hide();
        OnHidden += LoadGameScene;
    }

    private void LoadGameScene()
    {
        OnHidden -= LoadGameScene;
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameScenes.GameScene);
    }

    private void SelectTheme(int index)
    {
        AudioManager.Instance?.PlayButtonSFX();
        selectedThemeIndex = index;
    }

    #endregion

}