using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class BestScore
{
    public string Name;
    public int Score;
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public string Name;
    public BestScore bestScore;

    private string saveFilePath; 

    public void UpdateBestScore(int score)
    {
        if (bestScore.Score < score)
        {
            bestScore.Score = score;
            bestScore.Name = Name;

            File.WriteAllText(saveFilePath, JsonUtility.ToJson(bestScore));
        }
    }

    public void LoadBestScore()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            bestScore = JsonUtility.FromJson<BestScore>(json);
        }
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
        LoadBestScore();
    }
}
