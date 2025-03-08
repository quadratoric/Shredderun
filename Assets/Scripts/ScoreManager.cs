using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Track the last 5 runs
    private int[] runDeaths = new int[5];
    private string[] runTimes = new string[5];
    private int currentRunIndex = 0;

    // Keys for PlayerPrefs
    private const string DeathsKey = "RunDeaths";
    private const string TimesKey = "RunTimes";
    private const string RunIndexKey = "RunIndex";

    // Singleton instance (Optional, can be used to easily access from other scripts)
    public static ScoreManager instance;

    void Awake()
    {
        // Make sure there is only one instance of ScoreManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        // Load existing data from PlayerPrefs
        LoadData();
    }

    // Load data from PlayerPrefs
    private void LoadData()
    {
        for (int i = 0; i < 5; i++)
        {
            runDeaths[i] = PlayerPrefs.GetInt(DeathsKey + i, 99);  // Default to 0 if no data
            runTimes[i] = PlayerPrefs.GetString(TimesKey + i, "99:99:99");  // Default to max time if no data
        }

        currentRunIndex = PlayerPrefs.GetInt(RunIndexKey, 0); // Load the current run index
    }

    // Save data to PlayerPrefs
    private void SaveData()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt(DeathsKey + i, runDeaths[i]);
            PlayerPrefs.SetString(TimesKey + i, runTimes[i]);
        }

        PlayerPrefs.SetInt(RunIndexKey, currentRunIndex);
        PlayerPrefs.Save();
    }

    // Call this at the end of each run
    public void EndRun(int deaths, string timeTaken)
    {
        // Store the run data at the current run index
        runDeaths[currentRunIndex] = deaths;
        runTimes[currentRunIndex] = timeTaken;

        // Update the current run index, cycling through the array (0 to 4)
        currentRunIndex = (currentRunIndex + 1) % 5;

        // Save the updated data to PlayerPrefs
        SaveData();

        // Log the updated scoreboard for debugging
        Debug.Log("Run " + (currentRunIndex) + ": Deaths = " + deaths + ", Time = " + timeTaken);
    }

    public int[] GetRunDeaths() => runDeaths;
    public string[] GetRunTimes() => runTimes;
}
