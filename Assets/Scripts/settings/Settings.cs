using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Difficulty
{
    Easy,
    Normal,
    Hard
}

public class Settings : MonoBehaviour
{
    public TextMeshProUGUI difficultyText;
    private Difficulty difficulty = Difficulty.Normal;

    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance != null)
        {
            System.Enum.TryParse(DataManager.Instance.Difficulty, out difficulty);
            UpdateDifficultyText();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int temp = (int)difficulty;
            difficulty = (Difficulty)(--temp < 0 ? 2 : temp);

            if (DataManager.Instance != null)
            {
                DataManager.Instance.SaveDifficulty(difficulty.ToString());
                UpdateDifficultyText();
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int temp = (int)difficulty;
            difficulty = (Difficulty)(++temp > 2 ? 0 : temp);

            if (DataManager.Instance != null)
            {
                DataManager.Instance.SaveDifficulty(difficulty.ToString());
                UpdateDifficultyText();
            }
        }
    }

    void UpdateDifficultyText()
    {
        difficultyText.text = difficulty.ToString();
    }

    public void GoToBack()
    {
        SceneManager.LoadScene(0);
    }
}
