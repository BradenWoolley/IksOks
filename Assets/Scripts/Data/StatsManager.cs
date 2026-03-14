using UnityEngine;


/// <summary>
/// Handling of game stats. Uses player prefs for now
/// but could be modified to make use of different files for protecting data from players.
/// </summary>
public class StatsManager : MonoBehaviour
{

    #region Consts

    private const string Player1 = "Player 1";

    private const string Player2 = "Player 2";

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

        if (result.Contains(Player1))
        {
            data.Player1Wins++;
        }

        else if (result.Contains(Player2))
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
            return;
        }

        Instance = this;

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

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Save()
    {
        PlayerPrefs.SetString(STATS_KEY, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    #endregion

}