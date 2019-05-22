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
    private GameObject ARCoreDevice;
    [SerializeField]
    private Camera firstPersonCamera;
    private bool _Quitting = false;
    public DetectedPlane detectedPlane;
    private bool planeSet = false;
    [SerializeField]
    private GameObject detectSurfaceUI;
    [SerializeField]
    private GameObject gameOverUI;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private bool playerDead = false;
    [SerializeField]
    private GameObject _gameWorld;
    [SerializeField]
    private float _gameWorldScale;
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
        else
        {
            if(GameObject.FindGameObjectsWithTag("Plane").Length > 0)
            {
                OnTogglePlanes(false);
            }
        }

        if(playerDead)
        {
            gameOverUI.SetActive(true);
        }

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    SceneManager.LoadScene("MainMenu");
        //}
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
            
            if (detectedPlane.ExtentX <= detectedPlane.ExtentZ)
            {
                //_arenaScale = detectedPlane.ExtentX;
                _gameWorldScale = detectedPlane.ExtentX;
            }
            else
            {
                //_arenaScale = detectedPlane.ExtentZ;
                _gameWorldScale = detectedPlane.ExtentZ;
            }

            //Anchor anchor = this.detectedPlane.CreateAnchor(new Pose(detectedPlane.CenterPose.position, Quaternion.identity));
            Instantiate(_gameWorld, detectedPlane.CenterPose.position, Quaternion.identity);
            Instantiate(_player, detectedPlane.CenterPose.position, Quaternion.identity);
            firstPersonCamera.GetComponentInParent<ARCoreSession>().SessionConfig.PlaneFindingMode = GoogleARCore.DetectedPlaneFindingMode.Disabled;
            OnTogglePlanes(false);
            planeSet = true;
            detectSurfaceUI.SetActive(false);
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
        StartCoroutine(restart());
        OnTogglePlanes(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator restart()
    {
        ARCoreSession session = ARCoreDevice.GetComponent<ARCoreSession>();
        ARCoreSessionConfig myConfig = session.SessionConfig;
        DestroyImmediate(session);
        // Destroy(session);
        Debug.Log("destroyed session");
        yield return null;
        session = ARCoreDevice.AddComponent<ARCoreSession>();
        session.SessionConfig = myConfig;
        session.enabled = true;
        Debug.Log("new session");
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

    public bool GetPlaneSet()
    {
        return planeSet;
    }
    
    public float GetGameWorldScale()
    {
        return _gameWorldScale;
    }

    public void SetPlayerDead(bool value)
    {
        playerDead = value;
    }
}
