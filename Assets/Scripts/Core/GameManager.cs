using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    #region Fields

    [Header("Rules")]
    [SerializeField]
    private StandardGameRules rules;

    // index = row * boardSize + col
    private CellMark[] board;

    private int movesPlayed;

    #endregion


    #region Events

    public event Action OnDraw;

    public event Action<PlayerIndex> OnPlayerWin;

    public event Action<PlayerIndex> OnTurnChanged;

    public event Action<int[], WinDirection> OnWinLineFound;

    #endregion


    #region Properties

    public static GameManager Instance { get; private set; }

    public int BoardSize => rules.BoardSize;

    public PlayerIndex CurrentTurn { get; private set; } = PlayerIndex.Player1;

    public GameState State { get; private set; } = GameState.Idle;

    #endregion


    #region Methods

    public CellMark GetMark(int row, int col)
    {
        return board[row * BoardSize + col];
    }

    /// <summary>Begin or restart a match.</summary>
    public void StartGame()
    {
        board = new CellMark[BoardSize * BoardSize];
        movesPlayed = 0;
        CurrentTurn = PlayerIndex.Player1;
        State = GameState.Playing;

        OnTurnChanged?.Invoke(CurrentTurn);
    }

    /// <summary>
    /// Attempt to place at desired cell.
    /// Returns false if the cell is already occupied or the game isn't active.
    /// </summary>
    public bool TryPlaceMark(int row, int col)
    {
        if (State != GameState.Playing)
        {
            return false;
        }

        int index = row * BoardSize + col;
        if (board[index] != CellMark.Empty)
        {
            return false;
        }

        board[index] = CurrentTurn == PlayerIndex.Player1 ? CellMark.X : CellMark.O;
        movesPlayed++;

        if (rules.CheckWin(board, CurrentTurn, out int[] winLine, out WinDirection direction))
        {
            State = GameState.GameOver;
            OnWinLineFound?.Invoke(winLine, direction);
            OnPlayerWin?.Invoke(CurrentTurn);
            return true;
        }

        if (rules.CheckDraw(board, movesPlayed))
        {
            State = GameState.GameOver;
            OnDraw?.Invoke();
            return true;
        }

        SwitchTurn();
        return true;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void SwitchTurn()
    {
        CurrentTurn = CurrentTurn == PlayerIndex.Player1
            ? PlayerIndex.Player2
            : PlayerIndex.Player1;

        OnTurnChanged?.Invoke(CurrentTurn);
    }

    #endregion

}