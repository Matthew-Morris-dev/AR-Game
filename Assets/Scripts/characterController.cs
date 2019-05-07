﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GoogleARCore;
using UnityEngine.UI;

public class characterController : MonoBehaviour
{
    [SerializeField]
    private Camera _ARCamera;
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private MobileJoystickController _jsc;
    //Target Stuff
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _attackDamage;
    [SerializeField]
    private float _rateOfFire;

    private Vector3 moveDirection;
    
    [SerializeField]
    private float _jumpSpeed;
    [SerializeField]
    private bool _grounded = true;
    private float _groundRayDistance;

    private float startingY; // i dont think we need this
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
        _ARCamera = FindObjectOfType<Camera>().GetComponent<Camera>();
        _rb = GetComponent<Rigidbody>();
        _jsc = FindObjectOfType<MobileJoystickController>().GetComponent<MobileJoystickController>();
        _gm = FindObjectOfType<GameManager>();
        startingY = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Find GM or scale
        if (_gm == null)
        {
            _gm = FindObjectOfType<GameManager>();
        }
        else if (_scaled == false)
        {
            this.transform.localScale = new Vector3(_gm.GetGameWorldScale() * _xScaleFactor, _gm.GetGameWorldScale() * _yScaleFactor, _gm.GetGameWorldScale() * _zScaleFactor);
        }
        //Blend animations for moving diagonally
        _animator.SetFloat("Zmovement", _rb.velocity.z);
        _animator.SetFloat("Xmovement", _rb.velocity.x);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
        {
            _rb.velocity = Vector3.zero;
        }
        else if (_jsc.GetJoystickActive())
        {
            Vector3 camForward = _ARCamera.transform.forward;
            Vector3 camRight = _ARCamera.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward = camForward.normalized;
            camRight = camRight.normalized;

            moveDirection = ((camForward * _jsc.inputDirection.y + camRight * _jsc.inputDirection.x) * _speed * Time.deltaTime);
            _rb.velocity = moveDirection;
        }
    }
        
    public void Attack()
    {
        _animator.SetTrigger("Shoot");
        /*
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, _attackRange))
        {
            if (hit.transform.gameObject.tag == "Enemy")
            {
                hit.transform.gameObject.SendMessage("TakeDamage", _attackDamage);
            }
        }*/
    }

    public void TakeDamage()
    {
        Debug.Log("Player recieved damage message");
    }
    
    private void CheckGrounded()
    {
        if(_grounded)
        {
            _groundRayDistance = 0.35f;
        }
        else
        {
            _groundRayDistance = 0.15f;
        }

        if(Physics.Raycast(this.transform.position - new Vector3(0f, 0.15f, 0f), -1*this.transform.up, _groundRayDistance))
        {
            _grounded = true;
        }
        else
        {
            _grounded = false;
        }
    }

    public void SetGrounded(bool val)
    {
        _grounded = val;
    }

    public bool GetGrounded()
    {
        return _grounded;
    }
}
