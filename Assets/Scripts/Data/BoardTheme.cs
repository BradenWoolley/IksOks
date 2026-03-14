using UnityEngine;


[CreateAssetMenu(fileName = "BoardTheme", menuName = "ScriptableObjects/Theme/BoardTheme")]
public class BoardTheme : ScriptableObject
{

    #region Properties

    [Header("Board")]
    public Sprite BoardSprite;

    public Color BoardColor = Color.white;

    [Header("Cell")]
    public Sprite CellSprite;

    public Color CellColor = new Color(0.1f, 0.1f, 0.15f);


    #endregion

}