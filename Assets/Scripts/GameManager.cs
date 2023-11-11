using UnityEngine;

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
    public static GameManager instance;

    void Awake()
    { 
        instance = this;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) 
        { 
            if(currentGameState == GameState.GS_PAUSEMENU)
            {
                currentGameState = GameState.GS_GAME;
            }
            else if (currentGameState == GameState.GS_GAME)
            {
                currentGameState = GameState.GS_PAUSEMENU;
            }
        }
    }

    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
    }

    void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }

    void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }

    void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }
}
