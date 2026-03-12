using UnityEditor;
using UnityEngine;


public class MenuItems
{

    #region Methods

    [MenuItem("Tools/ClearPlayerPrefs")]
    private static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    #endregion

}