using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Multiplier : MonoBehaviour
{
    public int Index { get; set; }
    public bool IsLeft { get; set; }

    private void Start()
    {
        if (IsLeft)
        {
            transform.GetChild(0).GetComponent<Image>().color = GameManager.Instance.CurrentLevel.LeftMultiplierColors[Index];
            transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + GameManager.Instance.CurrentLevel.LeftMultipliers[Index];
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().color = GameManager.Instance.CurrentLevel.RightMultiplierColors[Index];
            transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + GameManager.Instance.CurrentLevel.RightMultipliers[Index];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Top değdi." + "Index " + Index + "IsLeft " + IsLeft);
        }
    }
}
