using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardShuffler : MonoBehaviour
{
    public List<DisplayCard> displayCards; // List of all display cards
    public Button shuffleButton; // Reference to the shuffle button
    public Animator cardAnimator; // Reference to the Animator component on the object you want to animate

   // public GameObject cardspre;
    public GameObject deck;
    public GameObject hand;

    private Dictionary<int, int> usedDetailsCount = new Dictionary<int, int>(); // Track used detail counts

    private void Start()
    {
        // Add a click listener to the shuffle button
        shuffleButton.onClick.AddListener(ShuffleCards);
    }

    public void ShuffleCards()
    {
        int childCount = deck.transform.childCount;

        if (childCount > 0)
        {
            // Find the last child (last card) under the Deck
            Transform lastCard = deck.transform.GetChild(childCount - 1);

            // Change the parent of the last card to the Hand
            lastCard.SetParent(hand.transform);
        }
        //Destroy(cardspre);
        // Play the shuffle animation
        cardAnimator.SetTrigger("ShuffleTrigger");
        StartCoroutine(BackToDefault(3));
        // Reset the used detail count dictionary
        usedDetailsCount.Clear();

        foreach (var card in displayCards)
        {
            ShuffleCard(card);
        }
    }

    private void ShuffleCard(DisplayCard card)
    {
        // Initialize variables to track the random detail and its count
        int randomDetail;
        int detailCount;

        // Generate a random detail until it doesn't exceed the limit of 2
        do
        {
            randomDetail = Random.Range(1, 5);
            usedDetailsCount.TryGetValue(randomDetail, out detailCount);
        } while (detailCount >= 2);

        // Update the used detail count
        usedDetailsCount[randomDetail] = detailCount + 1;

        // Set the random detail ID and update the card information
        if (card.transform.parent.name == "Deck") 
        {
        card.displayId = randomDetail;
        card.UpdateCardInformation();
        }
    }

    IEnumerator BackToDefault(float delay)
    {
        yield return new WaitForSeconds(delay);
        cardAnimator.SetTrigger("BackTrigger");
    }
}
