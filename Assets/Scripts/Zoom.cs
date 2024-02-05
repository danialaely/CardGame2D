using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zoom : MonoBehaviour
{
    public Camera mainCamera; // Reference to your main camera
    private float originalOrthographicSize;
    private Vector3 originalCamPos;

    public float maxDoubleTapTime = 0.3f; // Maximum time allowed between taps
    private float lastTapTime; // Time of the last tap
    private static bool isBoolVariableTrue = false; // Initial value of the boolean variable

    public Image dice1;
    public Image dice2;
     Vector3 originaldice1Pos;
     Vector3 originaldice2Pos;

    Vector3 zoomPos;

    public AudioSource src;
    public AudioClip shuffleClip;

    private void Start()
    {
        originaldice1Pos = dice1.transform.position;
        originaldice2Pos = dice2.transform.position;

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

    void Update()
    {
       
       // mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 400f, 0.0123f);
        // Check for a tap or click
        if (Input.GetMouseButtonDown(0)) // Assuming left mouse button for simplicity
        {
            // Check if it's a double tap
            if (Time.time - lastTapTime < maxDoubleTapTime && mainCamera != null)
            {
                // Double tap detected
                Debug.Log("Double Tap!");

                // Toggle the boolean variable
                isBoolVariableTrue = !isBoolVariableTrue;

                if (isBoolVariableTrue)
                {
                    Debug.Log("Boolean variable is true. Performing zoom in.");

                    // Get the position of the touch/click on the screen
                    Vector3 touchPos = Input.mousePosition;
                    touchPos.z = -10f; // Set a fixed distance from the camera


                    Vector3 touchPoz = Input.mousePosition;
                    touchPoz.z = 400f;
                    touchPoz.x += 90f;

                    // Convert screen space to world space
                    Vector3 worldPos = mainCamera.ScreenToWorldPoint(touchPos);


                    // Set the target orthographic size
                   // float targetOrthographicSize = 411f;

                    // Smoothly interpolate to the target orthographic size and position
                    float transitionSpeed = 0.5f;

                    //mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthographicSize, transitionSpeed);
                   zoomPos = Vector3.Lerp(mainCamera.transform.position, worldPos, transitionSpeed);

                }
            }

            // Update the last tap time
            lastTapTime = Time.time;
        }

        // Perform actions based on the boolean variable
        if (isBoolVariableTrue)
        {
            Debug.Log("Boolean variable is true. Performing zoom in.");

            // Set the target orthographic size
            float targetOrthographicSize = 411f;

            // Smoothly interpolate to the target orthographic size and position
            float transitionSpeed = 0.0225f;

            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthographicSize, transitionSpeed);
            if (mainCamera.transform.position != zoomPos) 
            {
                //mainCamera.transform.position = zoomPos;
                mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, zoomPos, 8f);
            }
        }
        else
        {
            Debug.Log("Boolean variable is false. Performing reset.");

            // Reset the camera to its original state
            if (mainCamera.orthographicSize <= 700f)
            {
                float targetOrthographicSize = 765.3f;
                float transitionSpeed = 0.0225f;
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthographicSize, transitionSpeed);
                dice1.transform.position = originaldice1Pos;
                dice2.transform.position = originaldice2Pos;
            }
            else 
            { 
            mainCamera.orthographicSize = originalOrthographicSize;
            mainCamera.transform.position = originalCamPos;
            dice1.transform.position = originaldice1Pos;
            dice2.transform.position = originaldice2Pos;
            }
        }
    }

    

    public static bool GetBool() 
    {
        return isBoolVariableTrue;
    }

    public void DeckSound() 
    {
        src.clip = shuffleClip;
        src.Play();
    }
}
