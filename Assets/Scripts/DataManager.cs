using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class BestScore
{
    public string Name;
    public int Score;

    public BestScore(string name, int score)
    {
        Name = name;
        Score = score;
    }

    public BestScore()
    {
        Name = "";
        Score = 0;
    }
}

[System.Serializable]
public class BestScoresData
{
    public List<BestScore> bestScores;

    public BestScoresData(List<BestScore> bestScores)
    {
        this.bestScores = bestScores;
    }
}

[System.Serializable]
public class SettingsData
{
    public string Difficulty;

    public SettingsData(string difficulty)
    {
        Difficulty = difficulty;
    }
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public string Name;
    public List<BestScore> bestScores;
    public string Difficulty = "Normal";

    public delegate void DifficultyEventHandler(string diffculty);
    public event DifficultyEventHandler onDifficultyChanged;

    private string saveScoreFilePath;
    private string saveSettingsFilePath;
    private const int SCORE_NUM = 5;

    public void UpdateBestScores(int score)
    {
        if (score <= bestScores[SCORE_NUM-1].Score)
        {
            return;
        }

        bestScores.Add(new BestScore(Name, score));
        bestScores.Sort((a, b) => b.Score.CompareTo(a.Score));
        bestScores.RemoveAt(SCORE_NUM);

        BestScoresData bestScoresData = new BestScoresData(bestScores);
        File.WriteAllText(saveScoreFilePath, JsonUtility.ToJson(bestScoresData));
    }

    public void SaveDifficulty(string difficulty)
    {
        Difficulty = difficulty;

        SettingsData data = new SettingsData(Difficulty);
        File.WriteAllText(saveSettingsFilePath, JsonUtility.ToJson(data));
        onDifficultyChanged?.Invoke(Difficulty);
    }

    public void LoadBestScores()
    {
        if (File.Exists(saveScoreFilePath))
        {
            string json = File.ReadAllText(saveScoreFilePath);
            var bestScoresData = JsonUtility.FromJson<BestScoresData>(json);
            bestScores = bestScoresData.bestScores;
        }
        else
        {
            bestScores = new List<BestScore>();
            for (int i = 0; i < SCORE_NUM; i++)
            {
                bestScores.Add(new BestScore());
            }
        }
    }

    public void LoadSettings()
    {
        if (File.Exists(saveSettingsFilePath))
        {
            string json = File.ReadAllText(saveSettingsFilePath);
            var settingsData = JsonUtility.FromJson<SettingsData>(json);
            Difficulty = settingsData.Difficulty;
        }
        else
        {
            Difficulty = "Normal";
        }
    }

    public BestScore GetBestScore()
    {
        return bestScores[0];
    }

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        saveScoreFilePath = Application.persistentDataPath + "/BestScore.json";
        saveSettingsFilePath = Application.persistentDataPath + "/Settings.json";
        DontDestroyOnLoad(gameObject);
        LoadBestScores();
        LoadSettings();
    }
}
