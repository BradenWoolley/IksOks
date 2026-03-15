using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class Cell : MonoBehaviour
{

    #region Fields

    [Header("References")]
    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private Image markImage;

    private Button button;

    private int column;

    private bool isOccupied;

    private int row;

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

        if (backgroundImage != null)
        {
            backgroundImage.enabled = true;
        }
    }

    public void SetTheme(Sprite sprite, Color color)
    {
        if (backgroundImage != null)
        {
            backgroundImage.sprite = sprite;
            backgroundImage.color = color;
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

        if (backgroundImage != null)
        {
            backgroundImage.enabled = false;
        }
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnCellClicked);
    }

    private void OnCellClicked()
    {
        if (isOccupied)
        {
            return;
        }

        Color playerColor = ColorTools.GetCurrentPlayerColor();

        bool accepted = GameManager.Instance.TryPlaceMark(row, column);

        if (accepted)
        {
            isOccupied = true;
            button.interactable = false;

            CellMark mark = GameManager.Instance.GetMark(row, column);
            Sprite sprite = ThemeManager.Instance != null
                ? ThemeManager.Instance.GetMarkSprite(mark)
                : null;

            SetMark(sprite);

            AudioManager.Instance?.PlayPlacementSFX();
            PlacementEffect.Instance?.Play(GetComponent<RectTransform>(), playerColor);
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnCellClicked);
    }

    private void PlayPlacementAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(PunchScale());
    }

    // Ideally would make use of Lean.Animations on place for more designer options,
    // however this makes creating new demo scenes quicker.
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

    #endregion

}