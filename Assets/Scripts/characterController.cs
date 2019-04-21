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
    private CharacterController _CharController;
    [SerializeField]
    private MobileJoystickController _jsc;
    //Target Stuff
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _attackDamage;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _dodgeForce;
    [SerializeField]
    private Text _joystickStatus;
    [SerializeField]
    private Text _currentVelocity;

    private Vector3 moveDirection;

    //Jump Variables
    //private float _maxJumpHeight = .5f;
    //private float _groundHeight;
    [SerializeField]
    private float _jumpSpeed;
    //[SerializeField]
    //private float _fallSpeed;
    [SerializeField]
    private bool _grounded = true;
    private float _groundRayDistance;
    //[SerializeField]
    //private bool _falling = false;

    private float startingY;
    // Start is called before the first frame update
    void Start()
    {
        _ARCamera = FindObjectOfType<Camera>().GetComponent<Camera>();
        _joystickStatus = GameObject.Find("Joystick Status").GetComponent<Text>();
        _currentVelocity = GameObject.Find("Velocity").GetComponent<Text>();
        _rb = GetComponent<Rigidbody>();
        //_CharController = GetComponent<CharacterController>();
        _jsc = FindObjectOfType<MobileJoystickController>().GetComponent<MobileJoystickController>();
        _target = GameObject.Find("CenterOfMass");
        FindObjectOfType<XbuttonController>().SetCharacterController(this.gameObject);
        FindObjectOfType<YbuttonController>().SetCharacterController(this.gameObject);
        startingY = this.transform.position.y;
        //_groundHeight = this.transform.position.y;
        //_maxJumpHeight = this.transform.position.y * 5;
    }

    // Update is called once per frame
    void Update()
    {
        //Always face the enemy
        if (_target != null && _grounded)
        {
            this.gameObject.transform.LookAt(_target.transform.position);
        }
        //_joystickStatus.text = "Joystick Status: " + _jsc.GetJoystickActive().ToString();
        //_currentVelocity.text = "Velocity: " + _rb.velocity.magnitude.ToString();
        //_joystickStatus.text = "grounded: " + _CharController.isGrounded;
        Debug.Log(moveDirection);

        //Check grounded
        CheckGrounded();
        _joystickStatus.text = "grounded: " + _grounded;
        Vector3 tempPos = this.transform.position;
        tempPos.y = startingY;
        this.transform.position = tempPos;
    }

    private void FixedUpdate()
    {

        //if (_grounded)
            if (_jsc.GetJoystickActive())
            {
                Vector3 camForward = _ARCamera.transform.forward;
                Vector3 camRight = _ARCamera.transform.right;
                camForward.y = 0f;
                camRight.y = 0f;
                camForward = camForward.normalized;
                camRight = camRight.normalized;

                moveDirection = ((camForward * _jsc.inputDirection.y + camRight * _jsc.inputDirection.x) * _speed * Time.deltaTime);
                _rb.velocity = moveDirection;
                //_CharController.Move(vel);
            }
            else
            {
                moveDirection = Vector3.zero;
                _rb.velocity = moveDirection;
                //_CharController.Move(Vector3.zero);
            }
    }
        /*
        else
        {
            RaycastHit hit;
            if(Physics.Raycast(this.transform.position,Vector3.down,out hit,(this.transform.localScale.y / 2 + 0.05f)))
            {
                if(hit.transform.gameObject.tag == "Arena")
                {
                    _grounded = true;
                }
            }
        }
        */

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
        _rb.AddForce( -1 * this.transform.forward * _dodgeForce);
        //_grounded = false;


        //Vector3 DodgeRoll = this.transform.forward * -1 * _dodgeForce;
        //_rb.AddForce(DodgeRoll);
        //_falling = false;
        //_grounded = false;
        //StartCoroutine("JumpBackwards");
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        _joystickStatus.text = "We collided";
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 100f))
        {
            Vector3.Distance(hit.point, this.transform.position);
            if (hit.transform.gameObject.tag == "Arena" && Vector3.Distance(hit.point, this.transform.position) < 0.1)
            {
                _grounded = true;
            }
        }
    }
    */
    /*
    IEnumerator CheckGrounded()
    {
        _joystickStatus.text = Time.time.ToString();
        yield return new WaitForSeconds(0.5f);
        _joystickStatus.text += "we check if still in air now: " + Time.time.ToString();
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 100f))
        {
            Vector3.Distance(hit.point, this.transform.position);
            if (hit.transform.gameObject.tag == "Arena" && Vector3.Distance(hit.point, this.transform.position) < 0.1)
            {
                _grounded = true;
            }
        }
    }
    */
    /*
    IEnumerator JumpBackwards()
    {
        while(true)
        {
            if(this.transform.position.y >= _maxJumpHeight)
            {
                _falling = true;
            }
            if(_falling==false)
            {
                this.transform.Translate(Vector3.up * _jumpSpeed * Time.smoothDeltaTime);
            }
            else if(_falling)
            {
                this.transform.Translate(Vector3.down * _fallSpeed * Time.smoothDeltaTime);
                if(this.transform.position.y < _groundHeight)
                {
                    Vector3 tempPos = this.transform.position;
                    tempPos.y = _groundHeight;
                    this.transform.position = tempPos;
                    _grounded = true;
                    StopAllCoroutines();
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
    */
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
