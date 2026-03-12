using TMPro;
using UnityEngine;
using UnityEngine.UI;


// Todo: Sort out all hardcoded strings here and in all clases.
public class HUDController : MonoBehaviour
{

    #region Fields

    [Header("Buttons")]
    [SerializeField]
    private Button settingsButton;

    [Header("Move Counts")]
    [SerializeField]
    private TextMeshProUGUI player1MoveText;

    [SerializeField]
    private TextMeshProUGUI player2MoveText;

    [Header("Popups")]
    [SerializeField]
    private SettingsPopup settingsPopup;

    [Header("Timer")]
    [SerializeField]
    private TextMeshProUGUI timerText;

    private int player1Moves;

    private int player2Moves;

    #endregion


    #region Methods

    public void ResetDisplay()
    {
        player1Moves = 0;
        player2Moves = 0;
        UpdateMoveDisplays();
        UpdateTimerDisplay("00:00");
        SetInteractable(true);
    }

    private void HandleTurnChanged(PlayerIndex newTurn)
    {
        if (newTurn == PlayerIndex.Player2)
        {
            player1Moves++;
        }

        else
        {
            player2Moves++;
        }

        UpdateMoveDisplays();
    }

    private void OnGameOver(PlayerIndex _) => SetInteractable(false);

    private void OnDestroy()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        GameManager.Instance.OnTurnChanged -= HandleTurnChanged;
        GameManager.Instance.OnPlayerWin -= OnGameOver;
        GameManager.Instance.OnDraw -= OnDraw;

        if (MatchTimer.Instance != null)
        {
            MatchTimer.Instance.OnTimerUpdated -= UpdateTimerDisplay;
        }
    }
    private void OnDraw() => SetInteractable(false);

    private void SetInteractable(bool state)
    {
        if (settingsButton)
        {
            settingsButton.interactable = state;
        }
    }

    private void Start()
    {
        GameManager.Instance.OnTurnChanged += HandleTurnChanged;
        GameManager.Instance.OnPlayerWin += OnGameOver;
        GameManager.Instance.OnDraw += OnDraw;
        MatchTimer.Instance.OnTimerUpdated += UpdateTimerDisplay;

        settingsButton?.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlayButtonSFX();
            settingsPopup?.Show();
        });

        ResetDisplay();
    }

    private void UpdateMoveDisplays()
    {
        if (player1MoveText)
        {
            player1MoveText.text = $"P1: {player1Moves}";
        }

        if (player2MoveText)
        {
            player2MoveText.text = $"P2: {player2Moves}";
        }
    }

    private void UpdateTimerDisplay(string time)
    {
        if (timerText)
        {
            timerText.text = time;
        }
    }

    #endregion

}