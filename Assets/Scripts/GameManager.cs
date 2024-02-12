using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //public GamePhase currentPhase { get; private set; }

    public GamePhase currentPhase;
    public TMP_Text  phaseText;

    public AudioSource src;
    public AudioClip errorClip;

    // Start is called before the first frame update
    void Start()
    {
        ChangePhase(GamePhase.Draw);
    }

    public void ChangePhase(GamePhase newPhase)
    {
        currentPhase = newPhase;
        Debug.Log("Game Phase:"+currentPhase);
        phaseText.text = currentPhase.ToString();
        // Additional logic for transitioning between phases, if needed
    }

    public void ErrorSound() 
    {
        src.clip = errorClip;
        src.Play();
    }

    public void OnTurnButtonClick()
    {
        // Assuming GameManager is a static class or a singleton
        if (currentPhase == GamePhase.Draw)
        {
            ChangePhase(GamePhase.Setup);
        }
        else if (currentPhase == GamePhase.Setup) 
        {
            ChangePhase(GamePhase.Move);
        }
    }

}

public enum GamePhase 
{
    Draw, 
    Setup, 
    Move
}
