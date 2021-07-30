using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private GameObject _scoreTextPrefab;

    public static event Action OnStartGameButtonClick;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        GameManager.GameStateChange += HandleGameStateChange;
        LevelManager.OnLastPaperCupCreate += HandleLastPaperCupCreate;
    }

    private void OnDisable()
    {
        GameManager.GameStateChange -= HandleGameStateChange;
        LevelManager.OnLastPaperCupCreate -= HandleLastPaperCupCreate;
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
                Failed();
                break;
            case GameState.END:
                End();
                break;
            default:
                break;
        }
    }

    private void HandleLastPaperCupCreate(Transform transform)
    {
        GameObject scoreTextGameObject = Instantiate(_scoreTextPrefab, transform);
    }

    private void Successful()
    {
        Debug.Log("Level Successful");
    }
    private void Failed()
    {
        Debug.Log("Level Failed");
    }

    private void End()
    {
        Debug.Log("The End");
    }

    #region Button Clicks

    public void StartGameButtonOnClick()
    {
        OnStartGameButtonClick?.Invoke();
    }

    #endregion
}