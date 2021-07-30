using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void OnEnable()
    {
        LevelManager.OnPlatformComplete += HandlePlatformComplete;
    }

    private void OnDisable()
    {
        LevelManager.OnPlatformComplete -= HandlePlatformComplete;
    }

    private void HandlePlatformComplete()
    {
        StopAllCoroutines();
        StartCoroutine(Shifting(transform.position.y - LevelManager.Instance.PlatformYSize));
    }

    IEnumerator Shifting(float desiredPosY)
    {
        float t = 0;
        Vector3 initialPos = transform.position;
        Vector3 targetPos = new Vector3(transform.position.x, desiredPosY, transform.position.z);

        while (!Mathf.Approximately(transform.position.y, desiredPosY))
        {
            t += Time.deltaTime;

            transform.position = Vector3.Lerp(initialPos, targetPos, t);
            yield return null;
        }
        transform.position = targetPos;
    }
}