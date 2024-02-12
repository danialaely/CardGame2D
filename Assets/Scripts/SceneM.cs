using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneM : MonoBehaviour
{
    public void LoadPlayScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
