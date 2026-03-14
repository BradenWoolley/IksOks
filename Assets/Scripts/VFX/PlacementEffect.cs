using UnityEngine;


public class PlacementEffect : MonoBehaviour
{

    #region Fields

    [Header("Particle Systems")]
    [SerializeField]
    private ParticleSystem orbBurst;

    [SerializeField]
    private ParticleSystem starScatter;

    [Header("References")]
    [SerializeField]
    private Camera gameCamera;

    [SerializeField]
    private Canvas gameCanvas;

    #endregion


    #region Properties

    public static PlacementEffect Instance { get; private set; }

    #endregion


    #region Methods

    public void Play(RectTransform cellRect, Color playerColor)
    {
        Vector3 worldPos = UIToWorldPosition(cellRect);
        transform.position = worldPos;

        ParticleTools.SetColor(orbBurst, playerColor);
        ParticleTools.SetColor(starScatter, playerColor);

        orbBurst?.Play();
        starScatter?.Play();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private Vector3 UIToWorldPosition(RectTransform rectTransform)
    {
        // Get screen position of the UI element's center
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(gameCanvas.worldCamera, rectTransform.position);

        // Convert screen position to world position at z=0
        Vector3 worldPos = gameCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, gameCamera.nearClipPlane));
        worldPos.z = 0f;
        return worldPos;
    }

    #endregion

}