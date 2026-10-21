using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    InputAction pressRight;
    InputAction pressLeft;
    InputAction pressEnter;
    Puissance4 gameScript;

    int[] cursorPos = { 0, 0 };

    RenderPuissance4Plateau rendering;

    void Start()
    {
        pressRight = InputSystem.actions.FindAction("Right");
        pressLeft = InputSystem.actions.FindAction("Left");
        pressEnter = InputSystem.actions.FindAction("Enter");

        gameScript = FindFirstObjectByType<Puissance4>();

        rendering = FindFirstObjectByType<RenderPuissance4Plateau>();
    }

    void Update()
    {
        //If the game is not paused
        if(!GameManager.Instance.isGamePaused)
        {
            //Load the endGame (if we have finished the game, the computer ask to press enter before loading the endScreen, to view the board one last time
            if (gameScript.GetIsGameEnded())
            {
                if (pressEnter.WasPerformedThisFrame())
                {
                    gameScript.EndGame();
                }
            }
            else
            {
                //Move the token to the right
                if (pressRight.WasPerformedThisFrame())
                {
                    Node.State playerTurn = gameScript.GetPlayerTurn();
                    if (playerTurn == Node.State.Player1)
                    {
                        if (cursorPos[0] < 7 - 1)
                        {
                            cursorPos[0]++;
                            rendering.UpdateBoard(cursorPos[0]);
                        }
                    }
                    else if (playerTurn == Node.State.Player2)
                    {
                        if (cursorPos[1] < 7 - 1)
                        {
                            cursorPos[1]++;
                            rendering.UpdateBoard(cursorPos[1]);
                        }
                    }
                }

                //Move the token to the left
                if (pressLeft.WasPerformedThisFrame())
                {
                    Node.State playerTurn = gameScript.GetPlayerTurn();
                    if (playerTurn == Node.State.Player1)
                    {
                        if (cursorPos[0] > 0)
                        {
                            cursorPos[0]--;
                            rendering.UpdateBoard(cursorPos[0]);
                        }
                    }
                    else if (playerTurn == Node.State.Player2)
                    {
                        if (cursorPos[1] > 0)
                        {
                            cursorPos[1]--;
                            rendering.UpdateBoard(cursorPos[1]);
                        }
                    }
                }

                //Drop the token
                if (pressEnter.WasPerformedThisFrame())
                {
                    Node.State playerTurn = gameScript.GetPlayerTurn();
                    if (playerTurn == Node.State.Player1)
                    {
                        gameScript.PlayTurn(cursorPos[0]);

                        cursorPos[0] = 0;
                    }
                    else if (playerTurn == Node.State.Player2)
                    {
                        gameScript.PlayTurn(cursorPos[1]);
                        cursorPos[1] = 0;
                    }
                }
            }
        }
    }
}
