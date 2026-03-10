using UnityEngine;


// <summary>
/// Todo: JSON persistence or simply PlayerPrefs for now?
/// </summary>
public class StatsManager : MonoBehaviour
{

    #region Properties

    public static StatsManager Instance { get; private set; }

    #endregion


    #region Methods

    public void RecordMatchResult(string result, float duration)
    {
        // Todo: Implement logic.
        Debug.Log($"[StatsManager] Result: {result} | Duration: {duration:F1}s");
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    #endregion

}