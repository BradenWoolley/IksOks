using TMPro;
using UnityEngine;


public class DropDownSetter : MonoBehaviour
{

	#region Fields

	TMP_Dropdown dropDown;

    #endregion


    #region Methods

    private void Start()
    {
        dropDown = GetComponent<TMP_Dropdown>();

        int index = dropDown.options.FindIndex(option => option.text.Equals(LanguageManager.Instance?.GetCurrentLanguage(), System.StringComparison.InvariantCultureIgnoreCase));
        if (index >= 0)
        {
            dropDown.value = index;
        }
    }

    #endregion

}