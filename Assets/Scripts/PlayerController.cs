using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Movement Variables
    public float Speed = 5f;
    public float JumpHeight = .5f;
    private Vector3 _moveDirection = Vector3.zero;
    //Attack Variables
    [SerializeField]
    private float _attackDamage;
    [SerializeField]
    private float _attackRange;
    //HitPoints Variables
    [SerializeField]
    private int _currentHealth = 100;
    private int _maxHealth;
    [SerializeField]
    private Image _healthBarImage;
    //Enemies Container
    private List<EnemyController> Enemies;
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
        //_currentVelocity = GameObject.Find("Velocity").GetComponent<Text>();
        //_joystickStatus = GameObject.Find("Joystick Status").GetComponent<Text>();
        //_ARCText = GameObject.Find("PlayerARCText").GetComponent<Text>();
        //_JSCText = GameObject.Find("PlayerJSC").GetComponent<Text>();

        //actuall needed stuff
        _ARCamera = FindObjectOfType<Camera>().GetComponent<Camera>();
        //_ARCText.text = "ARCam is: " + _ARCamera; //Debugging
        _rb = GetComponent<Rigidbody>();
        _groundChecker = transform.GetChild(0);
        _gm = FindObjectOfType<GameManager>();
        //_jsc = FindObjectOfType<MobileJoystickController>().GetComponent<MobileJoystickController>();

        //Get Mobile Joystick (so we can move player)
        _jsc = FindObjectOfType<MobileJoystickController>().GetComponent<MobileJoystickController>();
        //_JSCText.text = "JSC is: " + _jsc; //Debugging

        //Tell the buttons to control the player
        FindObjectOfType<YbuttonController>().SetPlayerController(this.gameObject);
        FindObjectOfType<XbuttonController>().SetPlayerController(this.gameObject);

        //Tell enemies who spawn before player about the player
        FindObjectOfType<EnemyController>().SetPlayer(this.gameObject);
        
        //This code makes the game scalable
        //this.transform.localScale = new Vector3(_gm.GetArenaScale(), _gm.GetArenaScale(), _gm.GetArenaScale()) * 0.1f;
        //Speed *= _gm.GetArenaScale() * 0.1f;
       // JumpHeight *= _gm.GetArenaScale() * 0.1f;
        //_attackRange *= _gm.GetArenaScale() * 0.1f;
       _target = GameObject.FindGameObjectWithTag("Enemy");
        //Set Max Health
        _maxHealth = _currentHealth;
    }

    void Update()
    {
        //Debugging Stuff
        if(_jsc == null)
        {
            _JSCText.text = "JSC is: null :( ";
        }
        //check if player is on the ground
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        //get player movement from joystick and calculate movement accoring to the camera's forward 
        //and right position, this allows players to walk around the augmented world
        //keeping movement unified.
        Vector3 camForward = _ARCamera.transform.forward;
        Vector3 camRight = _ARCamera.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward = camForward.normalized;
        camRight = camRight.normalized;

        _moveDirection = ((camForward * _jsc.inputDirection.y + camRight * _jsc.inputDirection.x)/* * _speed * Time.deltaTime*/);

        //_currentVelocity.text = "moveDirection: " + _moveDirection; //Debugging
        //_joystickStatus.text = "Joystick" + _jsc.GetJoystickActive(); //Debugging

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
            _isGrounded = false;
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
        _rb.AddForce((Vector3.up + (-1*this.transform.forward))* Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        UpdateHealthBar();
        if(_currentHealth <= 0)
        {
            Death();
        }
    }

    public bool GetIsGrounded()
    {
        return _isGrounded;
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveDirection * Speed * Time.fixedDeltaTime);
    }
    private void UpdateHealthBar()
    {
        float hpbarFill = (float)_currentHealth / (float)_maxHealth;
        _healthBarImage.fillAmount = hpbarFill;
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

    public void SetTarget(GameObject obj)
    {
        _target = obj;
    }
    /*
    public void AddEnemy(GameObject obj)
    {
        Enemies.Add(obj.GetComponent<EnemyController>());
        obj.GetComponent<EnemyController>().SetPlayer(this.gameObject);
    }

    public void RemoveEnemy(GameObject obj)
    {
        Enemies.Remove(obj.GetComponent<EnemyController>());
    }
    */
}
