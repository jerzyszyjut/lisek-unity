using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

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
    public TMP_Text scoreText;
    public Image[] keysTab;
    public static GameManager instance;
    private int score = 0;
    private int keysFound = 0;

    void Awake()
    { 
        instance = this;
        foreach (Image key in keysTab)
        {
            key.color = Color.gray;
        }
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
    }

    public void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;

        ingameCanvas.enabled = currentGameState == GameState.GS_GAME;
    }

    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }

    public void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }

    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public void AddKey(Color keyColor)
    {
        keysTab[keysFound].color = keyColor;
        keysFound += 1;
    }
}
