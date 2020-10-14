using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Scoreboard scoreboard;
    [SerializeField] private GameObject winMessage;
    [SerializeField] private Text timeText;
    [SerializeField] private int matchTime = 120;


    private void Awake()
    {
        slider.maxValue = matchTime;
        UpdateUI();
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DecrementTime", 1f, 1f);
    }

    /// <summary>
    /// Decrements time.
    /// </summary>
    void DecrementTime()
    {
        matchTime--;
        UpdateUI();

        if (matchTime == 0)
        {
            EndGame();
        }
    }

    /// <summary>
    /// Ends the game.
    /// </summary>
    public void EndGame()
    {
        CancelInvoke(); // Stops timer.
        ShowWinMessage();
        scoreboard.StopAllCoroutines(); // Stop moving all players on leaderboard.
        Time.timeScale = 0;
    }

    /// <summary>
    /// Shows the player who won.
    /// </summary>
    private void ShowWinMessage()
    {
        winMessage.SetActive(true);
        Text winText = winMessage.GetComponentInChildren<Text>();
        winText.text = scoreboard.GetWinner() + " Wins!";
    }

    /// <summary>
    /// Updates the clock.
    /// </summary>
    void UpdateUI()
    {
        slider.value = matchTime;
        timeText.text = GetRemainingTimeText();
    }

    /// <summary>
    /// Converts current time to "m:ss"
    /// </summary>
    /// <returns></returns>
    private string GetRemainingTimeText()
    {
        int minutes = matchTime / 60;
        int seconds = matchTime % 60;

        if (seconds < 10)
        {
            return minutes + ":0" + seconds;
        }

        return minutes + ":" + seconds;
    }
}
