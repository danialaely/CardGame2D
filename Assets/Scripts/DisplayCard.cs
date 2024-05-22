using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class DisplayCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public List<Card> display = new List<Card>();
    public int displayId;

    public TMP_Text nameText;
    public TMP_Text attackText;
    public TMP_Text healthText;
    public TMP_Text energyText;
  //  public TMP_Text defenseText;
    public Image crdImage;

    public CardShuffler shuffler;

    private CanvasGroup canvasGroup;
    private Vector3 initialPosition;

    public bool isSelected = false;

    public string tagToSearch = "Player2";
    GameObject[] player2;

    public List<GameObject> adjacentCards = new List<GameObject>();

    public List<DisplayCard> allDisplayCards; // Reference to all DisplayCard instances
    UnityEngine.UI.Image outerBorder;

    public Image dice1;
    public Image dice2;

    public static bool Discard;

    public static int P1Power;
    public static int P2Power;

    public List<BoardSlot> BoSlots; //Reference to all BoardSlots

    public GameManager gm;

    public bool canMove;


    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        UpdateCardInformation();

        player2 = GameObject.FindGameObjectsWithTag(tagToSearch);


        // Populate allDisplayCards with all instances of DisplayCard
        allDisplayCards = new List<DisplayCard>(FindObjectsOfType<DisplayCard>());
        outerBorder = this.transform.Find("OuterBorder").GetComponent<Image>();

        BoSlots = new List<BoardSlot>(FindObjectsOfType<BoardSlot>());

        dice1.enabled = false;
        dice2.enabled = false;

        Discard = false;
        canMove = true;
    }

    public IEnumerator CanMoveNow(float delay) 
    {
        yield return new WaitForSeconds(delay);
        canMove = true;
    }

    public void UpdateCardInformation()
    {
        Card card = display.Find(c => c.cardId == displayId);

        if (card != null)
        {
            nameText.text = card.cardName;
            attackText.text = card.cardAttack.ToString();
            healthText.text = card.cardHealth.ToString();
            energyText.text = card.cardEnergy.ToString(); 
         //   defenseText.text = card.cardDefence.ToString();
           crdImage.sprite = card.cardImage;
        }
        else
        {
            nameText.text = "Card Not Found";
            attackText.text = " ";
            healthText.text = " ";
            energyText.text = " ";
          //  defenseText.text = " ";
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
                if (gm.currentPhase == GamePhase.Play && this.transform.parent.name == "Hand")
                {
                    if ((i + 13 < bslot.transform.parent.childCount && bslot.transform.parent.GetChild(i + 13).childCount > 0 && bslot.transform.parent.GetChild(i + 13).GetChild(0).name == "SHCardP1") ||
                    (i + 14 < bslot.transform.parent.childCount && bslot.transform.parent.GetChild(i + 14).childCount > 0 && bslot.transform.parent.GetChild(i + 14).GetChild(0).name == "SHCardP1") ||
                    (i + 15 < bslot.transform.parent.childCount && bslot.transform.parent.GetChild(i + 15).childCount > 0 && bslot.transform.parent.GetChild(i + 15).GetChild(0).name == "SHCardP1") ||
                    (i + 1 < bslot.transform.parent.childCount && bslot.transform.parent.GetChild(i + 1).childCount > 0 && bslot.transform.parent.GetChild(i + 1).GetChild(0).name == "SHCardP1") ||
                    (i - 13 >= 0 && bslot.transform.parent.GetChild(i - 13).childCount > 0 && bslot.transform.parent.GetChild(i - 13).GetChild(0).name == "SHCardP1") ||
                    (i - 14 >= 0 && bslot.transform.parent.GetChild(i - 14).childCount > 0 && bslot.transform.parent.GetChild(i - 14).GetChild(0).name == "SHCardP1") ||
                    (i - 15 >= 0 && bslot.transform.parent.GetChild(i - 15).childCount > 0 && bslot.transform.parent.GetChild(i - 15).GetChild(0).name == "SHCardP1") ||
                    (i - 1 >= 0 && bslot.transform.parent.GetChild(i - 1).childCount > 0 && bslot.transform.parent.GetChild(i - 1).GetChild(0).name == "SHCardP1"))
                    {
                        Transform slot = bslot.transform.parent.GetChild(i);
                        slot.GetComponent<Image>().color = Color.green;
                    }
                }

                if (gm.currentPhase == GamePhase.Move && this.transform.parent.tag == "BSlot")
                {    //(i >= 84) ||
                    if ((i + 13 < bslot.transform.parent.childCount && bslot.transform.parent.GetChild(i + 13).childCount > 0 && bslot.transform.parent.GetChild(i + 13).GetChild(0).tag == "Player1") ||
                    (i + 14 < bslot.transform.parent.childCount && bslot.transform.parent.GetChild(i + 14).childCount > 0 && bslot.transform.parent.GetChild(i + 14).GetChild(0).tag == "Player1") ||
                    (i + 15 < bslot.transform.parent.childCount && bslot.transform.parent.GetChild(i + 15).childCount > 0 && bslot.transform.parent.GetChild(i + 15).GetChild(0).tag == "Player1") ||
                    (i + 1 < bslot.transform.parent.childCount && bslot.transform.parent.GetChild(i + 1).childCount > 0 && bslot.transform.parent.GetChild(i + 1).GetChild(0).tag == "Player1") ||
                    (i - 13 >= 0 && bslot.transform.parent.GetChild(i - 13).childCount > 0 && bslot.transform.parent.GetChild(i - 13).GetChild(0).tag == "Player1") ||
                    (i - 14 >= 0 && bslot.transform.parent.GetChild(i - 14).childCount > 0 && bslot.transform.parent.GetChild(i - 14).GetChild(0).tag == "Player1") ||
                    (i - 15 >= 0 && bslot.transform.parent.GetChild(i - 15).childCount > 0 && bslot.transform.parent.GetChild(i - 15).GetChild(0).tag == "Player1") ||
                    (i - 1 >= 0 && bslot.transform.parent.GetChild(i - 1).childCount > 0 && bslot.transform.parent.GetChild(i - 1).GetChild(0).tag == "Player1"))
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
        if (transform.parent!=null && transform.parent.name == "Hand"  && isP1Turn && gm.currentPhase == GamePhase.Play) 
        {
           if (dragging >= 0) 
           {
                 //transform.position = Input.mousePosition;

                 RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(),Input.mousePosition, Camera.main,out Vector2 localPos);
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
        if (gm.currentPhase == GamePhase.Play  || gm.currentPhase == GamePhase.Draw) 
        {
            if (transform.parent == null || transform.parent.CompareTag("Hand") )
            {
                transform.position = initialPosition;
                int cardEnergy = GetCardEnergy();
            }
        }

    }

    public int GetCardEnergy()
    {
        Card card = display.Find(c => c.cardId == displayId);
        if (card != null)
        {
            return card.cardEnergy;
        }
        return 0; // Return 0 if the card is not found.
    }

    public int GetCardAttack() 
    {
        Card card = display.Find(c => c.cardId == displayId);
        if (card != null)
        {
            return card.cardAttack;
        }
        return 0; // Return 0 if the card is not found.
    }

    public int GetCardHealth()
    {
        Card card = display.Find(c => c.cardId == displayId);
        if (card != null)
        {
            return card.cardHealth;
        }
        return 0; // Return 0 if the card is not found.
    }

    public void OnPtcClk() 
    {
        bool isP1Turn = ButtonTurn.GetPlayerTurn();    // Debug.Log("DC P1 Turn:"+isTurn);

        bool isZoom = Zoom.GetBool();
        if (isZoom)
        {
            //  Vector3 offt1 = new Vector3(-400f, 0, 0);
            //  dice1.transform.position = this.transform.position + offt1;
            //  dice2.transform.position = this.transform.position - offt1;
        }

        if (isP1Turn)
        {

            isSelected = !isSelected;
            if (isSelected)
            {
                outerBorder.color = Color.white;

                foreach (GameObject p2 in player2)
                {
                    float distance = Vector3.Distance(p2.transform.position, transform.position);

                    if (distance < 210f)
                    {
                        UnityEngine.UI.Image p2outerborder = p2.transform.Find("OuterBorder").GetComponent<Image>();
                        p2outerborder.color = Color.blue;

                        adjacentCards.Add(p2);

                    }
                }

                // Unselect other DisplayCard instances
                foreach (DisplayCard otherCard in allDisplayCards)
                {
                    if (otherCard != this && otherCard.isSelected)
                    {
                        //otherCard.isSelected = false;
                        otherCard.OnPtcClk();
                        otherCard.adjacentCards.Clear();
                        otherCard.outerBorder.color = Color.black;

                    }
                }

            }
            if (!isSelected)
            {
                outerBorder.color = Color.black;
                // DisplayCard2.dice1.enabled = false;
                // Reset the orthographic camera's size when the card is deselected


                foreach (GameObject p2 in player2)
                {
                    float distance = Vector3.Distance(p2.transform.position, transform.position);

                    if (distance < 210f)
                    {
                        UnityEngine.UI.Image p2outerborder = p2.transform.Find("OuterBorder").GetComponent<Image>();
                        p2outerborder.color = Color.yellow; //FFFF00
                        adjacentCards.Clear();
                    }
                }
            }
        }
        if (!isP1Turn)
        {
            foreach (GameObject displayCardObject in player2)
            {
                DisplayCard2 dp = displayCardObject.GetComponent<DisplayCard2>();
                if (dp != null && dp.adjCards.Contains(gameObject))
                {
                    foreach (DisplayCard lastselected in allDisplayCards)
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


            if (isSelected)  //(3):ATTACKING PHASE
            {
                foreach (GameObject displayCardObject in player2)
                {
                    DisplayCard2 dp = displayCardObject.GetComponent<DisplayCard2>();
                    if (dp != null && dp.adjCards.Contains(gameObject) && BoardSlot.GetCurrentEnergyP2() >= 2 && gm.currentPhase == GamePhase.Attack)
                    {
                        outerBorder.color = Color.red;
                        Debug.Log("Player2's Card Attack:" + dp.GetCardAttack());
                        dice1.enabled = true;
                        dice2.enabled = true;

                        shuffler.AttackSound();

                        P1Power = this.GetCardHealth();
                        P2Power = dp.GetCardAttack();
                        /* if (this.GetCardAttack() < dp.GetCardAttack()) 
                         {
                             Discard = true;
                         }*/
                    }
                }
            }
            if (!isSelected)
            {
                foreach (GameObject displayCardObject in player2)
                {
                    DisplayCard2 dp = displayCardObject.GetComponent<DisplayCard2>();
                    if (dp != null && dp.adjCards.Contains(gameObject))
                    {
                        outerBorder.color = Color.blue;
                        dice1.enabled = false;
                        dice2.enabled = false;
                    }
                }
            }
        }

        Debug.Log(isSelected);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPtcClk();
    }

    public bool GetDiscard() 
    {
        return Discard;
    }

    public bool GetSelected() 
    {
        return isSelected;
    }

    public int Getp1Power() 
    {
        return P1Power;
    }

    public int Getp2Power() 
    {
        return P2Power;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
     //   Debug.Log("Hovering over:"+gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
