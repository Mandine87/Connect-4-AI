using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameType
    {
        PvP,
        PvAI,
        AIvAI
    }

    public enum AIDifficulty
    {
        Easy, 
        Medium,
        Hard
    }

    public GameType gameType = GameType.PvP;

    public bool isAIStarting = false;

    public Node.State winner;

    public bool isGamePaused = false;

    static public GameManager Instance;

    public AIDifficulty[] aiDifficulty;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            aiDifficulty = new AIDifficulty[2];
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
