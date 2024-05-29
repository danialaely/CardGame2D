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
   // Vector3 zoomOut;

    public AudioSource src;
    public AudioClip shuffleClip;
    public AudioClip errorClip;

    public GameManager gm;

    private Vector3 Origin;
    private Vector3 Difference;
    //private Vector3 ResetCamera;

    private bool drag = false;

    private bool stickToPos;

    private void Start()
    {
        originaldice1Pos = dice1.transform.position;
        originaldice2Pos = dice2.transform.position;

        stickToPos = false;

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

    private IEnumerator Fluctuate(float delay) 
    {
        yield return new WaitForSeconds(delay);
        stickToPos = !stickToPos;
    }

    void Update()
    {

        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Calculate the distance between the two touches in the previous frame and in the current frame
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;
            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            // Find the difference in the distances between each frame
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Perform zoom based on the magnitude difference
            if (deltaMagnitudeDiff > 0)
            {
                // Pinch zoom out
                isBoolVariableTrue = false;
            }
            else if (deltaMagnitudeDiff < 0)
            {
                // Pinch zoom in
                isBoolVariableTrue = true;

                // Get the midpoint between the two touches for zoom center
                Vector3 touchPos = (touch1.position + touch2.position) / 2;
                touchPos.z = -10f; // Set a fixed distance from the camera
                Vector3 worldPos = mainCamera.ScreenToWorldPoint(touchPos);
                zoomPos = Vector3.Lerp(mainCamera.transform.position, worldPos, 0.5f);

                // Update dice positions
                Vector3 offt1 = new Vector3(-400f, 0, 0);
                dice1.transform.position = touchPos + offt1;
                dice2.transform.position = touchPos - offt1;
            }
        }

        if (isBoolVariableTrue)
        {
            float targetOrthographicSize = 411f;
            float transitionSpeed = 0.0925f;
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthographicSize, transitionSpeed);

            if (!Input.GetMouseButton(0) && !stickToPos)
            {
                mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, zoomPos, 8f);
            }

            if (Input.GetMouseButton(0))
            {
                Difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - mainCamera.transform.position;
                if (!drag)
                {
                    drag = true;
                    Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else
            {
                drag = false;
                RestrictCameraPosition();
            }

            if (drag)
            {
                if (mainCamera.transform.position.x > -200f && mainCamera.transform.position.x < 1230f && mainCamera.transform.position.y < 690f && mainCamera.transform.position.y > -170f)
                {
                    Camera.main.transform.position = Origin - Difference;
                }
            }
        }
        else
        {
            if (mainCamera.orthographicSize <= 765f)
            {
                float targetOrthographicSize = 765.3f;
                float transitionSpeed = 0.0925f;
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthographicSize, transitionSpeed);
                mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, originalCamPos, 8f);
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
        if (gm.currentPhase == GamePhase.Draw) 
        {
        src.clip = shuffleClip;
        src.Play();
        }
        if (gm.currentPhase == GamePhase.Play || gm.currentPhase == GamePhase.Move) 
        {
            src.clip = errorClip;
            src.Play();
        }
    }

    private void RestrictCameraPosition()
    {
        if (mainCamera.transform.position.x < -200f)
        {
            mainCamera.transform.position += new Vector3(5f, 0, 0);
        }
        if (mainCamera.transform.position.x > 1230f)
        {
            mainCamera.transform.position += new Vector3(-5f, 0, 0);
        }
        if (mainCamera.transform.position.y > 690f)
        {
            mainCamera.transform.position += new Vector3(0, -5f, 0);
        }
        if (mainCamera.transform.position.y < -170f)
        {
            mainCamera.transform.position += new Vector3(0, 5f, 0);
        }
    }

}
