using System.Collections;
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
    private GameObject _muzzleFlash;
    [SerializeField]
    private LaserSight _laserSight;
    [SerializeField]
    private float _animationSpeed;
    [SerializeField]
    private MobileJoystickController _jsc;
    //Target Stuff
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private GameObject shootRaycastFrom;
    [SerializeField]
    private Vector3 shootRaycastHit;
    [SerializeField]
    private GameObject _bulletEmitter;
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private AudioSource gunShotSFX;
    [SerializeField]
    private float _attackDamage;
    [SerializeField]
    private float _rateOfFire;
    private float _timeSinceLastBullet = 0;
    [SerializeField]
    private bool shoot = false;
    [SerializeField]
    private bool canMove = true;
    private Vector3 moveDirection;
    private Vector3 lookDirection;

    //Health stuff
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private float _currentHealth;
    [SerializeField]
    private Image _hpBar;

    [SerializeField]
    private float _jumpSpeed;
    [SerializeField]
    private bool _grounded = true;
    private float _groundRayDistance;

    private float startingY; // i dont think we need this
    //Raycast Stuff
    [SerializeField]
    private GameObject rayCastIcon;
    //Scaling stuff
    [SerializeField]
    private float _scaleFactor;
    [SerializeField]
    private GameManager _gm;

    private bool _scaled = false;
    private bool _dead = false;
    // Start is called before the first frame update
    void Start()
    {
        _ARCamera = FindObjectOfType<Camera>().GetComponent<Camera>();
        _rb = GetComponent<Rigidbody>();
        //_jsc = FindObjectOfType<MobileJoystickController>().GetComponent<MobileJoystickController>();
        _gm = FindObjectOfType<GameManager>();
        startingY = this.transform.position.y;
        Instantiate(rayCastIcon, Vector3.zero, Quaternion.identity);
        _currentHealth = _maxHealth;
        _animator.SetFloat("Health", _currentHealth);
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
            this.transform.localScale = new Vector3(_gm.GetGameWorldScale() * _scaleFactor, _gm.GetGameWorldScale() * _scaleFactor, _gm.GetGameWorldScale() * _scaleFactor);
            _speed *= _gm.GetGameWorldScale() * _scaleFactor;
            _animationSpeed *= _gm.GetGameWorldScale() * _scaleFactor;
            _scaled = true;
        }
        _hpBar.fillAmount = _currentHealth / _maxHealth;
        if (_dead == false)
        {
            if(Input.touchCount > 0)
            {
                setCanMove(false);
                setShoot(true);
            }
            else
            {
                setShoot(false);
            }
            //Blend animations for moving diagonally
            float forwardMovement = ((Vector3.Dot(this.transform.forward, _rb.velocity)) / (this.transform.forward.magnitude));
            float rightMovement = ((Vector3.Dot(this.transform.right, _rb.velocity)) / (this.transform.right.magnitude));
            _animator.SetFloat("Zmovement", forwardMovement);
            _animator.SetFloat("Xmovement", rightMovement);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookDirection), _rotationSpeed * Time.deltaTime);

            RaycastHit hit;
            if (Physics.Raycast(shootRaycastFrom.transform.position, shootRaycastFrom.transform.forward, out hit, 100))
            {
                //Debug.Log("we shoot raycast");
                shootRaycastHit = hit.point;
            }
            //Debug.Log("hit:" + hit.transform.tag);
            _laserSight.SetLaserSightEnd(shootRaycastHit);

            if (shoot)
            {
                if(_timeSinceLastBullet >= _rateOfFire)
                {
                    /*
                    RaycastHit hit;
                    if (Physics.Raycast(shootRaycastFrom.transform.position, shootRaycastFrom.transform.forward, out hit, 100))
                    {
                        Debug.Log("we shoot raycast");
                        shootRaycastHit = hit.point;
                    }
                    Debug.Log("hit:" + hit.transform.tag);
                    */
                    gunShotSFX.pitch = Random.Range(-.25f, 0.25f) + 1f;
                    gunShotSFX.Play();
                    Instantiate(_bullet, _bulletEmitter.transform.position, Quaternion.identity);
                    _muzzleFlash.SetActive(true);
                    _timeSinceLastBullet = 0;
                }
                else
                {
                    _timeSinceLastBullet += Time.deltaTime;
                }
            }
            else
            {
                _muzzleFlash.SetActive(false);
            }
        }
        else
        {
            _rb.freezeRotation = true;
            _muzzleFlash.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (_dead == false)
        {
            if (canMove == true)
            {
                //_rb.velocity = moveDirection * _speed * Time.fixedDeltaTime;
                transform.Translate(moveDirection * _speed * Time.fixedDeltaTime, Space.World);
                _animator.SetTrigger("Walking");
            }
            else
            {
                _rb.velocity = Vector3.zero;
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
                {
                    _animator.SetTrigger("Idle");
                }
            }
            if (!shoot)
            {
                canMove = true;
            }
            /*
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
            */
        }
    }
    /*
    public void Attack(Vector3 target)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
        {
            return;
        }
        else
        {
            _animator.SetTrigger("Shoot");
            RaycastHit hit;
            if (Physics.Raycast(_gun.transform.position, (target - _gun.transform.position), out hit, 100))
            {
                Debug.Log("we are shooting");
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    Debug.Log("we hit :" + hit.transform.gameObject.name);
                    hit.transform.gameObject.GetComponent<EnemyController>().TakeDamage(_attackDamage);
                }
            }
        }
    }
    */
    public void TakeDamage(float damage)
    {
        Debug.Log("Player recieved damage message");
        _currentHealth -= damage;
        if(_currentHealth <= 0)
        {
            _animator.SetFloat("Health", 0f);
            _dead = true;
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                enemy.GetComponent<EnemyController>().SetPlayerDead(true);
            }
            _gm.SetPlayerDead(true);
        }
    }

    private void CheckGrounded()
    {
        if (_grounded)
        {
            _groundRayDistance = 0.35f;
        }
        else
        {
            _groundRayDistance = 0.15f;
        }

        if (Physics.Raycast(this.transform.position - new Vector3(0f, 0.15f, 0f), -1 * this.transform.up, _groundRayDistance))
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

    public void setMoveDir(Vector3 Destination)
    {
        moveDirection = (Destination - this.transform.position);
        moveDirection = moveDirection.normalized;
        lookDirection = moveDirection;
    }

    public void setLookDir(Vector3 Destination)
    {
        lookDirection = Destination;
    }

    public void setCanMove(bool value)
    {
            canMove = value;
    }

    public Vector3 getLatestRaycastHit()
    {
        return shootRaycastHit;
    }

    public void setShoot(bool value)
    {
        shoot = value;
        //canMove = !value;
        _animator.SetBool("Shoot", shoot);
        if (value == false)
        {
            _muzzleFlash.SetActive(false);
        }
    }
}
