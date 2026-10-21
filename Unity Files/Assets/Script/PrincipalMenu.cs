using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class PrincipalMenu : MonoBehaviour
{
    GameObject mainMenu;
    GameObject gameTypeSelection;
    GameObject whoStartMenu;
    GameObject aiDifficultySelction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenu = transform.Find("FirstMenu").gameObject;
        gameTypeSelection = transform.Find("GameTypeSelection").gameObject;
        whoStartMenu = transform.Find("PlayerBeginSelection").gameObject;
        aiDifficultySelction = transform.Find("AIDifficultySelection").gameObject;

        mainMenu.SetActive(true);
        gameTypeSelection.SetActive(false);
        whoStartMenu.SetActive(false);
        aiDifficultySelction.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGameTypeSelection()
    {
        mainMenu.SetActive(false);
        gameTypeSelection.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayPvP()
    {
        GameManager.Instance.gameType = GameManager.GameType.PvP;
        SceneManager.LoadScene("Game");
    }

    public void PlayPvAI()
    {
        GameManager.Instance.gameType = GameManager.GameType.PvAI;
        whoStartMenu.SetActive(true);
        gameTypeSelection.SetActive(false);
    }

    public void PlayAIvAI()
    {
        GameManager.Instance.gameType = GameManager.GameType.AIvAI;
        aiDifficultySelction.SetActive(true);
        gameTypeSelection.SetActive(false);
    }

    public void PlayerStart()
    {
        GameManager.Instance.isAIStarting = false;
        SceneManager.LoadScene("Game");
    }

    public void AIStart()
    {
        GameManager.Instance.isAIStarting = true;
        SceneManager.LoadScene("Game");
    }

    public void AI1Difficulty(int _index)
    {
        switch(_index)
        {
            case 0:
                GameManager.Instance.aiDifficulty[0] = AIDifficulty.Easy;
                break;
            case 1:
                GameManager.Instance.aiDifficulty[0] = AIDifficulty.Medium;
                break;
            case 2:
                GameManager.Instance.aiDifficulty[0] = AIDifficulty.Hard;
                break;
            default:
                break;
        }
    }
    public void AI2Difficulty(int _index)
    {
        switch(_index)
        {
            case 0:
                GameManager.Instance.aiDifficulty[1] = AIDifficulty.Easy;
                break;
            case 1:
                GameManager.Instance.aiDifficulty[1] = AIDifficulty.Medium;
                break;
            case 2:
                GameManager.Instance.aiDifficulty[1] = AIDifficulty.Hard;
                break;
            default:
                break;
        }
    }

    public void StartAIvsAIGame()
    {
        SceneManager.LoadScene("Game");
    }
}
