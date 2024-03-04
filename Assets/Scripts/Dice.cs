using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Dice : MonoBehaviour
{

    // Array of dice sides sprites to load from Resources folder
    private Sprite[] diceSides;

    // Reference to sprite renderer to change sprites
   // private SpriteRenderer rend;
    private Image image;
    public Image otherdice;

    private static int finalSide=0;
    private static int finalSide2 = 0;

    public List<DisplayCard2> allDpCards;
    public List<DisplayCard> allDisplayCards;

    public GameObject discardpile;
    public GameObject discardpile2;

    public Animator discaranimator;
    public Animator animator2;

    public AudioSource src;
    public AudioClip diceClip;
    public AudioClip discardedClip;

    // Use this for initialization
    private void Start()
    {

        // Assign Renderer component
       // rend = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");

        allDpCards = new List<DisplayCard2>(FindObjectsOfType<DisplayCard2>());
        allDisplayCards = new List<DisplayCard>(FindObjectsOfType<DisplayCard>());
    }

    // If you left click over the dice then RollTheDice coroutine is started
    public void OnMouseDown()
    {
        DiceSound();
        StartCoroutine(RollAndDiscard());
    }

    private IEnumerator RollAndDiscard() 
    {
        yield return StartCoroutine(RollTheDice());
        yield return StartCoroutine(RollTheDice2());
        Discarded();
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
       // int finalSide = 0;

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = Random.Range(0, 5);

            // Set sprite to upper face of dice from array according to random value
            image.sprite = diceSides[randomDiceSide];
            
            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        finalSide = randomDiceSide + 1;

        // Show final dice value in Console
        Debug.Log("Our Dice:"+finalSide);
    }

    private IEnumerator RollTheDice2()
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide2 = 0;

        // Final side or value that dice reads in the end of coroutine
        //int finalSide2 = 0;

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide2 = Random.Range(0, 5);

            // Set sprite to upper face of dice from array according to random value
            otherdice.sprite = diceSides[randomDiceSide2];

            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        finalSide2 = randomDiceSide2 + 1;

        // Show final dice value in Console
        Debug.Log("Opponent's Dice:"+finalSide2);
    }

    public int GetDice() 
    {
        return finalSide;
    }

    public int GetDice2() 
    {
        return finalSide2;
    }

    public void Discarded() 
    {
        //if (GetDice() > GetDice2()) {Debug.Log(" Dice Attack True");}

                bool isP1Turn = ButtonTurn.GetPlayerTurn();
            if (isP1Turn)
            {//DisplayCard2
                    
                foreach (DisplayCard2 defenderCard in allDpCards)
                {
                    Debug.Log("Selected:"+defenderCard.isSelected);
                    if (defenderCard.isSelected)
                    {
                    if ((GetDice()+defenderCard.GetP1Power()) > ((GetDice2())+defenderCard.GetP2Power())) 
                    {
                        Debug.Log("ATTACKER Dice:"+GetDice()+"+"+"Attack:"+defenderCard.GetP1Power()+"="+ (GetDice() + defenderCard.GetP1Power()));
                        Debug.Log("DEFENSE Dice:" + GetDice2() + "+" + "Attack:" + defenderCard.GetP2Power() + "=" + (GetDice2() + defenderCard.GetP2Power()));
                        //  Debug.Log("Discard Value:"+ defenderCard.GetDiscard());
                        //Destroy(defenderCard.gameObject);
                        //defenderCard.transform.position += new Vector3(600f,-300f,0f);
                        discaranimator.SetBool("isDiscard",true);
                        
                        Transform discarcard = defenderCard.transform;
                        discarcard.SetParent(discardpile.transform);
                        DiscardSound();
                    }
                    }
                }
                // discaranimator.SetBool("isDiscard", false);  make IEnumerator
                StartCoroutine(DiscardAnim(2.0f));
            }
            // isP1Turn = ButtonTurn.GetPlayerTurn() 
            // if(isP1Turn) then if(dp.isSelected == true) { Destroy(dp.gameobject) } 
            //defe.GetDiscard

            if (!isP1Turn) 
            {
                Debug.Log("in dice attack in isp1turn false");
                //DisplayCard
                foreach (DisplayCard defcard in allDisplayCards)
                {
                    if (defcard.isSelected) 
                    {
                    if ((GetDice() + defcard.Getp2Power()) > ((GetDice2()) + defcard.Getp1Power())) 
                    {
                        Debug.Log("ATTACKER Dice:" + GetDice() + "+" + "Attack:" + defcard.Getp2Power() + "=" + (GetDice() + defcard.Getp2Power()));
                        Debug.Log("DEFENSE Dice:" + GetDice2() + "+" + "Attack:" + defcard.Getp1Power() + "=" + (GetDice2() + defcard.Getp1Power()));
                        // Debug.Log("Discard Value:" +defcard.GetDiscard());
                        // Destroy(defcard.gameObject);
                        animator2.SetBool("isDiscarded",true);
                        Transform discardCard = defcard.transform;
                        discardCard.SetParent(discardpile2.transform);
                        DiscardSound();
                    }
                    }
                }
                StartCoroutine(DiscardAnim2(2.0f));
            }

        if (GetDice() < GetDice2()) 
        {
            Debug.Log("Dice Attack False");
        }

       
    }

    public void DiceSound() 
    {
        src.clip = diceClip;
        src.Play();
    }

    public void DiscardSound()
    {
        src.clip = discardedClip;
        src.Play();
    }

    private IEnumerator DiscardAnim(float delay) 
    {
        yield return new WaitForSeconds(delay);
        discaranimator.SetBool("isDiscard", false);
    }

    private IEnumerator DiscardAnim2(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator2.SetBool("isDiscarded", false);
    }
}
