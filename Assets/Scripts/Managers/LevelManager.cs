using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject _paperCupPrefab;
    [SerializeField]
    private GameObject _platformPrefab;

    private GameObject _player;
    private GameObject[] _platforms;
    private GameObject[] _paperCups;

    private int _platformCount;
    private Vector3 _playerStartPosition = new Vector3(0, 4.5f, 0);
    private float _paperCupYPosition = -4f;

    private int _destroyedBallCount = 0;

    public int BallCount { get; private set; }
    public int FabricableBallCount { get; private set; }
    public float PlatformYSize { get; private set; } = 0;

    public static event Action OnPlatformComplete;
    public static event Action OnLevelSuccessful;
    public static event Action OnLevelFailed;
    public static event Action<Transform> OnLastPaperCupCreate;
    public static event Action<int> OnScoreAdd;

    private void Start()
    {
        _platformCount = GameManager.Instance.CurrentLevel.PlatformCount;

        _platforms = new GameObject[_platformCount];
        _paperCups = new GameObject[_platformCount];

        BallCount = GameManager.Instance.CurrentLevel.BeginingBallCount;
        FabricableBallCount = BallCount;

        CreatePlayerPlatformsAndPaperCups();
    }

    private void OnEnable()
    {
        MediumCup.OnBallDestroy += HandleBallDestroy;
        MediumCup.OnPlayerReplacement += HandlePlayerReplacement;
        Multiplier.OnMultiplierCollision += HandleMultiplierCollision;
    }

    private void OnDisable()
    {
        MediumCup.OnBallDestroy -= HandleBallDestroy;
        MediumCup.OnPlayerReplacement -= HandlePlayerReplacement;
        Multiplier.OnMultiplierCollision -= HandleMultiplierCollision;
    }

    private void CreatePlayerPlatformsAndPaperCups()
    {
        for (int i = 0; i < _platformCount; i++)
        {
            if (i == 0)
            {
                _platforms[i] = Instantiate(_platformPrefab);
                _platforms[i].transform.GetChild(0).GetComponent<Multiplier>().IsLeft = true;
                _platforms[i].transform.GetChild(0).GetComponent<Multiplier>().Index = i;
                _platforms[i].transform.GetChild(1).GetComponent<Multiplier>().IsLeft = false;
                _platforms[i].transform.GetChild(1).GetComponent<Multiplier>().Index = i;

                _paperCups[i] = Instantiate(_paperCupPrefab, new Vector3(0, _paperCupYPosition, 0), Quaternion.identity);
                _paperCups[i].transform.GetChild(0).gameObject.AddComponent<MediumCup>();

                PlatformYSize = _platforms[i].GetComponent<Renderer>().bounds.size.y;
            }
            else
            {
                _platforms[i] = Instantiate(_platformPrefab,
                                            new Vector3(
                                                _platforms[i - 1].transform.position.x,
                                                _platforms[i - 1].transform.position.y - PlatformYSize,
                                                _platforms[i - 1].transform.position.z),
                                            Quaternion.AngleAxis(-90, Vector3.right));

                _platforms[i].transform.GetChild(0).GetComponent<Multiplier>().IsLeft = true;
                _platforms[i].transform.GetChild(0).GetComponent<Multiplier>().Index = i;
                _platforms[i].transform.GetChild(1).GetComponent<Multiplier>().IsLeft = false;
                _platforms[i].transform.GetChild(1).GetComponent<Multiplier>().Index = i;

                _paperCups[i] = Instantiate(_paperCupPrefab,
                                            new Vector3(
                                                0,
                                                _paperCupYPosition - (i * PlatformYSize),
                                                0),
                                            Quaternion.identity);
                _paperCups[i].transform.GetChild(0).gameObject.AddComponent<MediumCup>();
            }
        }

        OnLastPaperCupCreate?.Invoke(_paperCups[_paperCups.Length - 1].transform);

        _player = Instantiate(_paperCupPrefab, _playerStartPosition, Quaternion.identity);
        _player.GetComponent<PlayerController>().enabled = true;
        _player.name = "Player";
    }

    private void HandleBallDestroy()
    {
        _destroyedBallCount++;

        if (_platforms.Length == 1)
        {
            OnScoreAdd?.Invoke(_destroyedBallCount);
        }

        if (_destroyedBallCount == BallCount)
        {
            StartCoroutine(EmitPlatformComplete(0.5f));
        }
    }

    private void HandlePlayerReplacement()
    {
        Destroy(_player);
        _player = _paperCups[0];

        // Skip(): Bypasses a specified number of elements in a sequence and then returns the remaining elements.
        _paperCups = _paperCups.Skip(1).ToArray();

        _player.GetComponent<PlayerController>().enabled = true;
        _player.name = "Player";

        // Removes MediumCup script from _player game object.
        Destroy(_player.transform.GetChild(0).gameObject.GetComponent<MediumCup>());

        // Destroy first platform
        Destroy(_platforms[0]);
        _platforms = _platforms.Skip(1).ToArray();

    }

    private void HandleMultiplierCollision(int multiplier)
    {
        BallCount += multiplier;
    }

    private IEnumerator EmitPlatformComplete(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_platforms.Length > 1)
        {
            _destroyedBallCount = 0;
            FabricableBallCount = BallCount;
            OnPlatformComplete?.Invoke();
        }
        else
        {
            if (_destroyedBallCount > GameManager.Instance.CurrentLevel.BallCountForSuccess)
            {
                OnLevelSuccessful?.Invoke();
            }
            else
            {
                OnLevelFailed?.Invoke();
            }
        }
    }
}