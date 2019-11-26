﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public int timeLimit = 120;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text timeText;

    private SavingData? savingData;

    private int score;

    private void Start()
    {
        var slot = SaveLoadSystem.GetCurrentSlot();
        savingData = SaveLoadSystem.Load(slot);
        score = !savingData.HasValue ? 0 : savingData.Value.score;
        if (!scoreText)
        {
            Debug.LogError("Score Text not assigned to ScoreSystem");
            Destroy(this);
        }

        if (!scoreText)
        {
            Debug.LogError("Time Text not assigned to ScoreSystem");
            Destroy(this);
        }
    }

    private void Update()
    {
        scoreText.text = $"Score: {score.ToString()}";

        var time = timeLimit - Time.timeSinceLevelLoad;

        if (time > 0)
        {
            var minutes = Mathf.FloorToInt(time / 60).ToString("00");
            var seconds = Mathf.FloorToInt(time % 60).ToString("00");
            timeText.text = $"{minutes}:{seconds}";
        }
        else
        {
            Time.timeScale = 0;

            // Show a gameover message
            FindObjectOfType<ModalScreenSystem>().Show("TIME OUT!", "Try again?", () =>
            {
                // Restart Scene 
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
            return;
        }

    }

    public void AddPoints(int points)
    {
        score += points;
    }
}
