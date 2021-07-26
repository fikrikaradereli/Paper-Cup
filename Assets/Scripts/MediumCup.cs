using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MediumCup : MonoBehaviour
{
    public static event Action OnBallDestroy;

    private float _ballDestroyDelay = 0.75f;
    private List<int> _collidedBalls = new List<int>();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Avoids same ball's collision detection
            int id = collision.gameObject.GetInstanceID();
            if (!_collidedBalls.Contains(id))
            {
                _collidedBalls.Add(id);

                Destroy(collision.gameObject, _ballDestroyDelay);
                OnBallDestroy?.Invoke();
            }
        }
    }
}