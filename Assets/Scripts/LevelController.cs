using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private GameObject _paperCupPrefab;
    [SerializeField]
    private GameObject _platformPrefab;

    private GameObject _playerPaperCup;
    private GameObject[] _platforms;
    private GameObject[] _paperCups;

    private int _platformCount = 3;
    private Vector3 _playerStartPosition = new Vector3(0, 4, 0);
    private float _paperCupYPosition = -4f;

    private int _destroyedBallCount = 0;

    private void Start()
    {
        _platforms = new GameObject[_platformCount];
        _paperCups = new GameObject[_platformCount];

        CreatePlayerPlatformsAndPaperCups();
    }

    private void OnEnable()
    {
        MediumCup.OnBallDestroy += HandleBallDestroy;
    }

    private void OnDisable()
    {
        MediumCup.OnBallDestroy -= HandleBallDestroy;
    }

    private void CreatePlayerPlatformsAndPaperCups()
    {
        float platformYSize = 0;

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

        _playerPaperCup = Instantiate(_paperCupPrefab, _playerStartPosition, Quaternion.identity);
        _playerPaperCup.GetComponent<PlayerController>().enabled = true;
        _playerPaperCup.name = "Player";
    }

    private void HandleBallDestroy()
    {
        _destroyedBallCount++;
        Debug.Log(_destroyedBallCount);
    }
}