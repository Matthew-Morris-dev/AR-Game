﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Movement Variables
    public float Speed = 5f;
    public float JumpHeight = 2f;
    private Vector3 _moveDirection = Vector3.zero;
    //Attack Variables
    [SerializeField]
    private float _attackDamage;
    [SerializeField]
    private float _attackRange;

    //Check grounded variables
    public float GroundDistance = 0.2f;
    public LayerMask Ground;
    private Transform _groundChecker;
    [SerializeField]
    private bool _isGrounded = true;

    //Other Components
    [SerializeField]
    private Camera _ARCamera;
    [SerializeField]
    private MobileJoystickController _jsc;
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private GameObject _target;
    private GameManager _gm;

    //debugging stuff
    [SerializeField]
    private Text _currentVelocity;
    [SerializeField]
    private Text _joystickStatus;
    [SerializeField]
    private Text _ARCText;
    [SerializeField]
    private Text _JSCText;

    void Start()
    {
        //debugging stuff
        _currentVelocity = GameObject.Find("Velocity").GetComponent<Text>();
        _joystickStatus = GameObject.Find("Joystick Status").GetComponent<Text>();
        //_ARCText = GameObject.Find("PlayerARCText").GetComponent<Text>();
        //_JSCText = GameObject.Find("PlayerJSC").GetComponent<Text>();

        //actuall needed stuff
        _ARCamera = FindObjectOfType<Camera>().GetComponent<Camera>();
        //_ARCText.text = "ARCam is: " + _ARCamera;
        _rb = GetComponent<Rigidbody>();
        _groundChecker = transform.GetChild(0);
        _gm = FindObjectOfType<GameManager>();
        //_jsc = FindObjectOfType<MobileJoystickController>().GetComponent<MobileJoystickController>();
        _jsc = FindObjectOfType<MobileJoystickController>().GetComponent<MobileJoystickController>();
        //_JSCText.text = "JSC is: " + _jsc;
        FindObjectOfType<YbuttonController>().SetPlayerController(this.gameObject);
        FindObjectOfType<XbuttonController>().SetPlayerController(this.gameObject);

        this.transform.localScale = new Vector3(_gm.GetArenaScale(), _gm.GetArenaScale(), _gm.GetArenaScale()) * 0.1f;
        Speed *= _gm.GetArenaScale() * 0.1f;
        JumpHeight *= _gm.GetArenaScale() * 0.1f;
        _target = GameObject.FindGameObjectWithTag("Enemy");
    }

    void Update()
    {
        if(_jsc == null)
        {
            _JSCText.text = "JSC is: null :( ";
        }
        //check if player is on the ground
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        //get player movement from joystick
        Vector3 camForward = _ARCamera.transform.forward;
        Vector3 camRight = _ARCamera.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward = camForward.normalized;
        camRight = camRight.normalized;

        _moveDirection = ((camForward * _jsc.inputDirection.y + camRight * _jsc.inputDirection.x)/* * _speed * Time.deltaTime*/);

        _currentVelocity.text = "moveDirection: " + _moveDirection;
        _joystickStatus.text = "Joystick" + _jsc.GetJoystickActive();
        //make sure target is focused
        if (_target != null && _isGrounded)
        {
            //Makes sure we look at target at characters eye level (removes rotation if player lower or higher than target center of mass)
            Vector3 targetLookPos = _target.transform.position;
            targetLookPos.y = this.transform.position.y;
            this.gameObject.transform.LookAt(targetLookPos);
        }
        /*
        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");
        */
        /*
        if (_inputs != Vector3.zero)
            transform.forward = _inputs;
        */
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rb.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
        /*
        if (Input.GetButtonDown("Dash"))
        {
            Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
            _body.AddForce(dashVelocity, ForceMode.VelocityChange);
        }*/
    }

    public void Attack()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, _attackRange))
        {
            if (hit.transform.gameObject.tag == "Enemy")
            {
                hit.transform.gameObject.SendMessage("TakeDamage", _attackDamage);
            }
        }
    }

    public void Dodge()
    {
        _rb.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }

    public bool GetIsGrounded()
    {
        return _isGrounded;
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveDirection * Speed * Time.fixedDeltaTime);
    }
}
