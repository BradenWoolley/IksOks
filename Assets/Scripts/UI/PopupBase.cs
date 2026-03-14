using System;
using System.Collections;
using UnityEngine;


/// <summary>
/// Base class for all popups. Handles animated show/hide via CanvasGroup
/// and scale punch. Subclasses override OnBeforeShow() and OnBeforeHide()
/// to inject their own data before animation plays.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public abstract class PopupBase : MonoBehaviour
{

    #region Fields

    [Header("Animation Settings")]
    [SerializeField]
    private float animationDuration = 0.2f;

    private Coroutine animationCoroutine;

    private CanvasGroup canvasGroup;

    #endregion


    #region Events

    public event Action OnHidden;

    public event Action OnShown;

    #endregion


    #region Methods

    /// <summary>
    /// Hides the pop-up.
    /// </summary>
    public void Hide()
    {
        OnBeforeHide();

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(AnimateHide());
    }

    /// <summary>
    /// Displays the pop-up.
    /// </summary>
    public void Show()
    {
        OnBeforeShow();
        gameObject.SetActive(true);
        AudioManager.Instance?.PlayPopupSFX();

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(AnimateShow());
    }

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called before Hide animation.
    /// </summary>
    protected virtual void OnBeforeHide()
    {
    }

    /// <summary>
    /// Called before Show animation. Set text/data here.
    /// </summary>
    protected virtual void OnBeforeShow()
    {
    }

    private IEnumerator AnimateHide()
    {
        canvasGroup.interactable = false;

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);
            transform.localScale = Vector3.one * (1f - t);
            canvasGroup.alpha = 1f - t;
            yield return null;
        }

        transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
        OnHidden?.Invoke();
    }

    private IEnumerator AnimateShow()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = true;

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);
            float eased = EaseOutBack(t);
            transform.localScale = Vector3.one * eased;
            canvasGroup.alpha = t;
            yield return null;
        }

        transform.localScale = Vector3.one;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        OnShown?.Invoke();
    }

    // Since Lean package is in project could replace this with a Lean transition for
    // the designer/technical artist to have greater control over.
    private float EaseOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    #endregion

}