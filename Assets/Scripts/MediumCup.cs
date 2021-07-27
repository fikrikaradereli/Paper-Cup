using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MediumCup : MonoBehaviour
{
    private List<int> _collidedBalls = new List<int>();

    public static event Action OnBallDestroy;
    public static event Action OnPlayerReplacement;

    private float _ballDestroyDelay = 0.75f;

    private void OnEnable()
    {
        LevelManager.OnPlatformComplete += HandlePlatformComplete;
    }

    private void OnDisable()
    {
        LevelManager.OnPlatformComplete -= HandlePlatformComplete;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Removes ball's bouncy physics material
            collision.collider.material = null;

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

    private void HandlePlatformComplete()
    {
        if (_collidedBalls.Count > 0)
        {
            StopAllCoroutines();
            StartCoroutine(Shifting(transform.parent.position.y - 5f));
        }
    }

    IEnumerator Shifting(float desiredPosY)
    {
        float t = 0;
        Vector3 initialPos = transform.parent.position;
        Vector3 targetPos = new Vector3(transform.parent.position.x, desiredPosY, transform.parent.position.z);

        // Shifts parent object that is to say shifts all materials of paper cup object; paper cup, sleeve etc.
        while (!Mathf.Approximately(transform.parent.position.y, desiredPosY))
        {
            t += Time.deltaTime;
            transform.parent.position = Vector3.Lerp(initialPos, targetPos, t);
            yield return null;
        }

        transform.parent.position = targetPos;

        OnPlayerReplacement?.Invoke();
    }
}