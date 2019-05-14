using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rayCastIcon : MonoBehaviour
{
    [SerializeField]
    private Material[] _mats;

    private Camera _ARCamera;
    private characterController _player;

    //Scaling stuff
    [SerializeField]
    private float _xScaleFactor;
    [SerializeField]
    private float _yScaleFactor;
    [SerializeField]
    private float _zScaleFactor;
    [SerializeField]
    private GameManager _gm;
    private bool _scaled = false;
    // Start is called before the first frame update
    void Start()
    {
        _ARCamera = FindObjectOfType<Camera>();
        _player = FindObjectOfType<characterController>();
        _gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_gm == null)
        {
            _gm = FindObjectOfType<GameManager>();
        }
        else if(_scaled == false)
        {
            this.transform.localScale = new Vector3(_gm.GetGameWorldScale() * _xScaleFactor, _gm.GetGameWorldScale() * _yScaleFactor, _gm.GetGameWorldScale() * _zScaleFactor);
            _scaled = true;
        }
        
        RaycastHit hit;
        if (Physics.Raycast(_ARCamera.transform.position, _ARCamera.transform.forward, out hit, 100))
        {
            if (hit.transform.gameObject.tag == "Arena")
            {
                this.gameObject.GetComponent<Renderer>().material = _mats[0];
                this.transform.position = hit.point;
                this.transform.up = hit.normal;
                Vector3 playerMovePosition = new Vector3(hit.point.x, _player.transform.position.y, hit.point.z);
                _player.setMoveDir(playerMovePosition);
            }
            else if(hit.transform.gameObject.tag == "Enemy")
            {
                this.gameObject.GetComponent<Renderer>().material = _mats[1];
                this.transform.position = hit.point;
                this.transform.up = hit.normal;
                Vector3 playerMovePosition = new Vector3(hit.point.x, _player.transform.position.y, hit.point.z);
                _player.setMoveDir(playerMovePosition);
            }
        }
        else
        {
            _player.setMoveDir(_player.transform.position);
        }

    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<characterController>().setCanMove(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<characterController>().setCanMove(true);
        }
    }*/
}
