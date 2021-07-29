using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public static event Action OnStartGameButtonClick;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        GameManager.GameStateChange += HandleGameStateChange;
    }

    private void OnDisable()
    {
        GameManager.GameStateChange -= HandleGameStateChange;
    }

    private void HandleGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.PREGAME:
                break;
            case GameState.RUNNING:
                break;
            case GameState.SUCCESSFUL:
                Successful();
                break;
            case GameState.FAILED:
                break;
            case GameState.END:
                break;
            default:
                break;
        }
    }

    private void Successful()
    {
        Debug.Log("Level Successful");
    }

    #region Button Clicks

    public void StartGameButtonOnClick()
    {
        OnStartGameButtonClick?.Invoke();
    }

    #endregion
}