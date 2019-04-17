using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class characterController : MonoBehaviour
{
    [SerializeField]
    private Camera _ARCamera;
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private MobileJoystickController _jsc;
    //Target Stuff
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private Text _joystickStatus;
    [SerializeField]
    private Text _currentVelocity;

    // Start is called before the first frame update
    void Start()
    {
        _ARCamera = FindObjectOfType<Camera>().GetComponent<Camera>();
        _joystickStatus = GameObject.Find("Joystick Status").GetComponent<Text>();
        _currentVelocity = GameObject.Find("Velocity").GetComponent<Text>();
        _rb = GetComponent<Rigidbody>();
        _jsc = FindObjectOfType<MobileJoystickController>().GetComponent<MobileJoystickController>();
        _target = GameObject.FindGameObjectWithTag("Enemy");
        FindObjectOfType<YbuttonController>().SetCharacterController(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Always face the enemy
        if (_target != null)
        {
            this.gameObject.transform.LookAt(_target.transform.position);
        }
        _joystickStatus.text = "Joystick Status: " + _jsc.GetJoystickActive().ToString();
        _currentVelocity.text = "Velocity: " + _rb.velocity.magnitude.ToString();
    }

    private void FixedUpdate()
    {
        if (_jsc.GetJoystickActive())
        {
            Vector3 camForward = _ARCamera.transform.forward;
            Vector3 camRight = _ARCamera.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward = camForward.normalized;
            camRight = camRight.normalized;

            Vector3 vel = ((camForward * _jsc.inputDirection.y + camRight * _jsc.inputDirection.x) * _speed * Time.deltaTime);
            _rb.velocity = vel;
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }
    }
}
