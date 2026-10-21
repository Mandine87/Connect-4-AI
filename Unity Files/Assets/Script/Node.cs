using System.Collections.Generic;

public class Node
{
    public enum State
    {
        Empty,
        Player1,
        Player2
    }

    public int lineCount = 6;
    public int columnCount = 7;

    public bool isPlayer1Turn = true;

    public List<Node> children = new List<Node>();

    //Node value
    public int value;

    public State[,] board =
        {
            { State.Empty,State.Empty,State.Empty,State.Empty,State.Empty,State.Empty,State.Empty },
            { State.Empty,State.Empty,State.Empty,State.Empty,State.Empty,State.Empty,State.Empty },
            { State.Empty,State.Empty,State.Empty,State.Empty,State.Empty,State.Empty,State.Empty },
            { State.Empty,State.Empty,State.Empty,State.Empty,State.Empty,State.Empty,State.Empty },
            { State.Empty,State.Empty,State.Empty,State.Empty,State.Empty,State.Empty,State.Empty },
            { State.Empty,State.Empty,State.Empty,State.Empty,State.Empty,State.Empty,State.Empty }
        };

    //Check if someone won
    public State CheckVictory()
    {
        List<State> winner = CheckIsAligned(4);
        if (winner.Count > 0)
        {
            //Take only the first value because when someone aligned 4, he wons so no one else can align 4
            return winner[0];
        }
        return State.Empty;
    }

    List<State> CheckIsAligned(int alignmentCount)
    {
        //This will be the list of State for a certain count of token aligned
        List<State> alignementsWinner = new List<State>();

        //Calculate the decalage for tests depending on the aligned count we want
        int decalage = alignmentCount - 1;

        int currentAlignement = 1;

        //Test the line
        for (int row = 0; row < lineCount; row++)
        {
            for (int column = 0; column < columnCount - decalage; column++)
            {
                if (board[row, column] != State.Empty)
                {
                    currentAlignement = 1;

                    while (currentAlignement <= decalage && board[row, column] == board[row, column + currentAlignement])
                    {
                        currentAlignement++;
                    }
                    if (currentAlignement == alignmentCount)
                    {
                        alignementsWinner.Add(board[row, column]);
                    }
                }
            }
        }

        //Test the column
        for (int row = 0; row < lineCount - decalage; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {

                if (board[row, column] != State.Empty)
                {
                    currentAlignement = 1;
                    while (currentAlignement <= decalage && board[row, column] == board[row + currentAlignement, column])
                    {
                        currentAlignement++;
                    }
                    if (currentAlignement == alignmentCount)
                    {
                        alignementsWinner.Add(board[row, column]);
                    }
                }
            }
        }

        //Test the diagonal in one way
        for (int row = 0; row < lineCount - decalage; row++)
        {
            for (int column = 0; column < columnCount - decalage; column++)
            {
                if (board[row, column] != State.Empty)
                {
                    currentAlignement = 1;
                    while (currentAlignement <= decalage && board[row, column] == board[row + currentAlignement, column + currentAlignement])
                    {
                        currentAlignement++;
                    }
                    if (currentAlignement == alignmentCount)
                    {

                        alignementsWinner.Add(board[row, column]);
                    }
                }
            }
        }

        //Test the diagonal in the other way
        for (int row = decalage; row < lineCount; row++)
        {
            for (int column = 0; column < columnCount - decalage; column++)
            {
                if (board[row, column] != State.Empty)
                {
                    currentAlignement = 1;
                    while (currentAlignement <= decalage && board[row, column] == board[row - currentAlignement, column + currentAlignement])
                    {
                        currentAlignement++;
                    }
                    if (currentAlignement == alignmentCount)
                    {
                        alignementsWinner.Add(board[row, column]);
                    }
                }
            }
        }

        return alignementsWinner;
    }

    //Evaluate the board value
    public int Evaluate(int _playerIndex)
    {
        //Choose the evaluate depending on the AI difficulty
        switch (GameManager.Instance.aiDifficulty[_playerIndex - 1])
        {
            case GameManager.AIDifficulty.Easy:
                return EvaluateEasy(_playerIndex);
            case GameManager.AIDifficulty.Medium:
                return EvaluateMedium(_playerIndex);
            case GameManager.AIDifficulty.Hard:
                return EvaluateHard(_playerIndex);
            default:
                break;
        }
        return 0;
    }

