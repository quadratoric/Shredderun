using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardController : MonoBehaviour
{
    int[] runDeaths;
    string[] runTimes;

    public TextMeshProUGUI[] texts;
    
    void Start()
    {
        runDeaths = ScoreManager.instance.GetRunDeaths();
        runTimes = ScoreManager.instance.GetRunTimes();
        
        List<RunData> runDatas = new List<RunData>();
        
        for (int i = 0; i < runDeaths.Length; i++) {
            if (runDeaths[i] == 99) {
                runDatas.Add(new RunData(0, runDeaths[i], runTimes[i]));
            } else {
                runDatas.Add(new RunData(i+1, runDeaths[i], runTimes[i]));
            }   
        }

        runDatas.Sort((a, b) =>
        {
            int deathComparison = a.deaths.CompareTo(b.deaths);
            if (deathComparison == 0)
            {
                return a.time.CompareTo(b.time);
            }
            return deathComparison;
        });

        for (int i = 0; i < 5; i++)
        {
            if (i < runDatas.Count)  // Ensure we don't go out of bounds
            {
                var run = runDatas[i];
                texts[i].text = $"{run.runIndex}  -  {run.deaths}  -  {run.time}";
            } else {
                texts[i].text = $"?  -  ??  -  ??:??:??";
            }
        }
    }
    
    private class RunData {
        public int runIndex;
        public int deaths;
        public string time;

        public RunData(int runIndex, int deaths, string time) {
            this.runIndex = runIndex;
            this.deaths = deaths;
            this.time = time;
        }
    }
}

