using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    #region Fields

    [Header("Board Settings")]
    [SerializeField]
    private int boardSize = 3;

    // index = row * boardSize + col
    private CellMark[] board;

    private int movesPlayed;

    #endregion


    #region Events

    public event Action OnDraw;

    // PlayerIndex.None == draw.
    public event Action<PlayerIndex> OnPlayerWin;

    public event Action<PlayerIndex> OnTurnChanged;

    // Cell indices of the winning line.
    public event Action<int[], WinDirection> OnWinLineFound;

    #endregion


    #region Properties

    public static GameManager Instance { get; private set; }

    public int BoardSize => boardSize;

    public PlayerIndex CurrentTurn { get; private set; } = PlayerIndex.Player1;

    public GameState State { get; private set; } = GameState.Idle;

    #endregion


    #region Methods

    public CellMark GetMark(int row, int col)
    {
        return board[row * boardSize + col];
    }

    /// <summary>Begin or restart a match.</summary>
    public void StartGame()
    {
        board = new CellMark[boardSize * boardSize];
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

        int index = row * boardSize + col;
        if (board[index] != CellMark.Empty)
        {
            return false;
        }

        board[index] = CurrentTurn == PlayerIndex.Player1 ? CellMark.X : CellMark.O;
        movesPlayed++;

        if (CheckWin(out int[] winLine, out WinDirection direction))
        {
            State = GameState.GameOver;
            OnWinLineFound?.Invoke(winLine, direction);
            OnPlayerWin?.Invoke(CurrentTurn);
            return true;
        }

        if (movesPlayed == boardSize * boardSize)
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

    /// <summary>
    /// Win detection that works for any NxN board — no hardcoded cell indices.
    /// </summary>
    private bool CheckWin(out int[] winLine, out WinDirection direction)
    {
        CellMark mark = CurrentTurn == PlayerIndex.Player1
            ? CellMark.X
            : CellMark.O;

        for (int row = 0; row < boardSize; row++)
        {
            if (IsLineComplete(mark, GetRowIndices(row)))
            {
                winLine = GetRowIndices(row);
                direction = WinDirection.Horizontal;
                return true;
            }
        }

        for (int col = 0; col < boardSize; col++)
        {
            if (IsLineComplete(mark, GetColumnIndices(col)))
            {
                winLine = GetColumnIndices(col);
                direction = WinDirection.Vertical;
                return true;
            }
        }

        // Diagonal (top-left to bottom-right)
        if (IsLineComplete(mark, GetDiagonalIndices(false)))
        {
            winLine = GetDiagonalIndices(false);
            direction = WinDirection.DiagonalForward;
            return true;
        }

        // Diagonal (top-right to bottom-left)
        if (IsLineComplete(mark, GetDiagonalIndices(true)))
        {
            winLine = GetDiagonalIndices(true);
            direction = WinDirection.DiagonalBackward;
            return true;
        }

        winLine = null;
        direction = WinDirection.Horizontal;
        return false;
    }

    private int[] GetColumnIndices(int col)
    {
        int[] indices = new int[boardSize];

        for (int row = 0; row < boardSize; row++)
        {
            indices[row] = row * boardSize + col;
        }

        return indices;
    }

    private int[] GetDiagonalIndices(bool anti)
    {
        int[] indices = new int[boardSize];

        for (int i = 0; i < boardSize; i++)
        {
            indices[i] = anti
                ? i * boardSize + (boardSize - 1 - i)
                : i * boardSize + i;
        }

        return indices;
    }

    private int[] GetRowIndices(int row)
    {
        int[] indices = new int[boardSize];

        for (int col = 0; col < boardSize; col++)
        {
            indices[col] = row * boardSize + col;
        }

        return indices;
    }

    private bool IsLineComplete(CellMark mark, int[] indices)
    {
        foreach (int i in indices)
        {
            if (board[i] != mark)
            {
                return false;
            }
        }

        return true;
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