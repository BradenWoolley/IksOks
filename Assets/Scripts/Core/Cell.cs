using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class Cell : MonoBehaviour
{

    #region Fields

    [Header("References")]
    [SerializeField]
    private Image markImage;       // shows X or O sprite

    [SerializeField]
    private Image backgroundImage; // tinted on win highlight

    private Button button;

    private int column;

    private bool isOccupied;

    private int row;

    #endregion


    #region Properties

    #endregion


    #region Methods

    public void Initialize(int row, int col)
    {
       this.row = row;
        column = col;
        ResetCell();
    }

    public void ResetCell()
    {
        isOccupied = false;
        button.interactable = true;

        if (markImage != null)
        {
            markImage.sprite = null;
            markImage.enabled = false;
        }

        SetWinHighlight(false);
    }

    /// <summary>
    /// Called by GameBoard when this cell is part of the winning line.
    /// </summary>
    public void SetWinHighlight(bool active)
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = active
            ? new Color(1f, 0.85f, 0.2f, 1f)
            : Color.white;
        }
    }

    /// <summary>
    /// Applies the visual mark. Called after GameManager confirms placement.
    /// </summary>
    public void SetMark(Sprite markSprite)
    {
        if (markImage != null)
        {
            markImage.sprite = markSprite;
            markImage.enabled = true;
            PlayPlacementAnimation();
        }
    }

    private void OnCellClicked()
    {
        if (isOccupied)
        {
            return;
        }

        bool accepted = GameManager.Instance.TryPlaceMark(row, column);

        if (!accepted)
        {
            return;
        }

        isOccupied = true;
        button.interactable = false;

        CellMark mark = GameManager.Instance.GetMark(row, column);
        Sprite sprite = ThemeManager.Instance != null
            ? ThemeManager.Instance.GetMarkSprite(mark)
            : null;

        SetMark(sprite);

        AudioManager.Instance?.PlayPlacementSFX();
    }

    private void PlayPlacementAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(PunchScale());
    }

    private IEnumerator PunchScale()
    {
        transform.localScale = Vector3.zero;
        float elapsed = 0f;
        float duration = 0.2f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float scale = Mathf.Sin(t * Mathf.PI) * 1.2f + (t >= 0.5f ? 1f - Mathf.Sin(t * Mathf.PI) * 0.2f : 0f);
            transform.localScale = Vector3.one * Mathf.Max(0, scale);
            yield return null;
        }

        transform.localScale = Vector3.one;
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnCellClicked);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnCellClicked);
    }

    #endregion

}