    public override string ToString()
    {
        string str = "";

        for (int i = 0; i < lineCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                switch (board[i, j])
                {
                    case State.Empty:
                        str += "0";
                        break;
                    case State.Player1:
                        str += "1";
                        break;
                    case State.Player2:
                        str += "2";
                        break;
                }
            }
            str += "\n";
        }
        return str;
    }

    private int EvaluateEasy(int _playerIndex)
    {
        int value = 0;
        //Add or Substract depending on a victory condition
        switch (CheckVictory())
        {
            case State.Player1:
                if (_playerIndex == 1)
                {
                    value += 100;
                }
                else
                {
                    value += -100;
                }
                break;
            case State.Player2:
                if (_playerIndex == 2)
                {
                    value += 100;
                }
                else
                {
                    value += -100;
                }
                break;
            case State.Empty:
                value += 0;
                break;
            default:
                break;

        }

        return value;
    }

    private int EvaluateMedium(int _playerIndex)
    {
        int value = 0;
        //Add or Substract depending on a victory condition
        switch (CheckVictory())
        {
            case State.Player1:
                if (_playerIndex == 1)
                {
                    value += 100;
                }
                else
                {
                    value += -100;
                }
                break;
            case State.Player2:
                if (_playerIndex == 2)
                {
                    value += 100;
                }
                else
                {
                    value += -100;
                }
                break;
            case State.Empty:
                //If no one won, add or substract value depending on the 3 token alignements
                List<State> alignement3 = CheckIsAligned(3);
                foreach (State aligner in alignement3)
                {
                    switch (aligner)
                    {
                        case State.Player1:
                            if (_playerIndex == 1)
                            {
                                value += 25;
                            }
                            else
                            {
                                value += -25;
                            }
                            break;
                        case State.Player2:
                            if (_playerIndex == 2)
                            {
                                value += 25;
                            }
                            else
                            {
                                value += -25;
                            }
                            break;
                        case State.Empty:
                            value += 0;
                            break;
                    }
                }
                break;
            default:
                break;

        }
        return value;
    }

    private int EvaluateHard(int _playerIndex)
    {
        int value = 0;
        //The tokens in the middle have more value than the others (they can add more victory chances)
        for (int line = 0; line < lineCount; line++)
        {
            if (_playerIndex == 1)
            {
                //In the middle
                if (board[line, 3] == State.Player1)
                {
                    value += 3;
                }
                else if (board[line, 3] == State.Player2)
                {
                    value -= 3;
                }

                //In the middle left
                if (board[line, 2] == State.Player1)
                {
                    value += 1;
                }
                else if (board[line, 2] == State.Player2)
                {
                    value -= 1;
                }

                //In the middle right
                if (board[line, 4] == State.Player1)
                {
                    value += 1;
                }
                else if (board[line, 4] == State.Player2)
                {
                    value -= 1;
                }

            }
            if (_playerIndex == 2 && board[line, 3] == State.Player2)
            {
                //In the middle
                if (board[line, 3] == State.Player2)
                {
                    value += 3;
                }
                else if (board[line, 3] == State.Player1)
                {
                    value -= 3;
                }

                //In the middle left
                if (board[line, 2] == State.Player2)
                {
                    value += 1;
                }
                else if (board[line, 2] == State.Player1)
                {
                    value -= 1;
                }

                //In the middle right
                if (board[line, 4] == State.Player2)
                {
                    value += 1;
                }
                else if (board[line, 4] == State.Player1)
                {
                    value -= 1;
                }

            }
        }

        //Add or Substract depending on a victory condition
        switch (CheckVictory())
        {
            case State.Player1:
                if (_playerIndex == 1)
                {
                    value += 100;
                }
                else
                {
                    value += -100;
                }
                break;
            case State.Player2:
                if (_playerIndex == 2)
                {
                    value += 100;
                }
                else
                {
                    value += -100;
                }
                break;
            case State.Empty:
                //If no one won, add or substract value depending on the 3 token alignements
                List<State> alignement3 = CheckIsAligned(3);
                foreach (State aligner in alignement3)
                {
                    switch (aligner)
                    {
                        case State.Player1:
                            if (_playerIndex == 1)
                            {
                                value += 25;
                            }
                            else
                            {
                                value += -25;
                            }
                            break;
                        case State.Player2:
                            if (_playerIndex == 2)
                            {
                                value += 25;
                            }
                            else
                            {
                                value += -25;
                            }
                            break;
                        case State.Empty:
                            value += 0;
                            break;
                    }
                }
                break;
            default:
                break;

        }
        return value;
    }
}
