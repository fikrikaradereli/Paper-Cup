using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        LevelManager.OnLevelSuccessful += HandleLevelSuccessful;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelSuccessful -= HandleLevelSuccessful;
    }

    private void HandleLevelSuccessful()
    {
        Debug.Log("Level Successful");
    }

    #region Button Clicks

    public void StartGameButtonOnClick()
    {
        SceneManager.LoadScene("Game Scene");
    }

    #endregion
}