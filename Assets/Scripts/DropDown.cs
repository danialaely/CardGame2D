using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropDown : MonoBehaviour
{
    public GameObject SinglePImg;
    public GameObject MultiplayerImg;
    public int value;

    public GameObject PlayBtn;
    public TMP_Text PBtnTxt;

    public void HandleInputData(int val) 
    {
        value = val;
        if (val ==0) 
        {
            SinglePImg.SetActive(false);
            MultiplayerImg.SetActive(true);
            PlayBtn.GetComponent<Image>().color = Color.white;
            PBtnTxt.color = Color.white;
        }
        if (val == 1)
        {
            SinglePImg.SetActive(true);
            MultiplayerImg.SetActive(false);
           PlayBtn.GetComponent<Image>().color = Color.gray;
            PBtnTxt.color = Color.gray;
            //414141
        }

    }

    public int Options() 
    {
        return value;
    }

}
