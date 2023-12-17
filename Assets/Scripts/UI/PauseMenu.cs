using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;

        //restore player controls
        DataManager.Instance.PlayerDataObject.Player.GetComponent<PlayerInput>().SwitchCurrentActionMap("MainGameplay");
    }

    public void Pause()
    {
        //disable player controls
        DataManager.Instance.PlayerDataObject.Player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
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
