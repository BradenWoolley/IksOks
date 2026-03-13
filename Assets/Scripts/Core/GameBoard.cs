using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Spawns Cell prefabs procedurally based on GameManager.BoardSize.
/// </summary>
public class GameBoard : MonoBehaviour
{

    #region Fields

    [Header("References")]
    [SerializeField]
    private RectTransform boardContainer;

    [SerializeField]
    private Image boardBackground;

    [SerializeField]
    private Cell cellPrefab;

    [Header("Layout")]
    [SerializeField]
    private float cellSpacing = 10f;

    [Header("Strike Line")]
    [SerializeField]
    private StrikeLine strikeLine;

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
        strikeLine?.Hide();

        foreach (Cell cell in cells)
        {
            cell.ResetCell();
        }
    }

    private void DrawStrikeLine(int[] winIndices)
    {
        if (strikeLine != null)
        {
            int firstIndex = winIndices[0];
            // Todo: Simplify.
            int lastIndex = winIndices[winIndices.Length - 1];

            RectTransform fromCell = cells[firstIndex / boardSize, firstIndex % boardSize].GetComponent<RectTransform>();
            RectTransform toCell = cells[lastIndex / boardSize, lastIndex % boardSize].GetComponent<RectTransform>();

            Color winnerColor = GameManager.Instance.CurrentTurn == PlayerIndex.Player1
            ? ThemeManager.Instance.ActiveTheme.Player1Color
            : ThemeManager.Instance.ActiveTheme.Player2Color;

            strikeLine.Animate(fromCell, toCell, winnerColor);
        }
    }

    private void GenerateBoard()
    {
        if (boardBackground != null && ThemeManager.Instance != null)
        {
            boardBackground.sprite = ThemeManager.Instance.ActiveTheme.BoardSprite;
            boardBackground.color = ThemeManager.Instance.ActiveTheme.BoardColor;
        }
    }

    private void GenerateCells()
    {
        cells = new Cell[boardSize, boardSize];

        // Clears any forgotten objects by designer.
        foreach (Transform child in boardContainer)
        {
            if (child.TryGetComponent<Cell>(out _))
            {
                Destroy(child.gameObject);
            }
        }

        float containerSize = boardContainer.rect.width;
        float cellSize = (containerSize - cellSpacing * (boardSize - 1)) / boardSize;

        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                Cell cell = Instantiate(cellPrefab, boardContainer);

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

    private void HandleDraw()
    {
        // Todo: Some animation?
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

        DrawStrikeLine(winIndices);

        AudioManager.Instance?.PlayWinSFX();
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
        GenerateCells();
    }

    #endregion

}