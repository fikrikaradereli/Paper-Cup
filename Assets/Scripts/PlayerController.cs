using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject _ballPrefab;
    [SerializeField]
    private Transform _ballCreationPoint;
    [SerializeField]
    private Transform _ballCreationPoint2;

    // Drag
    private float _deltaX;
    private float _dragScaler = 0.0025f;

    // Movement restriction
    private float _rightMovementLimit = 0.6748527f;
    private float _leftMovementLimit = -3.7f;
    private float _rightBallCreationLimit = 0.25f;

    // Creating balls
    private int _createdBallCount = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _deltaX = Input.mousePosition.x - transform.position.x; // For drag

            StopAllCoroutines();
            StartCoroutine(RotateZ(-90));

            InvokeRepeating(nameof(CreateBall), 0.5f, 1f);
        }
        else if (Input.GetMouseButton(0))
        {
            Drag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopAllCoroutines();
            StartCoroutine(RotateZ(0));

            CancelInvoke(nameof(CreateBall));
        }
    }

    private void Drag()
    {
        transform.position = new Vector3((Input.mousePosition.x - _deltaX) * _dragScaler, transform.position.y, transform.position.z);

        // Movement restriction
        if (transform.position.x < _leftMovementLimit)
        {
            transform.position = new Vector3(_leftMovementLimit, transform.position.y, transform.position.z);
        }

        if (transform.position.x > _rightMovementLimit)
        {
            transform.position = new Vector3(_rightMovementLimit, transform.position.y, transform.position.z);
        }
    }

    IEnumerator RotateZ(float targetAngle)
    {
        while (transform.rotation.z != targetAngle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, targetAngle), 7.5f * Time.deltaTime);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        yield return null;
    }

    private void CreateBall()
    {
        if (LevelManager.Instance.FabricableBallCount > _createdBallCount)
        {
            if (transform.position.x > _rightBallCreationLimit)
            {
                Instantiate(_ballPrefab, _ballCreationPoint2.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_ballPrefab, _ballCreationPoint.position, Quaternion.identity);
            }
            _createdBallCount++;
        }
    }
}