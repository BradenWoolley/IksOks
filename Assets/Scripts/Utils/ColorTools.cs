using UnityEngine;

public static class ColorTools
{

    #region Methods

    public static Color GetCurrentPlayerColor()
    {
        return GameManager.Instance.CurrentTurn == PlayerIndex.Player1
            ? ThemeManager.Instance.Player1Theme.PlayerColour
            : ThemeManager.Instance.Player2Theme.PlayerColour;
    }

    #endregion

}