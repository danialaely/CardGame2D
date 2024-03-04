using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class DisplayCard2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public List<Card> display = new List<Card>();
    public int displayId;

    public TMP_Text nameText;
    public TMP_Text attackText;
    public TMP_Text healthText;
    public TMP_Text energyText;
    public Image crdImage;

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

    public List<BoardSlot> BoSlots; //Reference to all BoardSlots

    public GameManager gm;

    public static int P1Power;
    public static int P2Power;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        UpdateCardInformation();

        player1 = GameObject.FindGameObjectsWithTag(tagToSearch);

        // Populate allDisplayCards with all instances of DisplayCard
        allDisplayCards = new List<DisplayCard2>(FindObjectsOfType<DisplayCard2>());
        outerBorder = this.transform.Find("OuterBorder").GetComponent<Image>();

        BoSlots = new List<BoardSlot>(FindObjectsOfType<BoardSlot>());

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
            crdImage.sprite = cardd.cardImage;
        }
        else
        {
            nameText.text = "Card Not Found";
            attackText.text = " ";
            healthText.text = " ";
            energyText.text = " ";
            crdImage.sprite = null;
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = transform.position;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        foreach (BoardSlot bslot in BoSlots)
        {
            for (int i = 0; i < bslot.transform.parent.childCount; i++)
            {
                if (gm.currentPhase == GamePhase.Setup && this.transform.parent.name == "Hand2") 
                {
                    if (i <= 13)
                    {
                        Transform slot = bslot.transform.parent.GetChild(i);
                        slot.GetComponent<Image>().color = Color.green;
                    }
                }

                if (gm.currentPhase == GamePhase.Move && this.transform.parent.tag == "BSlot") 
                { 
                    if ((i <= 13) ||
                    (i + 13 < bslot.transform.parent.childCount && bslot.transform.parent.GetChild(i + 13).childCount > 0 && bslot.transform.parent.GetChild(i + 13).GetChild(0).tag == "Player2") ||
                    (i + 14 < bslot.transform.parent.childCount && bslot.transform.parent.GetChild(i + 14).childCount > 0 && bslot.transform.parent.GetChild(i + 14).GetChild(0).tag == "Player2") ||
                    (i + 15 < bslot.transform.parent.childCount && bslot.transform.parent.GetChild(i + 15).childCount > 0 && bslot.transform.parent.GetChild(i + 15).GetChild(0).tag == "Player2") ||
                    (i + 1 < bslot.transform.parent.childCount && bslot.transform.parent.GetChild(i + 1).childCount > 0 && bslot.transform.parent.GetChild(i + 1).GetChild(0).tag == "Player2") ||
                    (i - 13 >= 0 && bslot.transform.parent.GetChild(i - 13).childCount > 0 && bslot.transform.parent.GetChild(i - 13).GetChild(0).tag == "Player2") ||
                    (i - 14 >= 0 && bslot.transform.parent.GetChild(i - 14).childCount > 0 && bslot.transform.parent.GetChild(i - 14).GetChild(0).tag == "Player2") ||
                    (i - 15 >= 0 && bslot.transform.parent.GetChild(i - 15).childCount > 0 && bslot.transform.parent.GetChild(i - 15).GetChild(0).tag == "Player2") ||
                    (i - 1 >= 0 && bslot.transform.parent.GetChild(i - 1).childCount > 0 && bslot.transform.parent.GetChild(i - 1).GetChild(0).tag == "Player2"))
                    {
                    Transform slot = bslot.transform.parent.GetChild(i);
                    slot.GetComponent<Image>().color = Color.green;
                    }
                }
            }
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        int dragging = BoardSlot.GetCurrentEnergy();
        int dragging2 = BoardSlot.GetCurrentEnergyP2();
        bool isP1Turn = ButtonTurn.GetPlayerTurn();

        if (transform.parent != null && transform.parent.name == "Hand")
        {
            if (dragging > 0)
            {
                transform.position = Input.mousePosition;
            }
        }

        if (transform.parent != null && transform.parent.name == "Hand2" && !isP1Turn)
        {
            if (dragging2 > 0)
            {
                // transform.position = Input.mousePosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out Vector2 localPos);
                transform.localPosition = localPos;
            }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Card card = display.Find(c => c.cardId == displayId);

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        foreach (BoardSlot bslot in BoSlots)
        {
            for (int i = 0; i < bslot.transform.parent.childCount; i++)
            {
                Transform slot = bslot.transform.parent.GetChild(i);
                slot.GetComponent<Image>().color = Color.white;
            }
        }

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

        bool isZoom = Zoom.GetBool();
        if (isZoom)
        {
            Vector3 offt1 = new Vector3(-400f, 0, 0);
            dice1.transform.position = this.transform.position + offt1;
            dice2.transform.position = this.transform.position - offt1;
        }

        if (!isP1Turn) 
        {
            isSelected = !isSelected;
            if (isSelected)
            {
                outerBorder.color = Color.green;

                foreach (GameObject p1 in player1)
                {
                    float distance = Vector3.Distance(p1.transform.position, transform.position);

                    if (distance < 210f)
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

                    if (distance < 210f)
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

           
            if (isSelected) //(3):ATTACKING PHASE
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

                        shuffler.AttackSound();

                        P1Power = dp.GetCardAttack();
                        P2Power = this.GetCardAttack();
                      /*  if (this.GetCardAttack() < dp.GetCardAttack()) 
                        {
                            //shuffler.DiscardSound();
                            DisCard = true; 
                        }*/
                      
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

    public int GetP1Power() 
    {
        return P1Power;
    }

    public int GetP2Power() 
    {
        return P2Power;
    }

}
