using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class ButtonTurn : MonoBehaviour
{

    private static bool isPlayer1Turn = true;
    public TMP_Text turnText;
    private Coroutine turnCoroutine;

    public HealthBar turnBar;
    private int turnCount;
    private int turnCount2;
    // private Coroutine healthtimerCoroutine;

    public Camera mainCamera; // Reference to your main camera

    private float originalOrthographicSize;
    Vector3 originalCamPos;

    public Button deckP1;
    public Button deckP2;

    public GameManager gmm;
    public BoardSlot boardSlot;

    public GridLayoutGroup handGrid;
    GameObject selectedCard;

    List<Transform> availableSlots;
    Transform randomSlot;

    public CCardShuffler CShufflerP2;

    private void Start()
    {
        boardSlot = FindAnyObjectByType<BoardSlot>();
        availableSlots = boardSlot.Available();
        boardSlot.AnotherMethod();

        turnCoroutine = StartCoroutine(ChangeTurn(30.0f));
        TurnStarter(isPlayer1Turn);

        if (mainCamera == null)
        {
            // If the mainCamera reference is not set, try to find the main camera in the scene
            mainCamera = Camera.main;
        }

        // Store the original orthographic size for resetting
        if (mainCamera != null)
        {
            originalOrthographicSize = mainCamera.orthographicSize;
            originalCamPos = mainCamera.transform.position;
        }

    }

    public IEnumerator PlaceToBoard(float delayed)
    {
        yield return new WaitForSeconds(delayed);
        MoveSelectedCardToRandomSlot();
        SelectRandomCard();
        //selectedCard.transform.position = randomSlot.position;
        selectedCard.transform.SetParent(randomSlot);
        selectedCard.transform.localPosition = Vector3.zero;
        Image carddBackImage = selectedCard.transform.Find("Back").GetComponent<Image>();
        carddBackImage.enabled = false;
    }

    public void MoveSelectedCardToRandomSlot()
    {
        randomSlot = availableSlots[Random.Range(0, availableSlots.Count)];
        Debug.Log("Random Slot from BSlot:" + randomSlot);
    }

    public void SelectRandomCard()
    {
        int cardCount = handGrid.transform.childCount;

        if (cardCount > 0)
        {
            // Generate a random index within the range of cardCount
            int randomIndex = Random.Range(0, cardCount);

            // Get the selected card object
            selectedCard = handGrid.transform.GetChild(randomIndex).gameObject;
            Debug.Log("Selected Card from Hand:" + selectedCard);
            // Now you have the selected card, you can perform actions on it
            // For example, you can disable the card, remove it from the hand, etc.
        }
        else
        {
            Debug.LogWarning("No cards in the hand.");
        }
    }

    IEnumerator ChangeAIPhaseToMove(float delayed) 
    {
        yield return new WaitForSeconds(delayed);
        gmm.ChangePhase(GamePhase.Move);
    }

    IEnumerator ChangingAIPhase(float delay)
    {
        yield return new WaitForSeconds(delay);
        gmm.ChangePhase(GamePhase.Setup);
        //  SelectRandomCard();
        StartCoroutine(PlaceToBoard(2.0f));
      //  StartCoroutine(ChangeAIPhaseToMove(3.0f));
    }

    public void OnTurnButtonClick()
    {
        ResetTimer();
        
        //DisplayCard.GetTurn();

        if (isPlayer1Turn)
        {
            gmm.ChangePhase(GamePhase.Draw);
            turnText.text = "P2 Turn";  //Button 
            turnCount = 0;
            turnBar.SetTurnTime(turnCount);
            //StartCoroutine(Turnbar2(1.0f));

            turnCount2 = 30;
            turnBar.SetTurnTime2(turnCount2);

            deckP1.enabled = false;
            deckP2.enabled = true;
            boardSlot.AnotherMethod2();

            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == "AI") 
            {
                if (gmm.currentPhase == GamePhase.Draw) 
                {
                   CShufflerP2.ShuffleCards();
                   StartCoroutine(ChangingAIPhase(3.0f));
                }
            }
            //  MoveSelectedCardToRandomSlot();
        }
        else
        {
            gmm.ChangePhase(GamePhase.Draw);
            turnText.text = "P1 Turn";
            turnCount = 30;
            turnBar.SetTurnTime(turnCount);

            turnCount2 = 0;
            turnBar.SetTurnTime2(turnCount2);

            deckP1.enabled = true;
            deckP2.enabled = false;
            boardSlot.AnotherMethod();
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
                gmm.ChangePhase(GamePhase.Draw);
                turnText.text = "P1 Turn";
                turnCount = 30;
                turnBar.SetTurnTime(turnCount);

                turnCount2 = 0;
                turnBar.SetTurnTime2(turnCount2);

                deckP1.enabled = true;
                deckP2.enabled = false;
                boardSlot.AnotherMethod();
            }
            else
            {
                gmm.ChangePhase(GamePhase.Draw);
                turnText.text = "P2 Turn";
                turnCount2 = 30;
                turnBar.SetTurnTime2(turnCount2);
              // StartCoroutine(Turnbar2(1.0f));

                turnCount = 0;
                turnBar.SetTurnTime(turnCount2);

                if (mainCamera != null)
                {
                    mainCamera.orthographicSize = originalOrthographicSize;
                    mainCamera.transform.position = originalCamPos;
                }

                deckP1.enabled = false;
                deckP2.enabled = true;
                boardSlot.AnotherMethod2();
            }
           // Debug.Log("IS PLAYER TURN:"+isPlayer1Turn);

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
               // Debug.Log("TurnCount:" + turnCount);
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
               // Debug.Log("TurnCount2:" + turnCount2);

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
