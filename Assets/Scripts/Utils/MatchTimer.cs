using System;
using UnityEngine;


/// <summary>
/// Tracks elapsed time for the current match.
/// </summary>
public class MatchTimer : MonoBehaviour
{

    #region Properties
    public static MatchTimer Instance { get; private set; }

    public float Duration { get; private set; }

    public bool IsRunning { get; private set; }

    #endregion


    #region Events

    /// <summary>Fired every second with the updated duration string.</summary>
    public event Action<string> OnTimerUpdated;

    #endregion


    #region Methods

    // Todo: To extension method?
    public static string FormatTime(float seconds)
    {
        int mins = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        // Todo: const format.
        return $"{mins:00}:{secs:00}";
    }

    public void ResetTimer()
    {
        Duration = 0f;
        IsRunning = false;
        OnTimerUpdated?.Invoke(FormatTime(0));
    }

    public void StartTimer()
    {
        Duration = 0f;
        IsRunning = true;
    }

    public void StopTimer()
    {
        IsRunning = false;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPlayerWin -= _ => StopTimer();
        GameManager.Instance.OnDraw -= StopTimer;
    }

    private void Start()
    {
        GameManager.Instance.OnPlayerWin += _ => StopTimer();
        GameManager.Instance.OnDraw += StopTimer;
    }

    private void Update()
    {
        if (IsRunning)
        {
            Duration += Time.deltaTime;
            OnTimerUpdated?.Invoke(FormatTime(Duration));
        }
    }

    #endregion

}