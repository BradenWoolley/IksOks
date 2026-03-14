using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StatsPopup : PopupBase
{

    #region Fields

    [Header("Buttons")]
    [SerializeField]
    private Button closeButton;

    [Header("Display Fields")]
    [SerializeField]
    private TextMeshProUGUI avgDurationText;

    [SerializeField]
    private TextMeshProUGUI drawsText;

    [SerializeField]
    private TextMeshProUGUI player1WinsText;

    [SerializeField]
    private TextMeshProUGUI player2WinsText;

    [SerializeField]
    private TextMeshProUGUI totalGamesText;

    #endregion


    #region Methods

    protected override void Awake()
    {
        base.Awake();
        closeButton?.onClick.AddListener(OnCloseClicked);
    }

    protected override void OnBeforeShow()
    {
        StatsData data = StatsManager.Instance.GetStats();

        if (totalGamesText)
        {
            totalGamesText.text = $"{data.TotalGames}";
        }

        if (player1WinsText)
        {
            player1WinsText.text = $"{data.Player1Wins}";
        }

        if (player2WinsText)
        {
            player2WinsText.text = $"{data.Player2Wins}";
        }

        if (drawsText)
        {
            drawsText.text = $"{data.Draws}";
        }

        string avgTime = data.TotalGames > 0
            ? TimerTools.FormatTime(data.TotalDuration / data.TotalGames)
            : TimerTools.NullTime;

        if (avgDurationText)
        {
            avgDurationText.text = $"{avgTime}";
        }
    }

    private void OnCloseClicked()
    {
        AudioManager.Instance?.PlayButtonSFX();
        Hide();
    }

    private void OnDestroy()
    {
        closeButton?.onClick.RemoveListener(OnCloseClicked);
    }

    #endregion

}