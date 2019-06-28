using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller_Desktop : MonoBehaviour
{
    public PhotonView photonView;
    [SerializeField]
    private Camera myCamera;
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private GameObject _muzzleFlash;
    [SerializeField]
    private GameObject _muzzleFlash2;
    [SerializeField]
    private float _animationSpeed;
    //Target Stuff
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

    //Fps gun
    [SerializeField]
    private GameObject playerGun;

    //movement
    [SerializeField]
    private float _movementSpeed;
    private float _movementX;
    private float _movementZ;


    //Health stuff
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private float _currentHealth;

    [SerializeField]
    private float _jumpSpeed;
    [SerializeField]
    private bool _grounded = true;
    private float _groundRayDistance;

    [SerializeField]
    private GameManager _gm;

    private bool cameraRotate = false;
    [SerializeField]
    private float cameraRotationSpeed;
    private bool _dead = false;

    public LayerMask camLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _gm = FindObjectOfType<GameManager>();
        _currentHealth = _maxHealth;
        _gm.UpdateHealth(_currentHealth);
        _animator.SetFloat("Health", _currentHealth);
        photonView.RPC("UpdateName", PhotonTargets.All, PlayerNetwork.Instance.GetName());
    }

    // Update is called once per frame
    void Update()
    {
        //Find GM or scale
        if (_gm == null)
        {
            _gm = FindObjectOfType<GameManager>();
        }
        if (Time.timeScale != 0f)
        {
            if (_dead == false)
            {
                _movementX = Input.GetAxis("Horizontal");
                _movementZ = Input.GetAxis("Vertical");
                moveDirection = new Vector3(_movementX, 0f, _movementZ);

                if (Input.GetMouseButton(0))
                {
                    setShoot(true);
                }
                else
                {
                    setShoot(false);
                }
                /*
                 * insert shooting code here

                */
                //Blend animations for moving diagonally
                float forwardMovement = ((Vector3.Dot(this.transform.forward, _rb.velocity)) / (this.transform.forward.magnitude));
                float rightMovement = ((Vector3.Dot(this.transform.right, _rb.velocity)) / (this.transform.right.magnitude));
                _animator.SetFloat("Zmovement", forwardMovement);
                _animator.SetFloat("Xmovement", rightMovement);

                //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookDirection), _rotationSpeed * Time.deltaTime);
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    shootRaycastHit = hit.point;
                }

                if (shoot)
                {
                    if (_timeSinceLastBullet >= _rateOfFire)
                    {
                        gunShotSFX.pitch = Random.Range(-.25f, 0.25f) + 1f;
                        gunShotSFX.Play();
                        PhotonNetwork.Instantiate("bullet_desktop", _bulletEmitter.transform.position, Quaternion.identity, 0);
                        _muzzleFlash.SetActive(true);
                        _muzzleFlash2.SetActive(true);
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
                    _muzzleFlash2.SetActive(false);
                    _timeSinceLastBullet += Time.deltaTime;
                }
            }
            else
            {
                _rb.freezeRotation = true;
                _muzzleFlash.SetActive(false);
                _muzzleFlash2.SetActive(false);
            }
        }
        if(cameraRotate)
        {
            Camera.main.transform.RotateAround(this.transform.position, Vector3.up, cameraRotationSpeed * Time.deltaTime);
            Camera.main.transform.LookAt(this.transform.position);
        }
    }

    private void FixedUpdate()
    {
        if (_dead == false)
        {
            if(moveDirection.magnitude >= 0.1f)
            {
                transform.Translate(moveDirection * _movementSpeed * Time.deltaTime, Space.Self);
                _animator.SetTrigger("Walking");
            }
            else
            {
                _animator.SetTrigger("Idle");
            }
            if (!shoot)
            {
                canMove = true;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Player recieved damage message");
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            //if (PhotonNetwork.isMasterClient)
            //{
            //    int wave = FindObjectOfType<WaveController>().CurrentWave();
            //    FindObjectOfType<WaveController>().UpdateWaveTracker(wave);
            //}
            PhotonNetwork.Disconnect();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            //_currentHealth = 0f;
            //_animator.SetFloat("Health", 0f);
            //if(!_dead)
            //{
            //    DeathCameraAnimation();
            //}
            //_dead = true;
            //foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            //{
            //    enemy.GetComponent<Enemy_Controller_Desktop>().SetPlayerDead(true);
            //}
            //_gm.SetPlayerDead(true);
            //playerGun.SetActive(false);
            
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
            _muzzleFlash2.SetActive(false);
        }
    }

    private void DeathCameraAnimation()
    {
        Camera.main.cullingMask += camLayerMask;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.up + -2*Camera.main.transform.forward, 5f);
        Camera.main.transform.LookAt(this.transform.position);
        cameraRotate = true;
    }

    public void DisableMyCamera()
    {
        myCamera.gameObject.GetComponent<Look>().enabled = false;
        myCamera.gameObject.GetComponent<AudioListener>().enabled = false;
        myCamera.enabled = false;
    }

    [PunRPC]
    private void UpdateName(string name)
    {
        this.gameObject.name = name;
    }
}
