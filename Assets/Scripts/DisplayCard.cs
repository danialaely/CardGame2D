using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayCard : MonoBehaviour
{
    public List<Card> display = new List<Card>();
    public int displayId;

    public TMP_Text nameText;
    public TMP_Text attackText;
    public TMP_Text healthText;
    public TMP_Text energyText;


  //  public GameObject[] cardObjects;

    // arr[card]   if(arr[card].transform.parent == deck)

    // Start is called before the first frame update
    void Start()
    {
        UpdateCardInformation();

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
}
