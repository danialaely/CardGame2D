using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public int cardId;
    public string cardName;
    public int cardAttack;
    public int cardHealth;
    public int cardEnergy;

    public Card() { }
    public Card(int id, string name, int attack, int health, int energy)
    {
        cardId = id;
        cardName = name;
        cardAttack = attack;
        cardHealth = health;
        cardEnergy = energy;
    }

}
