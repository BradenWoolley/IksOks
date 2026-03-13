using UnityEngine;


// <summary>
/// Todo: JSON persistence or simply PlayerPrefs for now?
/// </summary>
public class StatsManager : MonoBehaviour
{

    #region Consts

    private const string STATS_KEY = "game_stats";

    #endregion


    #region Fields

    private StatsData data;

    #endregion


    #region Properties

    public static StatsManager Instance { get; private set; }

    #endregion


    #region Methods

    public StatsData GetStats()
    {
        return data;
    }

    public void RecordMatchResult(string result, float duration)
    {
        data.TotalGames++;
        data.TotalDuration += duration;

        // Todo: consts
        if (result.Contains("Player 1"))
        {
            data.Player1Wins++;
        }

        else if (result.Contains("Player 2"))
        {
            data.Player2Wins++;
        }
        else
        {
            data.Draws++;
        }

        Save();
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

        DontDestroyOnLoad(gameObject);
        Load();
    }

    private void Load()
    {
        string json = PlayerPrefs.GetString(STATS_KEY, "");

        data = string.IsNullOrEmpty(json)
            ? new StatsData()
            : JsonUtility.FromJson<StatsData>(json);
    }

    private void Save()
    {
        PlayerPrefs.SetString(STATS_KEY, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    #endregion

}