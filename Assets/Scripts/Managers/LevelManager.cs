﻿using System.Collections;
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

    private int _platformCount = 3;
    private Vector3 _playerStartPosition = new Vector3(0, 4, 0);
    private float _paperCupYPosition = -4f;

    private int _destroyedBallCount = 0;

    public int maxBallCount = 3;
    public float platformYSize = 0;

    public static event Action OnPlatformComplete;
    public static event Action OnLevelSuccessful;

    private void Start()
    {
        _platforms = new GameObject[_platformCount];
        _paperCups = new GameObject[_platformCount];

        CreatePlayerPlatformsAndPaperCups();
    }

    private void OnEnable()
    {
        MediumCup.OnBallDestroy += HandleBallDestroy;
        MediumCup.OnPlayerReplacement += HandlePlayerReplacement;
    }

    private void OnDisable()
    {
        MediumCup.OnBallDestroy -= HandleBallDestroy;
        MediumCup.OnPlayerReplacement -= HandlePlayerReplacement;
    }

    private void CreatePlayerPlatformsAndPaperCups()
    {
        for (int i = 0; i < _platformCount; i++)
        {
            if (i == 0)
            {
                _platforms[i] = Instantiate(_platformPrefab);
                _paperCups[i] = Instantiate(_paperCupPrefab, new Vector3(0, _paperCupYPosition, 0), Quaternion.identity);
                _paperCups[i].transform.GetChild(0).gameObject.AddComponent<MediumCup>();

                platformYSize = _platforms[i].GetComponent<Renderer>().bounds.size.y;
            }
            else
            {
                _platforms[i] = Instantiate(_platformPrefab,
                                            new Vector3(
                                                _platforms[i - 1].transform.position.x,
                                                _platforms[i - 1].transform.position.y - platformYSize,
                                                _platforms[i - 1].transform.position.z),
                                            Quaternion.AngleAxis(-90, Vector3.right));

                _paperCups[i] = Instantiate(_paperCupPrefab,
                                            new Vector3(
                                                0,
                                                _paperCupYPosition - (i * platformYSize),
                                                0),
                                            Quaternion.identity);
                _paperCups[i].transform.GetChild(0).gameObject.AddComponent<MediumCup>();
            }
        }

        _player = Instantiate(_paperCupPrefab, _playerStartPosition, Quaternion.identity);
        _player.GetComponent<PlayerController>().enabled = true;
        _player.name = "Player";
    }

    private void HandleBallDestroy()
    {
        _destroyedBallCount++;
        Debug.Log("_destroyedBallCount: " + _destroyedBallCount);

        if (_destroyedBallCount == maxBallCount)
        {
            _destroyedBallCount = 0;
            StartCoroutine(EmitPlatformComplete(0.5f));
        }
    }

    private void HandlePlayerReplacement()
    {
        Debug.Log("_paperCups.Length: " + _paperCups.Length);

        Destroy(_player);
        _player = _paperCups[0];

        // Skip(): Bypasses a specified number of elements in a sequence and then returns the remaining elements.
        _paperCups = _paperCups.Skip(1).ToArray();

        _player.GetComponent<PlayerController>().enabled = true;
        _player.name = "Player";

        // _player'dan MediumCup scriptini kaldırır.
        Destroy(_player.transform.GetChild(0).gameObject.GetComponent<MediumCup>());

        // Destroy first platform
        Destroy(_platforms[0]);
        _platforms = _platforms.Skip(1).ToArray();

    }
    private IEnumerator EmitPlatformComplete(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_platforms.Length > 1)
        {
            OnPlatformComplete?.Invoke();
        }
        else
        {
            OnLevelSuccessful?.Invoke();
        }
    }
}