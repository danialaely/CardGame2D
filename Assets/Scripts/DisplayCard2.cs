using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DisplayCard2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public List<Card> display = new List<Card>();
    public int displayId;

    public TMP_Text nameText;
    public TMP_Text attackText;
    public TMP_Text healthText;
    public TMP_Text energyText;

    public CCardShuffler shuffler;

    private CanvasGroup canvasGroup;
    private Vector3 initialPosition;

    public bool isSelected = false;

    public string tagToSearch = "Player1";
    GameObject[] player1;

    public List<GameObject> adjCards = new List<GameObject>();

    public List<DisplayCard2> allDisplayCards; // Reference to all DisplayCard instances
    UnityEngine.UI.Image outerBorder;

    public Image dice1;
    public Image dice2;

    private static bool DisCard;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        UpdateCardInformation();

        player1 = GameObject.FindGameObjectsWithTag(tagToSearch);

        // Populate allDisplayCards with all instances of DisplayCard
        allDisplayCards = new List<DisplayCard2>(FindObjectsOfType<DisplayCard2>());
        outerBorder = this.transform.Find("OuterBorder").GetComponent<Image>();
        dice1.enabled = false;
        dice2.enabled = false;

        DisCard = false;
    }

    public void UpdateCardInformation()
    {
        Card cardd = display.Find(c => c.cardId == displayId);

        if (cardd != null)
        {
            nameText.text = cardd.cardName;
            attackText.text = cardd.cardAttack.ToString();
            healthText.text = cardd.cardHealth.ToString();
            energyText.text = cardd.cardEnergy.ToString();
        }
        else
        {
            nameText.text = "Card Not Found";
            attackText.text = " ";
            healthText.text = " ";
            energyText.text = " ";
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = transform.position;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        int dragging = BoardSlot.GetCurrentEnergy();
        int dragging2 = BoardSlot.GetCurrentEnergyP2();

        if (transform.parent != null && transform.parent.name == "Hand")
        {
            if (dragging > 0)
            {
                transform.position = Input.mousePosition;
            }
        }

        if (transform.parent != null && transform.parent.name == "Hand2")
        {
            if (dragging2 > 0)
            {
                transform.position = Input.mousePosition;
            }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Card card = display.Find(c => c.cardId == displayId);

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // If the card is not dropped on a slot, return it to the initial position.
        if (transform.parent == null || transform.parent.CompareTag("Hand"))
        {
            transform.position = initialPosition;

            int cardEnergy = GetCardEnergy();

        }
    }

    public int GetCardEnergy()
    {
        Card cardd = display.Find(c => c.cardId == displayId);
        if (cardd != null)
        {
            return cardd.cardEnergy;
        }
        return 0; // Return 0 if the card is not found.
    }

    public int GetCardAttack() 
    {
        Card cardd = display.Find(c => c.cardId == displayId);
        if (cardd != null)
        {
            return cardd.cardAttack;
        }
        return 0; // Return 0 if the card is not found.
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       // isSelected = !isSelected;
        bool isP1Turn = ButtonTurn.GetPlayerTurn();
      
        if (!isP1Turn) 
        {
            isSelected = !isSelected;
            if (isSelected)
            {
                outerBorder.color = Color.green;

                foreach (GameObject p1 in player1)
                {
                    float distance = Vector3.Distance(p1.transform.position, transform.position);

                    if (distance < 165f)
                    {
                        UnityEngine.UI.Image p2outerborder = p1.transform.Find("OuterBorder").GetComponent<Image>();
                        p2outerborder.color = Color.blue;

                        adjCards.Add(p1);
                      
                    }
                }

                foreach (DisplayCard2 othercard in allDisplayCards) 
                {
                    if (othercard!=this) 
                    {
                        othercard.isSelected = false;
                        othercard.adjCards.Clear();
                        othercard.outerBorder.color = Color.yellow;
                    }
                }

            }
            if (!isSelected)
            {
                outerBorder.color = Color.yellow;

                foreach (GameObject p1 in player1)
                {
                    float distance = Vector3.Distance(p1.transform.position, transform.position);

                    if (distance < 165f)
                    {
                        UnityEngine.UI.Image p1outerborder = p1.transform.Find("OuterBorder").GetComponent<Image>();
                        p1outerborder.color = Color.black;

                        adjCards.Clear();
                    }
                }

            } 
        }

        if (isP1Turn) 
        {
            foreach (GameObject displayCardObject in player1)
            {
                DisplayCard dp = displayCardObject.GetComponent<DisplayCard>();
                if (dp != null && dp.adjacentCards.Contains(gameObject))
                {
                    foreach (DisplayCard2 lastselected in allDisplayCards)
                    {

                        if (lastselected.isSelected)
                        {
                            lastselected.isSelected = false;
                            lastselected.outerBorder.color = Color.blue;
                        }
                        else 
                        {
                            isSelected = !isSelected;
                        }
                    }
                            isSelected = !isSelected;
                }
            }

           
            if (isSelected) 
            {
                foreach (GameObject displayCardObject in player1) 
                {
                    DisplayCard dp = displayCardObject.GetComponent<DisplayCard>();
                    if (dp != null && dp.adjacentCards.Contains(gameObject)) 
                    {
                        outerBorder.color = Color.red;
                        Debug.Log("Player1 Card's Attack:"+dp.GetCardAttack());
                        dice1.enabled = true;
                        dice2.enabled = true;
                        if (this.GetCardAttack() < dp.GetCardAttack()) 
                        {
                            DisCard = true; 
                        }
                      
                    }
                }
            }
            if (!isSelected) 
            {
                foreach (GameObject displayCardObject in player1)
                {
                    DisplayCard dp = displayCardObject.GetComponent<DisplayCard>();
                    if (dp != null && dp.adjacentCards.Contains(gameObject))
                    {
                        outerBorder.color = Color.blue;   //MainCamera Blue Color: 314D79    Board Color: 292E48 
                        dice1.enabled = false;
                        dice2.enabled = false;
                    }
                }
            }
        }

        Debug.Log(isSelected);
    }

    public bool GetDiscard() 
    {
        return DisCard;
    }

}
