using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    InputAction pauseGame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseGame = InputSystem.actions.FindAction("Pause");
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseGame.WasPerformedThisFrame())
        {
            if (GameManager.Instance.isGamePaused)
            {
                Resume();
            }
            else
            {
                transform.Find("Canvas").gameObject.SetActive(true);
                GameManager.Instance.isGamePaused = true;
                Time.timeScale = 0f;
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameManager.Instance.isGamePaused = false;
        transform.Find("Canvas").gameObject.SetActive(false);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        GameManager.Instance.winner = Node.State.Empty;
        GameManager.Instance.isGamePaused = false;
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
