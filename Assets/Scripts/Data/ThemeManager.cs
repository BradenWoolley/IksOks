using UnityEngine;


public class ThemeManager : MonoBehaviour
{

    #region Fields

    [Header("Default Theme (used if none selected)")]
    [SerializeField]
    private ThemeData defaultTheme;

    private ThemeData activeTheme;


    #endregion


    #region Properties

    public static ThemeManager Instance { get; private set; }

    public ThemeData ActiveTheme => activeTheme;

    #endregion


    #region Methods

    public Sprite GetMarkSprite(CellMark mark)
    {
        // Todo: Would prefer one return
        if (activeTheme == null)
        {
            return null;
        }

        return mark == CellMark.X
            ? activeTheme.XSprite
            : activeTheme.OSprite;
    }

    public void SetTheme(ThemeData theme)
    {
        if (theme != null)
        {
            activeTheme = theme;
        }
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
        activeTheme = defaultTheme;
    }

    #endregion

}