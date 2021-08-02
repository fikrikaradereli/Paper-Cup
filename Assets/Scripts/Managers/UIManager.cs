using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private GameObject _scoreTextCanvasPrefab;
    [SerializeField]
    private CanvasGroup _popupMenuSuccessful;
    [SerializeField]
    private CanvasGroup _popupMenuFail;
    [SerializeField]
    private TextMeshProUGUI _totalScoreText;

    private TextMeshProUGUI _scoreText;

    public static event Action OnStartGameButtonClick;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        GameManager.GameStateChange += HandleGameStateChange;
        LevelManager.OnLastPaperCupCreate += HandleLastPaperCupCreate;
        LevelManager.OnScoreAdd += HandleScoreAdd;
    }

    private void OnDisable()
    {
        GameManager.GameStateChange -= HandleGameStateChange;
        LevelManager.OnLastPaperCupCreate -= HandleLastPaperCupCreate;
        LevelManager.OnScoreAdd -= HandleScoreAdd;
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
        GameObject scoreTextCanvas = Instantiate(_scoreTextCanvasPrefab, transform);
        _scoreText = scoreTextCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void HandleScoreAdd(int score)
    {
        _scoreText.text = score + "/" + GameManager.Instance.CurrentLevel.BallCountForSuccess;
    }

    private void Successful()
    {
        _popupMenuSuccessful.gameObject.SetActive(true);
        _totalScoreText.text = GameManager.Instance.TotalScore.ToString();
        _popupMenuSuccessful.alpha = 0;
        _popupMenuSuccessful.LeanAlpha(1f, .5f);
    }

    private void Failed()
    {
        _popupMenuFail.gameObject.SetActive(true);
        _popupMenuFail.alpha = 0;
        _popupMenuFail.LeanAlpha(1f, .5f);
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

    public void NextLevelButtonOnClick()
    {
        Debug.Log("NEXT LEVEL");
    }

    public void RestartButtonOnClick()
    {
        _popupMenuFail.gameObject.SetActive(false);
        StartGameButtonOnClick();
    }

    #endregion
}