using UnityEngine;


public class ThemeManager : MonoBehaviour
{

    #region Fields

    [Header("Default Theme (used if none selected)")]
    [SerializeField]
    private ThemeData defaultPlayer1Theme;

    [SerializeField]
    private ThemeData defaultPlayer2Theme;

    private ThemeData player1Theme;

    private ThemeData player2Theme;


    #endregion


    #region Properties

    public static ThemeManager Instance { get; private set; }

    public ThemeData Player1Theme => player1Theme;
    public ThemeData Player2Theme => player2Theme;

    #endregion


    #region Methods

    public Sprite GetMarkSprite(CellMark mark)
    {
        return mark == CellMark.X
            ? player1Theme?.PlayerSprite
            : player2Theme?.PlayerSprite;
    }

    public void SetPlayer1Theme(ThemeData theme)
    {
        if (theme != null)
        {
            player1Theme = theme;
        }
    }

    public void SetPlayer2Theme(ThemeData theme)
    {
        if (theme != null)
        {
            player2Theme = theme;
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

        player1Theme = defaultPlayer1Theme;
        player2Theme = defaultPlayer2Theme;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    #endregion

}