using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameState CurrentGameState { get; private set; }
    public Level CurrentLevel { get; private set; }
    public int TotalScore { get; private set; }

    [SerializeField]
    private List<Level> levels;

    public static event Action<GameState> GameStateChange;

    protected override void Awake()
    {
        base.Awake();

        UpdateState(GameState.PREGAME);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int currentLevelIndex = PlayerPrefs.GetInt(Constants.PLAYER_PREFS_CURRENT_LEVEL_INDEX, -1);

        // If PLAYER_PREFS_CURRENT_LEVEL_INDEX does not exist it is return -1
        if (currentLevelIndex == -1)
        {
            CurrentLevel = levels[0];
        }
        else
        {
            CurrentLevel = levels[currentLevelIndex];
        }

        TotalScore = PlayerPrefs.GetInt(Constants.PLAYER_PREFS_TOTAL_SCORE, 0);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        UIManager.OnStartGameScene += StartGame;
        LevelManager.OnLevelSuccessful += LevelSuccessful;
        LevelManager.OnLevelFailed += LevelFailed;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        UIManager.OnStartGameScene -= StartGame;
        LevelManager.OnLevelSuccessful -= LevelSuccessful;
        LevelManager.OnLevelFailed -= LevelFailed;
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

    private void LevelSuccessful(int score)
    {
        TotalScore += score;
        PlayerPrefs.SetInt(Constants.PLAYER_PREFS_TOTAL_SCORE, TotalScore); // saves total score

        // Check whether it is the last level.
        if (levels.IndexOf(CurrentLevel) == levels.Count - 1)
        {
            UpdateState(GameState.END);
        }
        else
        {
            UpdateState(GameState.SUCCESSFUL);
            PlayerPrefs.SetInt(Constants.PLAYER_PREFS_CURRENT_LEVEL_INDEX, levels.IndexOf(CurrentLevel) + 1); // increase the level
        }
    }

    private void LevelFailed()
    {
        UpdateState(GameState.FAILED);
    }
}