using UnityEngine;


[CreateAssetMenu(fileName = "StandardGameRules", menuName = "ScriptableObjects/GameRules")]
public class StandardGameRules : ScriptableObject, IGameRules
{

    #region Fields

    [SerializeField]
    private int boardSize = 3;

    #endregion


    #region Properties

    public int BoardSize => boardSize;

    #endregion


    #region Methods

    public bool CheckDraw(CellMark[] board, int movesPlayed)
    {
        return movesPlayed == boardSize * boardSize;
    }

    public bool CheckWin(CellMark[] board, PlayerIndex currentPlayer, out int[] winLine, out WinDirection direction)
    {
        CellMark mark = currentPlayer == PlayerIndex.Player1
            ? CellMark.X
            : CellMark.O;

        for (int row = 0; row < boardSize; row++)
        {
            int[] indices = GetRowIndices(row);
            if (IsLineComplete(board, mark, indices))
            {
                winLine = indices;
                direction = WinDirection.Horizontal;
                return true;
            }
        }

        for (int col = 0; col < boardSize; col++)
        {
            int[] indices = GetColumnIndices(col);
            if (IsLineComplete(board, mark, indices))
            {
                winLine = indices;
                direction = WinDirection.Vertical;
                return true;
            }

        }

        // Diagonal top-left to bottom-right
        {
            int[] indices = GetDiagonalIndices(false);
            if (IsLineComplete(board, mark, indices))
            {
                winLine = indices;
                direction = WinDirection.DiagonalForward;
                return true;
            }
        }

        // Diagonal top-right to bottom-left
        {
            int[] indices = GetDiagonalIndices(true);
            if (IsLineComplete(board, mark, indices))
            {
                winLine = indices;
                direction = WinDirection.DiagonalBackward;
                return true;
            }
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

    private bool IsLineComplete(CellMark[] board, CellMark mark, int[] indices)
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

    #endregion

}