using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameState CurrentGameState { get; private set; }

    public static event Action<GameState> GameStateChange;

    protected override void Awake()
    {
        base.Awake();

        UpdateState(GameState.PREGAME);
    }

    private void OnEnable()
    {
        UIManager.OnStartGameButtonClick += StartGame;
        LevelManager.OnLevelSuccessful += LevelSuccessful;
    }

    private void OnDisable()
    {
        UIManager.OnStartGameButtonClick -= StartGame;
        LevelManager.OnLevelSuccessful -= LevelSuccessful;
    }

    private void UpdateState(GameState state)
    {
        CurrentGameState = state;
        GameStateChange?.Invoke(state); // Emit the game state
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Game Scene");

        UpdateState(GameState.RUNNING);
    }

    private void LevelSuccessful()
    {
        UpdateState(GameState.SUCCESSFUL);
    }

    private void LevelFailed()
    {
        UpdateState(GameState.FAILED);
    }
}