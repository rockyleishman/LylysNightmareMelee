using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public void Resume()
    {
        //play bg music
        EventManager.Instance.OnGameResumed.TriggerEvent(transform.position);

        //resume
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;

        //restore player controls
        DataManager.Instance.PlayerDataObject.Player.GetComponent<PlayerInput>().SwitchCurrentActionMap("MainGameplay");
    }

    public void Pause()
    {
        //disable player controls
        DataManager.Instance.PlayerDataObject.Player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");

        //pause
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;

        //play pause music
        EventManager.Instance.OnGamePaused.TriggerEvent(transform.position);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu"); 
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
