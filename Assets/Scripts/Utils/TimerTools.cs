using UnityEngine;

public static class TimerTools
{

    #region Consts

    public const string NullTime = "00:00";

    #endregion


    #region Methods

    public static string FormatTime(float seconds)
    {
        int mins = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);

        return $"{mins:00}:{secs:00}";
    }

    #endregion

}