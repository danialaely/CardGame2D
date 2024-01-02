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

    public Camera mainCamera; // Reference to your main camera
    private float originalOrthographicSize;
    Vector3 originalCamPos;

    private float lastClickTime = 0f;
    private float doubleClickDelay = 0.2f; // Adjust this value based on your desired double-tap speed

    // Vector2 difference = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        UpdateCardInformation();

        player2 = GameObject.FindGameObjectsWithTag(tagToSearch);


        // Populate allDisplayCards with all instances of DisplayCard
        allDisplayCards = new List<DisplayCard>(FindObjectsOfType<DisplayCard>());
        outerBorder = this.transform.Find("OuterBorder").GetComponent<Image>();

        dice1.enabled = false;
        dice2.enabled = false;

        Discard = false;


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

    public void UpdateCardInformation()
    {
        Card card = display.Find(c => c.cardId == displayId);

        if (card != null)
        {
            nameText.text = card.cardName;
            attackText.text = card.cardAttack.ToString();
            healthText.text = card.cardHealth.ToString();
            energyText.text = card.cardEnergy.ToString();   
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

       // difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)-(Vector2)transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        int dragging = BoardSlot.GetCurrentEnergy();
        int dragging2 = BoardSlot.GetCurrentEnergyP2();

        if (transform.parent!=null && transform.parent.name == "Hand") 
        {
           if (dragging > 0) 
           {
                 //transform.position = Input.mousePosition;

                 RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(),Input.mousePosition, Camera.main,out Vector2 localPos);
                 transform.localPosition = localPos;

                //transform.position =  Camera.main.ScreenToWorldPoint(Input.mousePosition);

                //transform.position = Camera.main.WorldToScreenPoint(Input.mousePosition);

                //  transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
                //transform.position = eventData.position;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        bool isP1Turn = ButtonTurn.GetPlayerTurn();    // Debug.Log("DC P1 Turn:"+isTurn);

                if (mainCamera != null && Time.time-lastClickTime<doubleClickDelay)
                {
                    // You can adjust the target orthographic size based on your desired zoom level
                    float targetOrthographicSize = 197.71f;

                    Vector3 offset = new Vector3(0f,0f,-10f);
                    mainCamera.transform.position = this.transform.position+offset;
                    // You can also add smoothness by using Lerp or other techniques
                    float transitionSpeed = 1f;
                    mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthographicSize,  transitionSpeed);
                }
                lastClickTime = Time.time;
        if (isP1Turn)
        {
            isSelected = !isSelected;
            if (isSelected)
            {      
                outerBorder.color = Color.green;

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
                    if (otherCard != this)
                    {
                        otherCard.isSelected = false;
                        otherCard.adjacentCards.Clear();
                        otherCard.outerBorder.color = Color.black;

                    /*    foreach (GameObject ptwo in player2)
                        {
                            float dist = Vector3.Distance(ptwo.transform.position,otherCard.transform.position);

                            if (dist<165f) 
                            {
                                UnityEngine.UI.Image ptwoouterborder = ptwo.transform.Find("OuterBorder").GetComponent<Image>();
                                ptwoouterborder.color = Color.yellow;
                            }

                        }*/
                    }
                }

            }
            if (!isSelected)
            {
                outerBorder.color = Color.black;
                // DisplayCard2.dice1.enabled = false;
                // Reset the orthographic camera's size when the card is deselected
                if (mainCamera != null)
                {
                    mainCamera.orthographicSize = originalOrthographicSize;
                    mainCamera.transform.position = originalCamPos;
                }


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


            if (isSelected)
            {
                foreach (GameObject displayCardObject in player2)
                {
                    DisplayCard2 dp = displayCardObject.GetComponent<DisplayCard2>();
                    if (dp != null && dp.adjCards.Contains(gameObject))
                    {
                        outerBorder.color = Color.red;
                        Debug.Log("Player2's Card Attack:"+dp.GetCardAttack());
                        dice1.enabled = true;
                        dice2.enabled = true;
                        if (this.GetCardAttack() < dp.GetCardAttack()) 
                        {
                            Discard = true;
                        }
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

    public bool GetDiscard() 
    {
        return Discard;
    }

    public bool GetSelected() 
    {
        return isSelected;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
     //   Debug.Log("Hovering over:"+gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
