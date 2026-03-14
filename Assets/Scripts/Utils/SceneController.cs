using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{

    #region Consts

    private const string MatchKey = "MatchDraw";

    private const string Player1Key = "MatchPlayer1";

    private const string Player2Key = "MatchPlayer2";

    private const float PopupDelay = 1f;

    #endregion


    #region Fields

    [Header("Scene References")]
    [SerializeField]
    private GameBoard gameBoard;

    [SerializeField]
    private GameOverPopup gameOverPopup;

    [SerializeField]
    private HUDController hud;

    private enum PendingAction { None, Retry, Exit }

    private PendingAction pendingAction = PendingAction.None;

    #endregion


    #region Methods

    public void OnExitPressed()
    {
        AudioManager.Instance?.PlayButtonSFX();
        pendingAction = PendingAction.Exit;
        gameOverPopup.Hide();
    }

    public void OnRetryPressed()
    {
        AudioManager.Instance?.PlayButtonSFX();
        pendingAction = PendingAction.Retry;
        gameOverPopup.Hide();
    }

    /*public void RetryMatch()
    {
        gameOverPopup?.Hide();
        StartMatch();
    }*/

    public void StartMatch()
    {
        gameBoard.ResetBoard();
        MatchTimer.Instance.StartTimer();
        GameManager.Instance.StartGame();
        hud?.ResetDisplay();
    }

    private void EndMatch(string result)
    {
        MatchTimer.Instance.StopTimer();
        AudioManager.Instance?.PlayWinSFX();
        StatsManager.Instance?.RecordMatchResult(result, MatchTimer.Instance.Duration);

        gameOverPopup.Prepare(result, TimerTools.FormatTime(MatchTimer.Instance.Duration));
        StartCoroutine(ShowGameOverDelayed());
    }

    private void HandleDraw()
    {
        EndMatch(LanguageManager.Instance?.GetTranslationText(MatchKey));
    }

    private void HandlePlayerWin(PlayerIndex winner)
    {
        string result = winner == PlayerIndex.Player1
            ? LanguageManager.Instance?.GetTranslationText(Player1Key)
            : LanguageManager.Instance?.GetTranslationText(Player2Key);

        EndMatch(result);
    }

    private void HandlePopupHidden()
    {
        switch (pendingAction)
        {
            case PendingAction.Retry:
                StartMatch();
                break;
            case PendingAction.Exit:
                SceneManager.LoadScene(GameScenes.PlayScene);
                break;
        }
        pendingAction = PendingAction.None;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerWin -= HandlePlayerWin;
            GameManager.Instance.OnDraw -= HandleDraw;
        }

        if (gameOverPopup != null)
        {
            gameOverPopup.OnHidden -= HandlePopupHidden;
        }
    }

    private IEnumerator ShowGameOverDelayed()
    {
        yield return new WaitForSeconds(PopupDelay);
        gameOverPopup.Show();
    }

    private void Start()
    {
        GameManager.Instance.OnPlayerWin += HandlePlayerWin;
        GameManager.Instance.OnDraw += HandleDraw;

        gameOverPopup.OnHidden += HandlePopupHidden;

        StartMatch();
    }

    #endregion

}