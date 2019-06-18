using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.Examples.Common;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/*
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
//using Input = GoogleARCore.InstantPreviewInput;
#endif
*/
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
    private TextMeshProUGUI waveAnnouncementText;
    [SerializeField]
    private TextMeshProUGUI killTrackerText;
    [SerializeField]
    private TextMeshProUGUI HPTrackerText;
    [SerializeField]
    private int numberOfKills = 0;
    [SerializeField]
    private int waveTracker = 0;
    [SerializeField]
    private GameObject gameOverUI;
    [SerializeField]
    private TextMeshProUGUI gameOverText;
    private bool gameOver = false;
    [SerializeField]
    private AudioSource restartGameAudio;
    [SerializeField]
    private GameObject skipTutorialUI;
    [SerializeField]
    private bool skipTutorialUIOpen = false;
    [SerializeField]
    private WaveController WC;
    [SerializeField]
    private TutorialManager TM;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private bool playerDead = false;
    [SerializeField]
    private GameObject _gameWorld;
    [SerializeField]
    private float _gameWorldScale;
    [SerializeField]
    private AudioSource ambientMusic;

    [SerializeField]
    private SceneTracker ST;

    [SerializeField]
    private bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        QuitOnConnectionErrors();
        ST = FindObjectOfType<SceneTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ST == null)
        {
            ST = FindObjectOfType<SceneTracker>();
        }
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

        if(gameOver)
        {
            GameOverTouches();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                paused = false;
                Time.timeScale = 0f;
                Debug.Log("Launch pause menu");
            }
            else
            {
                paused = true;
                Time.timeScale = 1f;
                Debug.Log("Launch pause menu");
            }
            /*
            ST.SetUsedEscape(true);
            ST.SetFastSkipMainMenu(false);
            Time.timeScale = 1f;
            restartGameAudio.Play();
            Invoke("RestartGame", 0.8f);
            */
        }
        /*
        if(playerDead)
        {
            gameOverUI.SetActive(true);
        }
        */
        /*
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused)
            {
                Time.timeScale = 1f;
                pauseMenu.SetActive(false);
            }
            else
            {
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
            }
            /*
            ST.SetUsedEscape(true);
            ST.SetFastSkipMainMenu(false);
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
        */
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

    private void GameOverTouches()
    {
        Touch touch;
        if ((Input.touchCount != 1) || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }
        ST.SetUsedEscape(false);
        restartGameAudio.Play();
        Invoke("RestartGame", 0.8f);
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
            if(ST.GetFastSkipMainMenu())
            {
                CheckSkipTutorial();
                SkipTutorial();
                ST.SetFastSkipMainMenu(false);
            }
            else
            {
                CheckSkipTutorial();
            }
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

    
    /*
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
    */
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
        GameOver();
    }

    public void AnnounceWave(int number)
    {
        waveTracker = number;
        waveAnnouncementText.text = ("WAVE " + number);
        waveAnnouncementText.enabled = true;
        Invoke("HideWave", 2f);
    }

    private void HideWave()
    {
        waveAnnouncementText.enabled = false;
    }

    public void IncrementKills()
    {
        numberOfKills++;
        killTrackerText.text = ("KILLS: " + numberOfKills);
    }

    private void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ambientMusic.Stop();
        if(waveTracker == 2 && numberOfKills == 1)
        {
            gameOverText.text = "Game Over! \n\n" + "You survived\n<color=#00C9FF>" + (waveTracker - 1) + "</color>\n" + " wave and killed\n<color=#00C9FF>" + numberOfKills + "</color>\n" + "enemy!";
        }
        else if(waveTracker == 2 && numberOfKills != 1)
        {
            gameOverText.text = "Game Over! \n\n" + "You survived\n<color=#00C9FF>" + (waveTracker - 1) + "</color>\n" + " wave and killed\n<color=#00C9FF>" + numberOfKills + "</color>\n" + "enemies!";
        }
        else if(waveTracker != 2 && numberOfKills == 1)
        {
            gameOverText.text = "Game Over! \n\n" + "You survived\n<color=#00C9FF>" + (waveTracker - 1) + "</color>\n" + " waves and killed\n<color=#00C9FF>" + numberOfKills + "</color>\n" + "enemy!";
        }
        else
        {
            gameOverText.text = "Game Over! \n\n" + "You survived\n<color=#00C9FF>" + (waveTracker - 1) + "</color>\n" + " waves and killed\n<color=#00C9FF>" + numberOfKills + "</color>\n" + "enemies!";
        }
        gameOverUI.SetActive(true);
        killTrackerText.enabled = false;
        Invoke("WaitGameOver", 1f);
    }

    private void CheckSkipTutorial()
    {
        TM = FindObjectOfType<TutorialManager>();
        WC = FindObjectOfType<WaveController>();
        skipTutorialUI.SetActive(true);
        skipTutorialUIOpen = true;
        Time.timeScale = 0f;
    }

    public void SkipTutorial()
    {
        TM.DestroyWaypointIndicator();
        TM.gameObject.SetActive(false);
        WC.setTutorialOver(true);
        Time.timeScale = 1f;
        skipTutorialUI.SetActive(false);
        skipTutorialUIOpen = false;
    }

    public void DontSkipTutorial()
    {
        Time.timeScale = 1f;
        skipTutorialUI.SetActive(false);
        skipTutorialUIOpen = false;
        TM.StartTutorial();
    }

    private void WaitGameOver()
    {
        gameOver = true;
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //close application
    public void Quit()
    {
        restartGameAudio.Play();
        Invoke("DelayQuit", 0.8f);
    }

    private void DelayQuit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void UpdateHealth(float health)
    {
        HPTrackerText.text = ("Health: " + health);
    }
}
