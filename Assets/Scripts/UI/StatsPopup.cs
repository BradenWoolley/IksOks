using TMPro;
using UnityEngine;


public class StatsPopup : PopupBase
{

    #region Fields

    [Header("Buttons")]
    [SerializeField]
    private UnityEngine.UI.Button closeButton;

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
        closeButton?.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlayButtonSFX();
            Hide();
        });
    }

    // Todo: Consts.
    protected override void OnBeforeShow()
    {
        StatsData data = StatsManager.Instance.GetStats();

        if (totalGamesText)
        {
            totalGamesText.text = $"Games Played: {data.TotalGames}";
        }

        if (player1WinsText)
        {
            player1WinsText.text = $"Player 1 Wins: {data.Player1Wins}";
        }

        if (player2WinsText)
        {
            player2WinsText.text = $"Player 2 Wins: {data.Player2Wins}";
        }

        if (drawsText)
        {
            drawsText.text = $"Draws: {data.Draws}";
        }

        string avgTime = data.TotalGames > 0
            ? MatchTimer.FormatTime(data.TotalDuration / data.TotalGames)
            : "00:00";

        if (avgDurationText)
        {
            avgDurationText.text = $"Avg Duration: {avgTime}";
        }
    }

    #endregion

}