using UnityEngine;


[CreateAssetMenu(fileName = "NewTheme", menuName = "ScriptableObjects/Theme")]
public class ThemeData : ScriptableObject
{

    #region Properties

    [Header("Marks")]
    public Sprite OSprite;

    public Sprite XSprite;

    [Header("Board")]
    public Sprite BoardSprite;

    public Color BoardColor = Color.white;

    [Header("Colors")]
    public Color BackgroundColor = new Color(0.1f, 0.1f, 0.15f);

    public Color Player1Color = new Color(0.2f, 0.6f, 1f);

    public Color Player2Color = new Color(1f, 0.4f, 0.4f);

    [Header("Identity")]
    public string ThemeName = "Default";

    public Sprite PreviewImage;

    #endregion

}