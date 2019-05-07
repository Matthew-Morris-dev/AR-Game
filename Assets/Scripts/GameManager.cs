using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.Examples.Common;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
// NOTE:
// - InstantPreviewInput does not support `deltaPosition`.
// - InstantPreviewInput does not support input from
//   multiple simultaneous screen touches.
// - InstantPreviewInput might miss frames. A steady stream
//   of touch events across frames while holding your finger
//   on the screen is not guaranteed.
// - InstantPreviewInput does not generate Unity UI event system
//   events from device touches. Use mouse/keyboard in the editor
//   instead.
using Input = GoogleARCore.InstantPreviewInput;
#endif

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Camera firstPersonCamera;
    private bool _Quitting = false;
    public DetectedPlane detectedPlane;
    private bool planeSet = false;
    [SerializeField]
    private TutorialTextController _ttc;

    [SerializeField]
    private GameObject _gameWorld;
    [SerializeField]
    private float _gameWorldScale;
    //Playable characters
    //[SerializeField]
    //private GameObject[] ListOfCharacters;
    //Enemies
    //[SerializeField]
    //private GameObject _Enemy;
    //[SerializeField]
    //private GameObject _arena;
    //private float _arenaScale;
    /*
    [SerializeField]
    private Text extentXtext;
    [SerializeField]
    private Text extentZtext;
    */
    // Start is called before the first frame update
    void Start()
    {
        QuitOnConnectionErrors();
    }

    // Update is called once per frame
    void Update()
    {
        //session must be tracking in order access the frame
        if (Session.Status != SessionStatus.Tracking)
        {
            int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
            return;
        }
        //if we are tracking check if screen is touched
        if (!planeSet)
        {
            ProcessTouches();
        }
    }

    //This will detect if user touches the screen.
    //if so then cast ray from camera to the touched position and check if it hits a ARCore detected plane
    private void ProcessTouches()
    {
        Touch touch;
        if ((Input.touchCount != 1) || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds | TrackableHitFlags.PlaneWithinPolygon;

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {
            SetSelectedPlane(hit.Trackable as DetectedPlane);
        }
    }

    //Used to test if raycast is working as intended
    private void SetSelectedPlane(DetectedPlane selectedPlane)
    {
        if (selectedPlane.PlaneType == DetectedPlaneType.HorizontalUpwardFacing)
        {
            Debug.Log("selected plane at " + selectedPlane.CenterPose.position);
            detectedPlane = selectedPlane;
            if (detectedPlane.ExtentX >= detectedPlane.ExtentZ)
            {
                //_arenaScale = detectedPlane.ExtentX;
                _gameWorldScale = detectedPlane.ExtentX;
            }
            else
            {
                //_arenaScale = detectedPlane.ExtentZ;
                _gameWorldScale = detectedPlane.ExtentZ;
            }
            //Instantiate(_arena, detectedPlane.CenterPose.position, Quaternion.identity);
            Instantiate(_gameWorld, detectedPlane.CenterPose.position, Quaternion.identity);
            OnTogglePlanes(false);
            /*
            if (_Enemy.name == "Enemy")
            {
                Instantiate(_Enemy, detectedPlane.CenterPose.position + new Vector3(0f,0.3f,0f), Quaternion.identity);
            }
            else
            {
                Instantiate(_Enemy, detectedPlane.CenterPose.position, Quaternion.identity);
            }
            */
            planeSet = true;
            _ttc.IncrementTutText();
            //extentXtext.text = "arena scale: " + _arenaScale;
            //extentZtext.text = "ExtentZ: " + detectedPlane.ExtentZ * 0.1;
            //_groundPlane.transform.localScale = new Vector3(detectedPlane.ExtentX * 0.1f, 1f, detectedPlane.ExtentZ * 0.1f);
            //Instantiate(_groundPlane, detectedPlane.CenterPose.position, Quaternion.identity);
        }
        else
        {
            return;
        }
    }

    //checks the ARCore session and that ARCore is working in our application
    private void QuitOnConnectionErrors()
    {
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            _ShowAndroidToastMessage("Camera permission is needed to run this application.");
            _Quitting = true;
            Invoke("_Quit()", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
            _Quitting = true;
            Invoke("_Quit()", 0.5f);
        }
    }

    //close application
    private void _Quit()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        OnTogglePlanes(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // Taken Directly from ARCore HELLOAR example (I understand what this does but not how it works !! need to learn)
    /// <summary>
    /// Show an Android toast message.
    /// </summary>
    /// <param name="message">Message string to show in the toast.</param>
    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                    message, 0);
                toastObject.Call("show");
            }));
        }
    }

    public void OnTogglePlanes(bool flag)
    {
        foreach (GameObject plane in GameObject.FindGameObjectsWithTag("Plane"))
        {
            Renderer r = plane.GetComponent<Renderer>();
            DetectedPlaneVisualizer t = plane.GetComponent<DetectedPlaneVisualizer>();
            r.enabled = flag;
            t.enabled = flag;
        }
    }
    /*
    public void VisualizePlanes(bool showPlanes)
    {
        foreach (GameObject plane in GameObject.FindGameObjectsWithTag("Plane"))
        {
            Renderer r = plane.GetComponent<Renderer>();
            DetectedPlaneVisualizer t = plane.GetComponent<DetectedPlaneVisualizer>();
            r.enabled = showPlanes;
            t.enabled = showPlanes;
        }
    }
    */
    public bool GetPlaneSet()
    {
        return planeSet;
    }

    /*
    public float GetArenaScale()
    {
        return _arenaScale;
    }
    */
    public float GetGameWorldScale()
    {
        return _gameWorldScale;
    }
    /*
    public void SpawnEnemy()
    {
        Instantiate(_Enemy, detectedPlane.CenterPose.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity);
    }
    */
}
