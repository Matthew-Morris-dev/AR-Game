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
    [SerializeField]
    private GameManager _gm;
    // Start is called before the first frame update
    void Start()
    {
        _ARCamera = FindObjectOfType<Camera>().GetComponent<Camera>();
        _rb = GetComponent<Rigidbody>();
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
        _hpBar.fillAmount = _currentHealth / _maxHealth;
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
                shootRaycastHit = hit.point;
            }
            _laserSight.SetLaserSightEnd(shootRaycastHit);

        if (shoot)
        {
            if (_timeSinceLastBullet >= _rateOfFire)
            {
                gunShotSFX.pitch = Random.Range(-.25f, 0.25f) + 1f;
                gunShotSFX.Play();
                PhotonNetwork.Instantiate("bullet_desktop", _bulletEmitter.transform.position, Quaternion.identity, 0);
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

    private void FixedUpdate()
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
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Player recieved damage message");
        _currentHealth -= damage;
        if(_currentHealth <= 0)
        {
            PhotonNetwork.Destroy(this.gameObject);
            PhotonNetwork.Disconnect();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        _gm.UpdateHealth(_currentHealth);
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
