using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Scores : MonoBehaviour
{
    public TextMeshProUGUI scoresText;

    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance == null)
        {
            return;
        }

        UpdateScoresText(DataManager.Instance.bestScores);
    }

    private void UpdateScoresText(List<BestScore> bestScores)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < bestScores.Count; i++)
        {
            var score = bestScores[i];
            sb.Append($"{i+1}. ");

            if (!string.IsNullOrEmpty(score.Name))
            {
                sb.Append($"{score.Name} : {score.Score}");
            }

            if (i < bestScores.Count-1) 
            {
                sb.AppendLine();
            }
        }

        scoresText.text = sb.ToString();
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
