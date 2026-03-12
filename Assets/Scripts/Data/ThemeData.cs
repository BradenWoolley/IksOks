using UnityEngine;


[CreateAssetMenu(fileName = "NewTheme", menuName = "ScriptableObjects/Theme")]
public class ThemeData : ScriptableObject
{

    #region Properties

    [Header("Marks")]
    public Sprite oSprite;
    public Sprite xSprite;

    [Header("Board")]
    public Sprite boardSprite;
    public Color boardColor = Color.white;

    [Header("Colors")]
    public Color backgroundColor = new Color(0.1f, 0.1f, 0.15f);
    public Color player1Color = new Color(0.2f, 0.6f, 1f);
    public Color player2Color = new Color(1f, 0.4f, 0.4f);

    [Header("Identity")]
    public string themeName = "Default";
    public Sprite previewImage;

    #endregion

}