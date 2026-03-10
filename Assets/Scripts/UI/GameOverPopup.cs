using TMPro;
using UnityEngine;


public class GameOverPopup : MonoBehaviour
{

    #region Fields

    [SerializeField]
    private TextMeshProUGUI durationText;

    [SerializeField]
    private TextMeshProUGUI resultText;

    #endregion


    #region Methods

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(string result, string duration)
    {
        gameObject.SetActive(true);

        if (resultText)
        {
            resultText.text = result;
        }

        if (durationText)
        {
            // Todo: Const format.
            durationText.text = $"Time: {duration}";
        }
    }

    #endregion

}