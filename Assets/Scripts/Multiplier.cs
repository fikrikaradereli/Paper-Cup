using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Multiplier : MonoBehaviour
{
    [SerializeField]
    private GameObject _ballPrefab;

    private List<int> _collidedBalls = new List<int>();

    public static event Action<int> OnMultiplierCollision;


    public int Index { get; set; }
    public bool IsLeft { get; set; }

    private int _multiplier;

    private Vector3 _offset = new Vector3(0, 0.5f, 0);

    private void Start()
    {
        if (IsLeft)
        {
            transform.GetChild(0).GetComponent<Image>().color = GameManager.Instance.CurrentLevel.LeftMultiplierColors[Index];
            transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + GameManager.Instance.CurrentLevel.LeftMultipliers[Index];

            _multiplier = GameManager.Instance.CurrentLevel.LeftMultipliers[Index];
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().color = GameManager.Instance.CurrentLevel.RightMultiplierColors[Index];
            transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + GameManager.Instance.CurrentLevel.RightMultipliers[Index];

            _multiplier = GameManager.Instance.CurrentLevel.RightMultipliers[Index];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            // Avoids same ball's collision detection
            int id = other.gameObject.GetInstanceID();
            if (!_collidedBalls.Contains(id))
            {
                _collidedBalls.Add(id);

                for (int i = 0; i < _multiplier; i++)
                {
                    GameObject ball = Instantiate(_ballPrefab, other.gameObject.transform.position - _offset, Quaternion.identity);
                    _collidedBalls.Add(ball.GetInstanceID());
                }

                OnMultiplierCollision?.Invoke(_multiplier);
            }
        }
    }
}
