using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
    private Button playAgainButton;

    private void Awake()
    {
        playAgainButton = GetComponent<Button>();
    }

    private void Start()
    {
        playAgainButton.onClick.AddListener(RestartLevel);
    }

    /// <summary>
    /// Restarts the current level.
    /// </summary>
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
