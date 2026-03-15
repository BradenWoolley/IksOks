using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameOverPopup : PopupBase
{

    #region Consts

    private const string MatchTimeKey = "MatchTime";

    #endregion


    #region Fields

    [Header("Display")]
    [SerializeField]
    private TextMeshProUGUI durationText;

    [SerializeField]
    private TextMeshProUGUI resultText;

    [Header("Buttons")]
    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private Button retryButton;

    private string pendingDuration;

    private string pendingResult;

    #endregion


    #region Methods

    /// <summary>
    /// Set display data then call Show().
    /// </summary>
    public void Prepare(string result, string duration)
    {
        pendingResult = result;
        pendingDuration = duration;
    }

    protected override void OnBeforeShow()
    {
        if (resultText)
        {
            resultText.text = pendingResult;
        }

        if (durationText)
        {
            durationText.text = $"{LanguageManager.Instance?.GetTranslationText(MatchTimeKey)}: {pendingDuration}";
        }
    }

    protected override void Awake()
    {
        base.Awake();
        retryButton?.onClick.AddListener(OnRetryClicked);
        exitButton?.onClick.AddListener(OnExitClicked);
    }

    private void OnRetryClicked()
    {
        AudioManager.Instance?.PlayButtonSFX();
        Hide();
    }

    private void OnExitClicked()
    {
        AudioManager.Instance?.PlayButtonSFX();
        Hide();
    }

    private void OnDestroy()
    {
        retryButton?.onClick.RemoveListener(OnRetryClicked);
        exitButton?.onClick.RemoveListener(OnExitClicked);
    }

    #endregion

}