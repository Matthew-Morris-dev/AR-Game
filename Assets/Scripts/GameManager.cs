using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GoogleARCore;
//using GoogleARCore.Examples.Common;
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

    //public DetectedPlane detectedPlane;

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
    private GameObject pauseMenuUI;
    [SerializeField]
    private WaveController WC;
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
    private PhotonGameManager PGM;
    [SerializeField]
    private bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        object[] myData = new object[1];
        myData[0] = PlayerNetwork.Instance.GetName();
        Debug.Log("OVR HEADSET: " + OVRManager.isHmdPresent);
        if (OVRManager.isHmdPresent)
        {
            GameObject temp = PhotonNetwork.Instantiate("Player_VR", Vector3.zero, Quaternion.identity, 0, myData);
            temp.transform.parent = GameObject.Find("World").gameObject.transform;
            waveAnnouncementText = FindObjectOfType<WaveTextFinder>().GetComponent<TextMeshProUGUI>();
        }
        else
        {
            GameObject temp = PhotonNetwork.Instantiate("Player_Desktop", Vector3.zero, Quaternion.identity, 0, myData);
            temp.transform.parent = GameObject.Find("World").gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PGM == null)
        {
            PGM = FindObjectOfType<PhotonGameManager>();
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

    public void UpdateKills(int kills)
    {
        killTrackerText.text = ("KILLS: " + kills);
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

    private void WaitGameOver()
    {
        gameOver = true;
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

    public int GetKills()
    {
        return numberOfKills;
    }
}
