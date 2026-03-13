using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class StrikeLine : MonoBehaviour
{

    #region Fields

    [Header("Settings")]
    [SerializeField]
    private float animationDuration = 0.3f;

    [SerializeField]
    private Color lineColor = new Color(1f, 1f, 1f, 0.85f);

    [SerializeField]
    private float lineThickness = 12f;

    [Header("Particle Effects")]
    [SerializeField]
    private StrikeEffect strikeEffect;

    private Image image;

    private RectTransform rectTransform;

    #endregion


    #region Methods

    /// <summary>
    /// Call this with the first and last cell RectTransforms of the winning line.
    /// GameBoard resolves which cells to pass in.
    /// </summary>
    public void Animate(RectTransform fromCell, RectTransform toCell, Color effectColor)
    {
        transform.SetAsLastSibling();
        gameObject.SetActive(true);
        strikeEffect?.SetColor(effectColor);
        StartCoroutine(DrawLine(fromCell, toCell));
    }

    public void Animate(RectTransform fromCell, RectTransform toCell)
    {
        Animate(fromCell, toCell, Color.white);
    }

    public void Hide()
    {
        StopAllCoroutines();
        strikeEffect?.Reset();
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        image.color = lineColor;
        gameObject.SetActive(false);
    }

    private IEnumerator DrawLine(RectTransform fromCell, RectTransform toCell)
    {
        // Get cell centers in local space of the BoardContainer
        Vector2 fromPos = fromCell.anchoredPosition;
        Vector2 toPos = toCell.anchoredPosition;

        // Position the line at the midpoint between the two cells
        Vector2 midpoint = (fromPos + toPos) / 2f;
        float fullLength = Vector2.Distance(fromPos, toPos);

        // Angle between the two cells
        Vector2 direction = (toPos - fromPos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set position and rotation — width animates from 0 to fullLength
        rectTransform.anchoredPosition = midpoint;
        rectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
        rectTransform.sizeDelta = new Vector2(0f, lineThickness);

        strikeEffect?.StartTrail();

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);
            float eased = EaseOutCubic(t);
            float width = Mathf.Lerp(0f, fullLength, eased);

            rectTransform.sizeDelta = new Vector2(width, lineThickness);

            // Move the effect along the line as it draws
            if (strikeEffect != null)
            {
                Vector2 currentTip = fromPos + direction * width;
                strikeEffect.transform.localPosition = new Vector3(currentTip.x, currentTip.y, 0f);
            }
            yield return null;
        }

        rectTransform.sizeDelta = new Vector2(fullLength, lineThickness);

        // Trigger burst at the endpoint in world space
        if (strikeEffect != null)
        {
            //Vector3 endWorld = toCell.TransformPoint(Vector3.zero);
            Vector3 endWorld = toCell.position;
            strikeEffect.EndTrail(endWorld);
        }
    }

    private float EaseOutCubic(float t)
    {
        return 1f - Mathf.Pow(1f - t, 3f);
    }

    #endregion

}