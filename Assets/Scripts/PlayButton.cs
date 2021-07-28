using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void PlayButtonOnClick()
    {
        SceneManager.LoadScene("Game Scene");
    }
}