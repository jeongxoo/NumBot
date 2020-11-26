using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void To_main()
    {
        SceneManager.LoadScene("Main");
    }

    public void To_play()
    {
        SceneManager.LoadScene("Play");
    }

    public void Replay()
    {
        SceneManager.LoadScene("Play");
    }
}
