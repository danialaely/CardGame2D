using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public class ShP2Card : MonoBehaviour , IPointerClickHandler
{

    public TMP_Text defenseText;
    public TMP_Text healthText;
    public Image outerBorder;

    private int defense = 5;
    public  int health = 100;

    public bool isSelected = false;

    public string tagToSearch = "Player1";
    GameObject[] player1;

    public Image dice1;
    public Image dice2;

    public static int P1Power;
    public static int P2Power;

    public int SHealth;

    public GameManager gm;

    public CCardShuffler shuffler;


    // Start is called before the first frame update
    void Start()
    {
        defenseText.text = defense.ToString();
        healthText.text = health.ToString();

        player1 = GameObject.FindGameObjectsWithTag(tagToSearch);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetCardHealth()
    {
        return health;
    }

    public int GetCardDefense() 
    {
        return defense;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool isP1Turn = ButtonTurn.GetPlayerTurn();
        if (isP1Turn) 
        {
            isSelected = !isSelected;
            if (isSelected) 
            {
                foreach (GameObject displayCardObject in player1)
                {
                    DisplayCard dp = displayCardObject.GetComponent<DisplayCard>();
                    if (dp != null && dp.adjacentCards.Contains(gameObject) && BoardSlot.GetCurrentEnergy() >= 2 && gm.currentPhase == GamePhase.Attack)
                    {
                        outerBorder.color = Color.red;
                        Debug.Log("Player1 Card's Attack:" + dp.GetCardAttack());
                        dice1.enabled = true;
                        dice2.enabled = true;

                        shuffler.AttackSound();

                        P1Power = dp.GetCardAttack();
                        P2Power = this.GetCardDefense();

                        SHealth = this.GetCardHealth();
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
    }

    public int GetP1Power()
    {
        return P1Power;
    }

    public int GetP2Power()
    {
        return P2Power;
    }

    public int GetSHealth() 
    {
        return SHealth;
    }

    public void SetSHealth(int health) 
    { 
        SHealth = health;
    }
}
