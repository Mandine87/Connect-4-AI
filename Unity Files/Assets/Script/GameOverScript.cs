using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    void Start()
    {
        //Display the winner
        if(GameManager.Instance.winner == Node.State.Player1)
        {
            transform.Find("Ex aequo").gameObject.SetActive(false);
            transform.Find("WinnerText").gameObject.SetActive(true);
            transform.Find("WinnerText").Find("Player1").gameObject.SetActive(true);
            transform.Find("WinnerText").Find("Player2").gameObject.SetActive(false);

        }
        else if (GameManager.Instance.winner == Node.State.Player2)
        {
            transform.Find("Ex aequo").gameObject.SetActive(false);
            transform.Find("WinnerText").gameObject.SetActive(true);
            transform.Find("WinnerText").Find("Player1").gameObject.SetActive(false);
            transform.Find("WinnerText").Find("Player2").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("WinnerText").gameObject.SetActive(false);
            transform.Find("Ex aequo").gameObject.SetActive(true);
        }
    }

    public void Rematch()
    {
        GameManager.Instance.winner = Node.State.Empty;
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToMenu()
    {
        GameManager.Instance.winner = Node.State.Empty;
        SceneManager.LoadScene("Menu");
    }
}
