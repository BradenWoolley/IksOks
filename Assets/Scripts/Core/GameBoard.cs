using UnityEngine;

/// <summary>
/// Spawns Cell prefabs procedurally based on GameManager.BoardSize.
/// </summary>
public class GameBoard : MonoBehaviour
{

    #region Consts

    #endregion


    #region Fields

    [Header("References")]
    [SerializeField]
    private Cell cellPrefab;

    [SerializeField]
    private RectTransform boardContainer;

    [Header("Layout")]
    [SerializeField]
    private float cellSpacing = 10f;

    private int boardSize;

    private Cell[,] cells;

    #endregion


    #region Methods

    public Cell GetCell(int row, int col)
    {
        return cells[row, col];
    }

    /// <summary>Resets for rematch.</summary>
    public void ResetBoard()
    {
        foreach (Cell cell in cells)
        {
            cell.ResetCell();
        }
    }

    private void GenerateBoard()
    {
        cells = new Cell[boardSize, boardSize];

        // Clears any forgotten objects by designer.
        foreach (Transform child in boardContainer)
        {
            Destroy(child.gameObject);
        }

        float containerSize = boardContainer.rect.width;
        float cellSize = (containerSize - cellSpacing * (boardSize - 1)) / boardSize;

        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                Cell cell = Instantiate(cellPrefab, boardContainer);
                // Todo: Const format
                cell.name = $"Cell_{row}_{col}";

                // Position within the container
                RectTransform rt = cell.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(cellSize, cellSize);

                float x = col * (cellSize + cellSpacing) - (containerSize - cellSize) / 2f;
                float y = -row * (cellSize + cellSpacing) + (containerSize - cellSize) / 2f;
                rt.anchoredPosition = new Vector2(x, y);

                cell.Initialize(row, col);
                cells[row, col] = cell;
            }
        }
    }

    private void HandleWinLine(int[] winIndices, WinDirection direction)
    {
        // Highlight winning cells — each Cell knows how to highlight itself
        foreach (int index in winIndices)
        {
            int row = index / boardSize;
            int col = index % boardSize;
            cells[row, col].SetWinHighlight(true);
        }

        // Todo: Draw strike line. Investigate Line renderer usage.
    }

    private void HandleDraw()
    {
        // Todo: Some animation?
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnWinLineFound -= HandleWinLine;
        GameManager.Instance.OnDraw -= HandleDraw;
    }

    private void Start()
    {
        GameManager.Instance.OnWinLineFound += HandleWinLine;
        GameManager.Instance.OnDraw += HandleDraw;

        boardSize = GameManager.Instance.BoardSize;

        GenerateBoard();
    }

    #endregion

}
