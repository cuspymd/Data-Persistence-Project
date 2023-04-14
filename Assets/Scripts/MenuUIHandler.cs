using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUIHandler : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateBestScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void GoToScoreScene()
    {
        SceneManager.LoadScene(2);
    }

    public void OnNameEndEdit(string name)
    {
        Debug.Log($"Input name {name}");
        DataManager.Instance.Name = name;
    }

    private void UpdateBestScoreText()
    {
        BestScore bestScore = DataManager.Instance.GetBestScore();

        if (string.IsNullOrEmpty(bestScore.Name))
        {
            bestScoreText.text = $"Best Score : {bestScore.Score}";
        }
        else
        {
            bestScoreText.text = $"Best Score : {bestScore.Name} : {bestScore.Score}";
        }
    }
}
