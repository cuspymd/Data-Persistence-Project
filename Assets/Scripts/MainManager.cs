using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public static float maxBallSpeed = 2;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        AddPoint(0);
        UpdateBestScoreText();
        if (DataManager.Instance != null)
        {
            UpdateBallSpeed(DataManager.Instance.Difficulty);
            RegisterDifficultyEvent();
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        if (DataManager.Instance)
        {
            ScoreText.text = $"Score : {DataManager.Instance.Name} : {m_Points}";
        } 
        else
        {
            ScoreText.text = $"Score : {m_Points}";
        }
        
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (DataManager.Instance)
        {
            DataManager.Instance.UpdateBestScores(m_Points);
            UpdateBestScoreText();
        }
    }

    private void UpdateBestScoreText()
    {
        BestScore bestScore = new BestScore();

        if (DataManager.Instance)
        {
            bestScore = DataManager.Instance.GetBestScore();

        }

        if (string.IsNullOrEmpty(bestScore.Name))
        {
            BestScoreText.text = $"Best Score : {bestScore.Score}";
        }
        else
        {
            BestScoreText.text = $"Best Score : {bestScore.Name} : {bestScore.Score}";
        }
    }

    public void UpdateBallSpeed(string difficulty)
    {
        if (difficulty == "Easy")
        {
            maxBallSpeed = 3;
        }
        else if (difficulty == "Normal")
        {
            maxBallSpeed = 6;
        }
        else if (difficulty == "Hard")
        {
            maxBallSpeed = 9;
        }
    }

    private void RegisterDifficultyEvent() 
    {
        if (DataManager.Instance) 
        {
            DataManager.Instance.onDifficultyChanged += UpdateBallSpeed;
        }
    }

    private void UnregisterDifficultyEvent()
    {
        if (DataManager.Instance)
        {
            DataManager.Instance.onDifficultyChanged -= UpdateBallSpeed;
        }
    }

    private void OnDestroy()
    {
        UnregisterDifficultyEvent();
    }
}
