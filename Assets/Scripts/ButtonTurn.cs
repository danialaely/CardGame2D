using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonTurn : MonoBehaviour
{

    private static bool isPlayer1Turn = true;
    public TMP_Text turnText;
    private Coroutine turnCoroutine;

    public HealthBar turnBar;
    private int turnCount;
    private int turnCount2;
    // private Coroutine healthtimerCoroutine;

    private void Start()
    {
        turnCoroutine = StartCoroutine(ChangeTurn(30.0f));
        TurnStarter(isPlayer1Turn);
    }

    public void OnTurnButtonClick()
    {
        ResetTimer();
        //DisplayCard.GetTurn();

        if (isPlayer1Turn)
        {
            turnText.text = "P2 Turn";
            turnCount = 0;
            turnBar.SetTurnTime(turnCount);
            //StartCoroutine(Turnbar2(1.0f));

            turnCount2 = 30;
            turnBar.SetTurnTime2(turnCount2);
        }
        else
        {
            turnText.text = "P1 Turn";
            turnCount = 30;
            turnBar.SetTurnTime(turnCount);

            turnCount2 = 0;
            turnBar.SetTurnTime2(turnCount2);
        }

        // Toggle the turn
        isPlayer1Turn = !isPlayer1Turn;
        Debug.Log("PLAYER 1 TURN:" + isPlayer1Turn);
    }

    public static bool GetPlayerTurn()
    {
        return isPlayer1Turn;
    }

    private IEnumerator ChangeTurn(float delay)
    {
        while (true)
        {
            if (isPlayer1Turn)
            {
                turnText.text = "P1 Turn";
                turnCount = 30;
                turnBar.SetTurnTime(turnCount);

                turnCount2 = 0;
                turnBar.SetTurnTime2(turnCount2);
            }
            else
            {
                turnText.text = "P2 Turn";
                turnCount2 = 30;
                turnBar.SetTurnTime2(turnCount2);
              // StartCoroutine(Turnbar2(1.0f));

                turnCount = 0;
                turnBar.SetTurnTime(turnCount2);
            }
            Debug.Log(isPlayer1Turn);

            yield return new WaitForSeconds(delay);
            isPlayer1Turn = !isPlayer1Turn;
        }
    }

    private void ResetTimer()
    {
        StopCoroutine(turnCoroutine);

        turnCoroutine = StartCoroutine(ChangeTurn(30.0f));
    }

    private IEnumerator Turnbar(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (turnCount >= 0)
            {
                turnCount--;
                Debug.Log("TurnCount:" + turnCount);
                turnBar.SetTurnTime(turnCount);
            }
        }
    }

    private IEnumerator Turnbar2(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (turnCount2 >= 0)
            {
                turnCount2--;
                Debug.Log("TurnCount2:" + turnCount2);

                turnBar.SetTurnTime2(turnCount2);
            }

        }
    }

    private void TurnStarter(bool isP1)
    {
        
        if (isP1)
        {
         //   turnCount = 30;
          //  turnBar.SetTurnTime(turnCount);
            StartCoroutine(Turnbar(1.0f));
            StartCoroutine(Turnbar2(1.0f));
        }
        
    }

}
