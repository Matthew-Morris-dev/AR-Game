using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GoogleARCore;
using GoogleARCore.Examples.Common;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Minions;
    public GameObject _characterToSpawn;
    //public Text currentMinionText;
    private bool _playerCharacterSpawned = false;
    private GameManager _gm;
    [SerializeField]
    private TutorialTextController _ttc;
    //spawning variables

    // Start is called before the first frame update
    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        SetPlayerCharacterToCube();
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
        if ((_gm.GetPlaneSet() == true) && _playerCharacterSpawned == false)
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
        if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds | TrackableHitFlags.PlaneWithinPolygon;

            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                //anchor hopefully keeps the players character model in a good place.
                Anchor anchor = _gm.detectedPlane.CreateAnchor(new Pose(hit.Pose.position, Quaternion.identity));
                if (_characterToSpawn == Minions[0])
                {
                    Instantiate(_characterToSpawn, hit.Pose.position + new Vector3(0f, (_characterToSpawn.transform.localScale.y/2), 0f), Quaternion.identity, anchor.transform);
                    _ttc.IncrementTutText();
                    OnTogglePlanes(false);
                    //currentMinionText.text = "Current Minion:" + _characterToSpawn.name; //Debugging
                }
                else
                {
                    Instantiate(_characterToSpawn, hit.Pose.position, Quaternion.identity, anchor.transform);
                }
                if(_characterToSpawn == Minions[0])
                {
                    _playerCharacterSpawned = true;
                }
            }
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
    /* might use later
    private void SpawnFoodInstance()
    {
        insideFoodSpawnerText.enabled = true;
        GameObject foodItem = foodModels[Random.Range(0, foodModels.Length)];

        List<Vector3> vertices = new List<Vector3>();
        _sc.detectedPlane.GetBoundaryPolygon(vertices);
        Vector3 pt = vertices[Random.Range(0, vertices.Count)];
        float dist = Random.Range(0.05f, 1f);
        Vector3 position = Vector3.Lerp(pt, _sc.detectedPlane.CenterPose.position, dist);
        //move the object above the plane
        position.y += .05f;

        Anchor anchor = _sc.detectedPlane.CreateAnchor(new Pose(position, Quaternion.identity));

        foodObject = Instantiate(foodItem, position, Quaternion.identity, anchor.transform);

        //Set the tag
        foodObject.tag = "food";

        foodObject.transform.localScale = new Vector3(.25f, .25f, .25f);
        foodObject.transform.SetParent(anchor.transform);
        foodAge = 0;

        foodObject.AddComponent<FoodMotion>();
        _sc.foodTagText.text = "food tag is: " + foodObject.tag;
        Debug.Log("we spawned: " + foodObject.name);
    }
    */

    public void SetSpawnObject(GameObject obj)
    {
        _characterToSpawn = obj;
        //currentMinionText.text = "Current Minion:" + _characterToSpawn.name;
    }
    /*
    public void SetSpawnToTotem()
    {
        _characterToSpawn = _gm.GetEnemy(0);
        currentMinionText.text = "Current Minion:" + _characterToSpawn.name;
    }
    */
    public void SetPlayerCharacterToCube()
    {
        _characterToSpawn = Minions[0];
        //currentMinionText.text = "Current Minion:" + _characterToSpawn.name;
    }

    public void SetPlayerCharacterToSphere()
    {
        _characterToSpawn = Minions[1];
        //currentMinionText.text = "Current Minion: " + _characterToSpawn.name;
    }

    public void SetPlayerCharacterSpawned(bool value)
    {
        _playerCharacterSpawned = value;
    }
}
