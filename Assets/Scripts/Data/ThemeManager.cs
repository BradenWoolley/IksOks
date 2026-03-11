using UnityEngine;


public class ThemeManager : MonoBehaviour
{

    #region Fields

    [Header("Placeholder Sprites (will swap with ThemeData ScriptableObject)")]
    [SerializeField]
    private Sprite oSprite;

    [SerializeField]
    private Sprite xSprite;


    #endregion


    #region Properties

    public static ThemeManager Instance { get; private set; }

    #endregion


    #region Methods

    public Sprite GetMarkSprite(CellMark mark)
    {
        return mark == CellMark.X
            ? xSprite
            : oSprite;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else
        {
            Instance = this;
        }
    }

    #endregion

}