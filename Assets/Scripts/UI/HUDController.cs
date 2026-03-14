using TMPro;
using UnityEngine;
using UnityEngine.UI;


// Todo: Sort out all hardcoded strings here and in all clases.
public class HUDController : MonoBehaviour
{

    #region Consts

    private const string Player1 = "Player1";

    private const string Player2 = "Player2";

    #endregion


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
        UpdateTimerDisplay(TimerTools.NullTime);
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
        settingsButton?.onClick.RemoveListener(OnSettingsClicked);

        if (MatchTimer.Instance != null)
        {
            MatchTimer.Instance.OnTimerUpdated -= UpdateTimerDisplay;
        }
    }
    private void OnDraw() => SetInteractable(false);

    private void OnSettingsClicked()
    {
        AudioManager.Instance?.PlayButtonSFX();
        settingsPopup?.Show();
    }

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

        settingsButton?.onClick.AddListener(OnSettingsClicked);

        ResetDisplay();
    }

    private void UpdateMoveDisplays()
    {
        if (player1MoveText)
        {
            player1MoveText.text = $"{LanguageManager.Instance?.GetTranslationText(Player1)}: {player1Moves}";
        }

        if (player2MoveText)
        {
            player2MoveText.text = $"{LanguageManager.Instance?.GetTranslationText(Player2)}: {player2Moves}";
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