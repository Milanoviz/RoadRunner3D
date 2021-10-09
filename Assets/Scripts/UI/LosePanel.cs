using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LosePanel : MonoBehaviour
{
    [SerializeField] Text _recordText;

    private void Start()
    {
        int lastRunScore = PlayerPrefs.GetInt("lastRunScore");
        int recordScore = PlayerPrefs.GetInt("recordScore");

        if(lastRunScore > recordScore)
        {
            recordScore = lastRunScore;
            PlayerPrefs.SetInt("recordScore", recordScore);
            _recordText.text = $"Best result : {recordScore.ToString()}";
        }
        else
        {
            _recordText.text = $"Best result : {recordScore.ToString()}";
        }
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
