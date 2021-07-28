using System;
using System.Collections.Generic;
using UnityEngine;

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
        LevelManager.OnLevelSuccessful += LevelSuccessful;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelSuccessful -= LevelSuccessful;
    }

    private void UpdateState(GameState state)
    {
        CurrentGameState = state;
        GameStateChange?.Invoke(state); // Emit the game state
    }

    public void StartGame()
    {
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