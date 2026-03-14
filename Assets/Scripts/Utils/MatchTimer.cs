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

    public void ResetTimer()
    {
        Duration = 0f;
        IsRunning = false;
        OnTimerUpdated?.Invoke(TimerTools.FormatTime(0));
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
        if (GameManager.Instance == null)
        {
            return;
        }

        GameManager.Instance.OnPlayerWin -= OnPlayerWin;
        GameManager.Instance.OnDraw -= StopTimer;

        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void OnPlayerWin(PlayerIndex _) => StopTimer();

    private void Start()
    {
        GameManager.Instance.OnPlayerWin += OnPlayerWin;
        GameManager.Instance.OnDraw += StopTimer;
    }

    private void Update()
    {
        if (IsRunning)
        {
            Duration += Time.deltaTime;
            OnTimerUpdated?.Invoke(TimerTools.FormatTime(Duration));
        }
    }

    #endregion

}