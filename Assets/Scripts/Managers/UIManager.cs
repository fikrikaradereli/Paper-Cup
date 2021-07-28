using System;
using System.Collections.Generic;
using UnityEngine;

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
}