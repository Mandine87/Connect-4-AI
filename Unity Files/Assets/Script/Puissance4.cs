using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Puissance4 : MonoBehaviour
{
    public enum PlayerType
    {
        Human,
        AI
    }

    static PlayerType[] players;

    Node root;
    //int depth = 5;

    RenderPuissance4Plateau rendering;

    bool isGameEnded = false;

    void Start()
    {
        rendering = FindFirstObjectByType<RenderPuissance4Plateau>();

        //Create the  board (root)
        root = new Node();

        //Init the playersType
        players = new PlayerType[2];
        switch (GameManager.Instance.gameType)
        {
            case GameManager.GameType.PvP:
                players[0] = PlayerType.Human;
                players[1] = PlayerType.Human;
                break;
            case GameManager.GameType.PvAI:
                if (GameManager.Instance.isAIStarting)
                {
                    players[0] = PlayerType.AI;
                    players[1] = PlayerType.Human;
                }
                else
                {
                    players[0] = PlayerType.Human;
                    players[1] = PlayerType.AI;
                    //Set the AI dificulty to the second player place
                    GameManager.Instance.aiDifficulty[1] = GameManager.Instance.aiDifficulty[0];
                }
                break;
            case GameManager.GameType.AIvAI:
                players[0] = PlayerType.AI;
                players[1] = PlayerType.AI;
                break;
        }

        //CreateTreeMinMax(root, 1, depth, int.MinValue, int.MaxValue);
    }

    void Update()
    {
        if(!isGameEnded && !GameManager.Instance.isGamePaused)
        {
            if (!rendering.GetIsPieceDropping())
            {
                //If player1 is AI, make it play when it is it turn
                if (players[0] == PlayerType.AI && root.isPlayer1Turn)
                {
                    PlayAI(1);
                }
                //If player2 is AI, make it play when it is it turn
                else if (players[1] == PlayerType.AI && !root.isPlayer1Turn)
                {
                    PlayAI(2);
                }
            }
        }

    }

    //PlayerIndex will be 1 or 2 (it is rhe current player turn)
    //Create the tree and do the min-max at the same time to do the alpha-beta breaks
    int CreateTreeMinMax(Node node, int _playerIndex, int _depth, int alpha, int beta)
    {
        //If we go too deep in the tree, we just return the value or a player as won
        if (_depth <= 0 || node.CheckVictory() != Node.State.Empty)
        {
            node.value = node.Evaluate(_playerIndex);
            return node.Evaluate(_playerIndex);
        }

        //Maximise
        if ((node.isPlayer1Turn && _playerIndex == 1) || (!node.isPlayer1Turn && _playerIndex == 2))
        {
            int maxValue = int.MinValue;

            //We go through each column
            for (int j = 0; j < node.columnCount; j++)
            {
                int i = node.lineCount - 1;
                //Check if the column is not full
                if (node.board[0, j] == Node.State.Empty)
                {
                    while (node.board[i, j] != Node.State.Empty && i > 0)
                    {
                        i--;
                    }

                    //Create a new possible move
                    Node newNode = new Node();
                    //Copy the table (a simple = will make a copy of a pointer, but we want to copy the values) 
                    Array.Copy(node.board, newNode.board, node.lineCount * node.columnCount);
                    //Apply the state of the move played
                    if (node.isPlayer1Turn)
                    {
                        newNode.board[i, j] = Node.State.Player1;
                    }
                    else
                    {
                        newNode.board[i, j] = Node.State.Player2;
                    }

                    //Change the turn of the node
                    newNode.isPlayer1Turn = !node.isPlayer1Turn;

                    //Add the new move to the tree
                    node.children.Add(newNode);

                    //Create the childrens of the new move depending on the alpha beta cut and get the maxValue
                    maxValue = Math.Max(CreateTreeMinMax(newNode, _playerIndex, _depth - 1, alpha, beta), maxValue);
                    alpha = Math.Max(alpha, maxValue);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
            }
            //Set the maxValue to the node (because we are maximising)
            node.value = maxValue;
            return maxValue;
        }
        //Minimise
        else
        {
            int minValue = int.MaxValue;

            //We go through each column
            for (int j = 0; j < node.columnCount; j++)
            {
                int i = node.lineCount - 1;
                if (node.board[0, j] != Node.State.Empty)
                {
                    //Debug.Log("Coup pas possible, colonne déjà remplie");
                }
                else
                {
                    while (node.board[i, j] != Node.State.Empty && i > 0)
                    {
                        i--;
                    }

                    //Create a new possible move
                    Node newNode = new Node();
                    //Copy the table (a simple = will make a copy of a pointer, but we want to copy the values) 
                    Array.Copy(node.board, newNode.board, node.lineCount * node.columnCount);
                    //Apply the state of the move played
                    if (node.isPlayer1Turn)
                    {
                        newNode.board[i, j] = Node.State.Player1;
                    }
                    else
                    {
                        newNode.board[i, j] = Node.State.Player2;
                    }
                    //Change the turn of the node
                    newNode.isPlayer1Turn = !node.isPlayer1Turn;

                    //Add the new move to the tree
                    node.children.Add(newNode);

                    //Create the childrens of the new move depending on the alpha beta cut and get the minValue
                    minValue = Math.Min(CreateTreeMinMax(newNode, _playerIndex, _depth - 1, alpha, beta), minValue);
                    beta = Math.Min(beta, minValue);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
            }
            //Set the mminValue to the node (because we are minimising)
            node.value = minValue;
            return minValue;
        }
    }

    //Return the player turn if the current player is Human, if not, return Empty
    public Node.State GetPlayerTurn()
    {
        if (rendering.GetIsPieceDropping())
        {
            return Node.State.Empty;
        }

        if (players[0] == PlayerType.Human && root.isPlayer1Turn)
        {
            return Node.State.Player1;
        }
        else if (players[1] == PlayerType.Human)
        {
            return Node.State.Player2;
        }

        return Node.State.Empty;
    }

    //Play the human player turn
    public void PlayTurn(int _column)
    {
        int i = root.lineCount - 1;

        if (root.board[0, _column] != Node.State.Empty)
        {
            Debug.Log("Coup pas possible, colonne déjà remplie");
        }
        else
        {
            while (root.board[i, _column] != Node.State.Empty && i > 0)
            {
                i--;
            }
            if (root.isPlayer1Turn)
            {
                root.board[i, _column] = Node.State.Player1;
                rendering.DropJeton(_column, i, true);
            }
            else
            {
                root.board[i, _column] = Node.State.Player2;
                rendering.DropJeton(_column, i, false);
            }

            root.isPlayer1Turn = !root.isPlayer1Turn;

            //We check if someone won or if the board is full to load end game menu
            if (root.CheckVictory() != Node.State.Empty || isBoardFull())
            {
                isGameEnded = true;
                transform.Find("EndGameText").gameObject.SetActive(true);
            }
        }
    }

    //Play the AI player turn
    void PlayAI(int _playerIndex)
    {
        if (root.children.Count >= 0 && root.CheckVictory() == Node.State.Empty)
        {
            root.children.Clear();
            //The depth is chosen depeding on the ai difficulty (if easy, low depth, if hard, hight depth)
            int optiValue = CreateTreeMinMax(root, _playerIndex, 3 + (2 *(int)GameManager.Instance.aiDifficulty[_playerIndex-1]), int.MinValue, int.MaxValue);
            Debug.Log(optiValue);

            //ICI Voir pour sélectionner un coup aléatoire en fonction des coups trouvés
            Node newRoot = root.children.First(x => x.value == optiValue);

            //Search for the difference between the 2 boards to drop the piece at the right place
            for (int row = 0; row < root.lineCount; row++)
            {
                for (int column = 0; column < root.columnCount; column++)
                {
                    if (root.board[row, column] != newRoot.board[row, column])
                    {
                        rendering.DropJeton(column, row, root.isPlayer1Turn);
                    }
                }
            }
            root = newRoot;

            //We check if someone won or if the board is full to load end game menu
            if (root.CheckVictory() != Node.State.Empty || isBoardFull())
            {
                isGameEnded = true;
                transform.Find("EndGameText").gameObject.SetActive(true);
            }
        }
    }

    //Load the EndGame screen
    public void EndGame()
    {
        GameManager.Instance.winner = root.CheckVictory();
        SceneManager.LoadScene("GameOver");
    }

    //Return true if the board is full, false if not
    private bool isBoardFull()
    {
        for (int column = 0; column < root.columnCount; column++)
        {
            if (root.board[0, column] == Node.State.Empty)
            {
                return false;
            }
        }
        return true;
    }

    //Return true if the game is ended but we didn't load the EndGame screen(to view the board), false if not
    public bool GetIsGameEnded()
    {
        return isGameEnded;
    }
}

