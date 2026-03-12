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

    #endregion


    #region Methods

    protected override void Awake()
    {
        base.Awake();

        // Wire each theme button by index
        for (int i = 0; i < themeButtons.Length; i++)
        {
            int index = i; // capture for closure
            themeButtons[i]?.onClick.AddListener(() => SelectTheme(index));
        }

        startButton?.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlayButtonSFX();
            ThemeManager.Instance.SetTheme(availableThemes[selectedThemeIndex]);
            Hide();
            OnHidden += LoadGameScene;
        });

        cancelButton?.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlayButtonSFX();
            Hide();
        });
    }

    protected override void OnBeforeShow()
    {
        SelectTheme(0);
    }

    private void LoadGameScene()
    {
        OnHidden -= LoadGameScene;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    private void SelectTheme(int index)
    {
        AudioManager.Instance?.PlayButtonSFX();
        selectedThemeIndex = index;

        // Highlight selected button — simple color tint for now
        for (int i = 0; i < themeButtons.Length; i++)
        {
            if (themeButtons[i] == null)
            {
                continue;
            }

            var colors = themeButtons[i].colors;
            colors.normalColor = i == index
                ? new Color(0.6f, 0.9f, 0.6f) // selected: green tint
                : Color.white;
            themeButtons[i].colors = colors;
        }
    }

    #endregion

}