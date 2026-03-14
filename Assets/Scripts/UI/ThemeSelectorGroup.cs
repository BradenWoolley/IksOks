using UnityEngine;
using UnityEngine.UI;


public class ThemeSelectorGroup : MonoBehaviour
{

    #region Consts

    private const int PreviewDepth = 1;

    #endregion


    #region Fields

    [SerializeField]
    private ThemeData[] availableThemes;

    [SerializeField]
    private GameObject[] selectionIndicators;

    [SerializeField]
    private Button[] themeButtons;

    private int selectedIndex = 0;

    #endregion


    #region Properties

    public ThemeData SelectedTheme => availableThemes[selectedIndex];

    #endregion


    #region Methods

    public void Initialize()
    {
        Image[] previews = new Image[themeButtons.Length];
        for (int i = 0; i < themeButtons.Length; i++)
        {
            previews[i] = themeButtons[i].transform.GetChild(PreviewDepth).GetComponent<Image>();
            if (previews[i] != null && i < availableThemes.Length)
            {
                previews[i].sprite = availableThemes[i].PlayerSprite;
                previews[i].preserveAspect = true;
            }

            int index = i;
            themeButtons[i]?.onClick.AddListener(() => SelectTheme(index));
        }

        SelectTheme(0);
    }

    private void SelectTheme(int index)
    {
        AudioManager.Instance?.PlayButtonSFX();
        selectedIndex = index;

        for (int i = 0; i < selectionIndicators.Length; i++)
        {
            if (selectionIndicators[i] != null)
            {
                selectionIndicators[i].SetActive(i == index);
            }
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < themeButtons.Length; i++)
        {
            themeButtons[i]?.onClick.RemoveAllListeners();
        }
    }

    #endregion

}