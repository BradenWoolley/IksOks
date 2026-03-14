using UnityEngine;


[CreateAssetMenu(fileName = "NewTheme", menuName = "ScriptableObjects/Theme/PlayerTheme")]
public class ThemeData : ScriptableObject
{

    #region Properties

    [Header("Marks")]
    public Sprite PlayerSprite;

    [Header("Colors")]

    public Color PlayerColour = new Color(0.2f, 0.6f, 1f);

    [Header("Identity")]
    public string ThemeName = "Default";

    #endregion

}