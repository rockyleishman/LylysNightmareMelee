using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    void Update()
    {
        // Check for a mouse click or touch input
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Play();
        }
    }
    public void Play()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Level_01"))
        {
            SceneManager.LoadScene("Level_01");
        }        
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
