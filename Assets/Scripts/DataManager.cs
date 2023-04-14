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

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public string Name;
    public List<BestScore> bestScores;

    private string saveFilePath;
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
        File.WriteAllText(saveFilePath, JsonUtility.ToJson(bestScoresData));
    }

    public void LoadBestScores()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
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
        saveFilePath = Application.persistentDataPath + "/BestScore.json";
        DontDestroyOnLoad(gameObject);
        LoadBestScores();
    }
}
