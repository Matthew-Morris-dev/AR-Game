using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWaypoint : MonoBehaviour
{
    [SerializeField]
    private float _ScaleFactor;
    private bool _scaled = false;
    [SerializeField]
    private GameManager _gm;
    private TutorialManager _tm;
    [SerializeField]
    private Text _waypointText;

    private void Start()
    {
        _tm = FindObjectOfType<TutorialManager>();
        _gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if(_tm == null)
        {
            _tm = FindObjectOfType<TutorialManager>();
        }
        if (_gm = null)
        {
            _gm = FindObjectOfType<GameManager>();
        }
        else if (_scaled == false)
        {
            this.transform.localScale = new Vector3(_gm.GetGameWorldScale() * _ScaleFactor, _gm.GetGameWorldScale() * _ScaleFactor, _gm.GetGameWorldScale() * _ScaleFactor);
            _scaled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            _tm.incrementTutorialStep();
            _waypointText.enabled = false;
            this.gameObject.SetActive(false);
        }
    }
}
