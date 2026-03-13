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

    /// <summary>Set display data then call Show().</summary>
    public void Prepare(string result, string duration)
    {
        pendingResult = result;
        pendingDuration = duration;
    }

    /// <inheritdoc />
    protected override void OnBeforeShow()
    {
        if (resultText)
        {
            resultText.text = pendingResult;
        }

        if (durationText)
        {
            durationText.text = $"{MatchTimeKey}: {pendingDuration}";
        }
    }

    protected override void Awake()
    {
        base.Awake();

        retryButton?.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlayButtonSFX();
            Hide();
        });

        exitButton?.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlayButtonSFX();
            Hide();
        });
    }

    #endregion

}