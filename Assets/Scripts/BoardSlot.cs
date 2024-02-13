using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardSlot : MonoBehaviour, IDropHandler
{
    public TMP_Text energyText;
    public TMP_Text energyTextP2;
    // public GameObject hand;

    // Make currentEnergy static so that it's shared among all instances.
    private static int currentEnergy = 0;
    private static int currentEnergyP2 = 0;

    public HealthBar healthBar;

    public GameManager gmmm;
    // End Turn Button to move between Turns

    public AudioSource src;
    public AudioClip cardPlacementClip;

    public Color highlightColor;
    private Color originalColor;

    public void OnDrop(PointerEventData eventData)    
    {
        DisplayCard card = eventData.pointerDrag.GetComponent<DisplayCard>();
        DisplayCard2 cardd = eventData.pointerDrag.GetComponent<DisplayCard2>();

        //  if (card.transform.parent.name == "Hand"){ }
        bool isP1Turn = ButtonTurn.GetPlayerTurn();

       // originalColor = GetComponent<Image>().color;

        if (card != null && transform.childCount == 0)  //(2): CARD PLACEMENT PHASE
        {
            int cardEnergy = card.GetCardEnergy();

            if (int.TryParse(energyText.text, out currentEnergy))
            {
                if (currentEnergy >= cardEnergy && isP1Turn)
                {
                    int rowIndex = transform.GetSiblingIndex();
                    int maxRowIndex = 97;
                    
                    //  int columnIndex = transform.parent.GetSiblingIndex();
                    if (card.transform.parent.name == "Hand" && gmmm.currentPhase == GamePhase.Draw) 
                    {
                        gmmm.ErrorSound();
                    }

                    if (card.transform.parent.name == "Hand" && gmmm.currentPhase== GamePhase.Setup) 
                    { 
                    if ((rowIndex >= 84 && rowIndex < maxRowIndex) || 
                        (transform.parent.GetChild(rowIndex - 1).childCount > 0 && transform.parent.GetChild(rowIndex - 1).GetChild(0).tag == "Player1") || 
                        (transform.parent.GetChild(rowIndex - 13).childCount > 0 && transform.parent.GetChild(rowIndex - 13).GetChild(0).tag == "Player1") || 
                        (transform.parent.GetChild(rowIndex - 14).childCount > 0 && transform.parent.GetChild(rowIndex - 14).GetChild(0).tag == "Player1") ||
                        (transform.parent.GetChild(rowIndex - 15).childCount > 0 && transform.parent.GetChild(rowIndex - 15).GetChild(0).tag == "Player1") ||
                        (transform.parent.GetChild(rowIndex + 1).childCount > 0 && transform.parent.GetChild(rowIndex + 1).GetChild(0).tag == "Player1") || 
                        (transform.parent.GetChild(rowIndex + 13).childCount > 0 && transform.parent.GetChild(rowIndex + 13).GetChild(0).tag == "Player1")||
                        (transform.parent.GetChild(rowIndex + 14).childCount > 0 && transform.parent.GetChild(rowIndex + 14).GetChild(0).tag == "Player1") ||
                        (transform.parent.GetChild(rowIndex + 15).childCount > 0 && transform.parent.GetChild(rowIndex + 15).GetChild(0).tag == "Player1"))
                    {                                                                 // card.transform.parent.name == "Hand"
                        currentEnergy -= cardEnergy;
                    energyText.text = currentEnergy.ToString();
                    healthBar.SetHealth(currentEnergy); //
                    if (currentEnergy == -1)
                    {
                        energyText.text = "0";
                    }
                    card.transform.SetParent(transform);
                    card.transform.localPosition = Vector3.zero;
                    card.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            GetPlacementSound();
                            //GetComponent<Image>().color = highlightColor;
                          //  HighlightValidSlots();
                        }
                    }

                    if (card.transform.parent.name == "Hand" && gmmm.currentPhase == GamePhase.Move)
                    {
                        gmmm.ErrorSound();
                    }

                    if (card.transform.parent.tag == "BSlot" && gmmm.currentPhase == GamePhase.Draw) 
                    {
                        gmmm.ErrorSound();
                    }

                    if (card.transform.parent.tag == "BSlot" && gmmm.currentPhase == GamePhase.Move)
                    {
                        if ((rowIndex >= 84 && rowIndex < maxRowIndex) ||
                            (transform.parent.GetChild(rowIndex - 1).childCount > 0 && transform.parent.GetChild(rowIndex - 1).GetChild(0).tag == "Player1") ||
                            (transform.parent.GetChild(rowIndex - 13).childCount > 0 && transform.parent.GetChild(rowIndex - 13).GetChild(0).tag == "Player1") ||
                            (transform.parent.GetChild(rowIndex - 14).childCount > 0 && transform.parent.GetChild(rowIndex - 14).GetChild(0).tag == "Player1") ||
                            (transform.parent.GetChild(rowIndex - 15).childCount > 0 && transform.parent.GetChild(rowIndex - 15).GetChild(0).tag == "Player1") ||
                            (transform.parent.GetChild(rowIndex + 1).childCount > 0 && transform.parent.GetChild(rowIndex + 1).GetChild(0).tag == "Player1") ||
                            (transform.parent.GetChild(rowIndex + 13).childCount > 0 && transform.parent.GetChild(rowIndex + 13).GetChild(0).tag == "Player1") ||
                            (transform.parent.GetChild(rowIndex + 14).childCount > 0 && transform.parent.GetChild(rowIndex + 14).GetChild(0).tag == "Player1") ||
                            (transform.parent.GetChild(rowIndex + 15).childCount > 0 && transform.parent.GetChild(rowIndex + 15).GetChild(0).tag == "Player1"))
                        {                                                                 // card.transform.parent.name == "Hand"
                            currentEnergy -= cardEnergy;
                            energyText.text = currentEnergy.ToString();
                            healthBar.SetHealth(currentEnergy); //
                            if (currentEnergy == -1)
                            {
                                energyText.text = "0";
                            }
                            card.transform.SetParent(transform);
                            card.transform.localPosition = Vector3.zero;
                            card.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            GetPlacementSound();
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Invalid energyText value: " + energyText.text);
                // Handle the error as needed.
            }

            // Snap the card to the slot.

        }

        //if (card.transform.parent.name == "Hand2"){ }
       else if (cardd.gameObject.tag == "Player2")   //(2):CARD PLACEMENT PHASE
        { 
        if (cardd != null && transform.childCount == 0)
        {
            int carddEnergy = cardd.GetCardEnergy();

            if (int.TryParse(energyTextP2.text, out currentEnergyP2))
            {
                if (currentEnergyP2 >= carddEnergy && !isP1Turn)
                {
                        int rowIndex = transform.GetSiblingIndex();
                        int maxRowIndex = 14;

                        if (cardd.transform.parent.name == "Hand2" && gmmm.currentPhase == GamePhase.Draw)
                        {
                            gmmm.ErrorSound();
                        }

                        if (cardd.transform.parent.name == "Hand2" && gmmm.currentPhase == GamePhase.Move)
                        {
                            gmmm.ErrorSound();
                        }

                        if (cardd.transform.parent.tag == "BSlot" && gmmm.currentPhase == GamePhase.Draw)
                        {
                            gmmm.ErrorSound();
                        }

                        if (cardd.transform.parent.name == "Hand2" && gmmm.currentPhase == GamePhase.Setup) 
                        {
                        if ((rowIndex >= 0 && rowIndex < maxRowIndex) || 
                        (transform.parent.GetChild(rowIndex - 1).childCount > 0 && transform.parent.GetChild(rowIndex - 1).GetChild(0).tag=="Player2") ||
                        (transform.parent.GetChild(rowIndex - 13).childCount > 0 && transform.parent.GetChild(rowIndex - 13).GetChild(0).tag == "Player2") || 
                        (transform.parent.GetChild(rowIndex - 14).childCount > 0 && transform.parent.GetChild(rowIndex - 14).GetChild(0).tag == "Player2") ||
                        (transform.parent.GetChild(rowIndex - 15).childCount > 0 && transform.parent.GetChild(rowIndex - 15).GetChild(0).tag == "Player2") ||
                        (transform.parent.GetChild(rowIndex + 1).childCount > 0 && transform.parent.GetChild(rowIndex + 1).GetChild(0).tag == "Player2") || 
                        (transform.parent.GetChild(rowIndex + 13).childCount > 0 && transform.parent.GetChild(rowIndex + 13).GetChild(0).tag == "Player2")|| 
                        (transform.parent.GetChild(rowIndex + 14).childCount > 0 && transform.parent.GetChild(rowIndex + 14).GetChild(0).tag == "Player2") || 
                        (transform.parent.GetChild(rowIndex + 15).childCount > 0 && transform.parent.GetChild(rowIndex + 15).GetChild(0).tag == "Player2") )
                        { 

                        currentEnergyP2 -= carddEnergy;
                    energyTextP2.text = currentEnergyP2.ToString();
                    healthBar.SetHealth2(currentEnergyP2); //
                    if (currentEnergyP2 == -1)
                    {
                        energyTextP2.text = "0";
                    }
                    cardd.transform.SetParent(transform);
                    cardd.transform.localPosition = Vector3.zero;
                    cardd.GetComponent<CanvasGroup>().blocksRaycasts = true;

                    Image carddBackImage = cardd.transform.Find("Back").GetComponent<Image>();
                    carddBackImage.enabled = false;
                                GetPlacementSound();
                                //  Debug.Log("Child:"+ transform.parent.GetChild(rowIndex - 1).GetChild(0));
                            }
                        }

                        if (cardd.transform.parent.tag == "BSlot" && gmmm.currentPhase == GamePhase.Move)
                        {
                            if ((rowIndex >= 0 && rowIndex < maxRowIndex) ||
                            (transform.parent.GetChild(rowIndex - 1).childCount > 0 && transform.parent.GetChild(rowIndex - 1).GetChild(0).tag == "Player2") ||
                            (transform.parent.GetChild(rowIndex - 13).childCount > 0 && transform.parent.GetChild(rowIndex - 13).GetChild(0).tag == "Player2") ||
                            (transform.parent.GetChild(rowIndex - 14).childCount > 0 && transform.parent.GetChild(rowIndex - 14).GetChild(0).tag == "Player2") ||
                            (transform.parent.GetChild(rowIndex - 15).childCount > 0 && transform.parent.GetChild(rowIndex - 15).GetChild(0).tag == "Player2") ||
                            (transform.parent.GetChild(rowIndex + 1).childCount > 0 && transform.parent.GetChild(rowIndex + 1).GetChild(0).tag == "Player2") ||
                            (transform.parent.GetChild(rowIndex + 13).childCount > 0 && transform.parent.GetChild(rowIndex + 13).GetChild(0).tag == "Player2") ||
                            (transform.parent.GetChild(rowIndex + 14).childCount > 0 && transform.parent.GetChild(rowIndex + 14).GetChild(0).tag == "Player2") ||
                            (transform.parent.GetChild(rowIndex + 15).childCount > 0 && transform.parent.GetChild(rowIndex + 15).GetChild(0).tag == "Player2"))
                            {

                                currentEnergyP2 -= carddEnergy;
                                energyTextP2.text = currentEnergyP2.ToString();
                                healthBar.SetHealth2(currentEnergyP2); //
                                if (currentEnergyP2 == -1)
                                {
                                    energyTextP2.text = "0";
                                }
                                cardd.transform.SetParent(transform);
                                cardd.transform.localPosition = Vector3.zero;
                                cardd.GetComponent<CanvasGroup>().blocksRaycasts = true;

                                Image carddBackImage = cardd.transform.Find("Back").GetComponent<Image>();
                                carddBackImage.enabled = false;

                                GetPlacementSound();
                                //  Debug.Log("Child:"+ transform.parent.GetChild(rowIndex - 1).GetChild(0));
                            }
                        }

                    }
            }
            else
            {
                Debug.LogError("Invalid energyText value: " + energyTextP2.text);
                // Handle the error as needed.
            }

            // Snap the card to the slot.

        } }

       // GetComponent<Image>().color = originalColor;
    }

    

    public void AnotherMethod()  // (1):CARD DRAW PHASE
    {  
        int value = currentEnergy;
        // Debug.Log("CE: " + value);
        if (value >= 0) 
        {
        value += 8;
        energyText.text = value.ToString();
            healthBar.SetHealth(value);
        }
        if (value == -1) 
        {
            value += 9;
            energyText.text = value.ToString();
            healthBar.SetHealth(value);
        }
       
    }

    public void AnotherMethod2()  // (1):CARD DRAW PHASE
    {
        int value = currentEnergyP2;
        // Debug.Log("CE: " + value);
        if (value >= 0)
        {
            value += 8;
            energyTextP2.text = value.ToString();
            healthBar.SetHealth2(value);
        }
        if (value == -1)
        {
            value += 9;
            energyTextP2.text = value.ToString();
            healthBar.SetHealth2(value);
        }
    }

    public static int GetCurrentEnergy()
    {
        return currentEnergy;
    }

    public static int GetCurrentEnergyP2() 
    {
        return currentEnergyP2;
    }

    public void GetPlacementSound() 
    {
        src.clip = cardPlacementClip;
        src.Play();
    }

}
