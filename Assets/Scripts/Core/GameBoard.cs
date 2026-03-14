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

    [Header("Board Theme")]
    [SerializeField]
    private BoardTheme boardTheme;

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

    public void ResetBoard()
    {
        strikeLine?.Hide();

        foreach (Cell cell in cells)
        {
            cell.ResetCell();
        }
    }

    // Clears any forgotten objects by designer.
    private void ClearBoard()
    {
        foreach (Transform child in boardContainer)
        {
            if (child.TryGetComponent<Cell>(out _))
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void DrawStrikeLine(int[] winIndices)
    {
        if (strikeLine != null)
        {
            int firstIndex = winIndices[0];
            int lastIndex = winIndices[^1];

            RectTransform fromCell = cells[firstIndex / boardSize, firstIndex % boardSize].GetComponent<RectTransform>();
            RectTransform toCell = cells[lastIndex / boardSize, lastIndex % boardSize].GetComponent<RectTransform>();

            Color winnerColor = ColorTools.GetCurrentPlayerColor();

            strikeLine.Animate(fromCell, toCell, winnerColor);
        }
    }

    private void GenerateBoard()
    {
        if (boardTheme == null)
        {
            return;
        }

        if (boardBackground != null)
        {
            boardBackground.sprite = boardTheme.BoardSprite;
            boardBackground.color = boardTheme.BoardColor;
        }
    }

    private void GenerateCells()
    {
        cells = new Cell[boardSize, boardSize];

        ClearBoard();

        float containerSize = boardContainer.rect.width;
        float cellSize = (containerSize - cellSpacing * (boardSize - 1)) / boardSize;

        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                InstantiateCells(containerSize, cellSize, row, col);
            }
        }
    }

    private void HandleWinLine(int[] winIndices, WinDirection direction)
    {
        DrawStrikeLine(winIndices);

        AudioManager.Instance?.PlayWinSFX();
    }

    private void InstantiateCells(float containerSize, float cellSize, int row, int col)
    {
        Cell cell = Instantiate(cellPrefab, boardContainer);

        Image cellImage = cell.GetComponent<Image>();
        if (cellImage != null && boardTheme != null)
        {
            cell.SetTheme(boardTheme.CellSprite, boardTheme.CellColor);
        }

        cell.name = $"Cell_{row}_{col}";

        // Position within the container
        RectTransform rect = cell.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(cellSize, cellSize);

        float x = col * (cellSize + cellSpacing) - (containerSize - cellSize) / 2f;
        float y = -row * (cellSize + cellSpacing) + (containerSize - cellSize) / 2f;
        rect.anchoredPosition = new Vector2(x, y);

        cell.Initialize(row, col);
        cells[row, col] = cell;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        GameManager.Instance.OnWinLineFound -= HandleWinLine;
    }

    private void Start()
    {
        GameManager.Instance.OnWinLineFound += HandleWinLine;

        boardSize = GameManager.Instance.BoardSize;

        GenerateBoard();
        GenerateCells();
    }

    #endregion

}