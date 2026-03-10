using UnityEngine;


// Todo: All to consts
public class SceneController : MonoBehaviour
{

    #region Fields

    [Header("Scene References")]
    [SerializeField]
    private GameBoard gameBoard;

    // Todo: Implement.
    [SerializeField]
    private GameOverPopup gameOverPopup;

    #endregion


    #region Methods

    public void StartMatch()
    {
        gameBoard.ResetBoard();
        MatchTimer.Instance.StartTimer();
        GameManager.Instance.StartGame();
    }

    public void RetryMatch()
    {
        gameOverPopup?.Hide();
        StartMatch();
    }

    public void ExitToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlayScene");
    }

    private void HandlePlayerWin(PlayerIndex winner)
    {
        string resultText = winner == PlayerIndex.Player1 ? "Player 1 Wins!" : "Player 2 Wins!";
        ShowGameOver(resultText);
    }

    private void HandleDraw()
    {
        ShowGameOver("It's a Draw!");
    }

    private void ShowGameOver(string resultText)
    {
        MatchTimer.Instance.StopTimer();

        StatsManager.Instance?.RecordMatchResult(resultText, MatchTimer.Instance.Duration);

        gameOverPopup?.Show(resultText, MatchTimer.FormatTime(MatchTimer.Instance.Duration));
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPlayerWin -= HandlePlayerWin;
        GameManager.Instance.OnDraw -= HandleDraw;
    }

    private void Start()
    {
        GameManager.Instance.OnPlayerWin += HandlePlayerWin;
        GameManager.Instance.OnDraw += HandleDraw;

        StartMatch();
    }

    #endregion

}