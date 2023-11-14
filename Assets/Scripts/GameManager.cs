using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public enum GameState
{
    GS_PAUSEMENU,
    GS_GAME,
    GS_LEVELCOMPLETED,
    GS_GAME_OVER
}

public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.GS_PAUSEMENU;
    public Canvas ingameCanvas;
    public Canvas pauseMenuCanvas;
    public TMP_Text scoreText;
    public TMP_Text enemiesDefeatedText;
    public TMP_Text timeCounterText;
    public Image[] keysTab;
    public Image[] livesTab;
    public static GameManager instance;
    private int score = 0;
    private int keysFound = 0;
    private int lives;
    private int enemiesDefeated = 0;
    private float timer = 0;

    void Awake()
    {
        InGame();
        instance = this;
        foreach (Image key in keysTab)
        {
            key.color = Color.gray;
        }
        UpdateCounters();
        lives = livesTab.Length;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) 
        { 
            if(currentGameState == GameState.GS_PAUSEMENU)
            {
                InGame();
            }
            else if (currentGameState == GameState.GS_GAME)
            {
                PauseMenu();
            }
        }

        if (currentGameState == GameState.GS_GAME)
        {
            timer += Time.deltaTime;
        }
        UpdateCounters();
    }

    public void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;

        ingameCanvas.enabled = currentGameState == GameState.GS_GAME;
        pauseMenuCanvas.enabled = currentGameState == GameState.GS_PAUSEMENU;   
    }

    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
        Time.timeScale = 0.0f;
    }

    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
        Time.timeScale = 1.0f;
    }

    public void LevelCompleted()
    {
        if (keysFound == keysTab.Length)
        {
             SetGameState(GameState.GS_LEVELCOMPLETED);
        }
        else
        {
            Debug.Log($"You need to find {keysTab.Length - keysFound} more keys!");
        }
    }

    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }

    public void AddPoints(int points)
    {
        score += points;
    }

    public void IncrementLives()
    {
        if (lives >= 0 && lives < livesTab.Length) livesTab[lives].color = Color.white;
        if (lives > 0 && lives < livesTab.Length) lives += 1;
    }

    public void DecrementLives()
    {
        lives -= 1;
        if (lives >= 0 && lives <= livesTab.Length) livesTab[lives].color = Color.gray;

        if(lives <= 0)
        {
            GameOver();
        }
    }

    public void IncrementEnemiesDefeated()
    {
        enemiesDefeated += 1;
        AddPoints(1);
    }

    public void AddKey(Color keyColor)
    {
        keysTab[keysFound].color = keyColor;
        keysFound += 1;
    }

    public void UpdateCounters()
    {
        scoreText.text = score.ToString();
        enemiesDefeatedText.text = enemiesDefeated.ToString();
        float minutes = timer / 60.0f, seconds = timer % 60.0f;
        timeCounterText.text = string.Format("{0:00}:{1:00}", Math.Floor(minutes), Math.Floor(seconds));
    }

    public void OnResumeClick()
    {
        InGame();
    }
    public void OnRestartClick()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnReturnToMainMenuButtonClicked()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}
