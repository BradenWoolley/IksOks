public interface IGameRules
{

    #region Properties

    /// <summary>
    /// Board size for this rule set (e.g. 3 for a 3x3 board).
    /// </summary>
    int BoardSize { get; }

    #endregion


    #region Methods

    /// <summary>
    /// Check whether the game is a draw.
    /// </summary>
    bool CheckDraw(CellMark[] board, int movesPlayed);

    /// <summary>
    /// Check if the current player has won.
    /// Returns true if a winning line is found, and outputs the winning cell
    /// indices and the direction of the line.
    /// </summary>
    bool CheckWin(CellMark[] board, PlayerIndex currentPlayer, out int[] winLine, out WinDirection direction);

    #endregion

}