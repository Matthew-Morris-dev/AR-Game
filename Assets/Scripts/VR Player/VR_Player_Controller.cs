using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VR_Player_Controller : MonoBehaviour
{
    public PhotonView photonView;
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private GameObject _muzzleFlash;
    [SerializeField]
    private LaserSight_Desktop _laserSight;
    [SerializeField]
    private float _animationSpeed;
    //Target Stuff
    [SerializeField]
    private Camera shootRaycastFrom;
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

    public Transform cameraRef;

    //movement
    [SerializeField]
    private float _movementSpeed;
    private float _movementX;
    private float _movementZ;
    [SerializeField]
    private float rotationSpeed;


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
    [SerializeField]
    private TMP_Text HealthText;
    [SerializeField]
    private TMP_Text KillsText;

    public LayerMask camLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _gm = FindObjectOfType<GameManager>();
        _currentHealth = _maxHealth;
        UpdateHealth(_currentHealth);
        _animator.SetFloat("Health", _currentHealth);
        _laserSight.gameObject.SetActive(true);
        if (PhotonNetwork.isMasterClient && photonView.isMine)
        {
            PhotonNetwork.InstantiateSceneObject("PhotonGameManager", Vector3.zero, Quaternion.identity, 0, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Find GM or scale
        if (_gm == null)
        {
            _gm = FindObjectOfType<GameManager>();
        }
        else
        {
            UpdateKills(_gm.GetKills());
        }
            RaycastHit hit;
            if (Physics.Raycast(_bulletEmitter.transform.position, _bulletEmitter.transform.forward, out hit))
            {
                shootRaycastHit = hit.point;
            }
            else
            {
                shootRaycastHit = Vector3.zero;
            }
            _laserSight.SetLaserSightEnd(shootRaycastHit);

            Vector2 movementXY = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            _movementX = movementXY.x;
            _movementZ = movementXY.y;

            moveDirection = (movementXY.y * Camera.main.transform.forward + movementXY.x * Camera.main.transform.right);
            moveDirection.y = 0f;

        Vector2 rotationXY = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        if(rotationXY.magnitude != 0f)
        {
            Debug.LogError(rotationXY.x * rotationSpeed);
        }
        transform.Rotate(Vector3.up, rotationXY.x * rotationSpeed);

            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                setShoot(true);
            }
            else
            {
                setShoot(false);
            }

            float forwardMovement = ((Vector3.Dot(this.transform.forward, _rb.velocity)) / (this.transform.forward.magnitude));
            float rightMovement = ((Vector3.Dot(this.transform.right, _rb.velocity)) / (this.transform.right.magnitude));
            _animator.SetFloat("Zmovement", forwardMovement);
            _animator.SetFloat("Xmovement", rightMovement);
        
            if (shoot)
            {
                if (_timeSinceLastBullet >= _rateOfFire)
                {
                    gunShotSFX.pitch = Random.Range(-.25f, 0.25f) + 1f;
                    gunShotSFX.Play();
                    GameObject temp = PhotonNetwork.Instantiate("bullet_desktop", _bulletEmitter.transform.position, Quaternion.identity, 0);
                temp.transform.parent = GameObject.Find("World").gameObject.transform;
                _muzzleFlash.SetActive(false);
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
                _timeSinceLastBullet += Time.deltaTime;
            }
    }

    private void FixedUpdate()
    {
            if (moveDirection.magnitude >= 0.1f)
            {
                transform.Translate(moveDirection * _movementSpeed * Time.deltaTime, Space.World);
                cameraRef.position = new Vector3(this.transform.position.x, cameraRef.transform.position.y, this.transform.position.z);
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

    public void TakeDamage(float damage)
    {
        Debug.Log("Player recieved damage message");
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            PhotonNetwork.Disconnect();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        UpdateHealth(_currentHealth);
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

    public void EnableLaserSight()
    {
        _laserSight.gameObject.SetActive(true);
    }

    private void DeathCameraAnimation()
    {
        Camera.main.cullingMask += camLayerMask;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.up + -2 * Camera.main.transform.forward, 5f);
        Camera.main.transform.LookAt(this.transform.position);
    }

    private void UpdateHealth(float hp)
    {
        HealthText.text = "Health: " + hp;
    }

    private void UpdateKills(int kills)
    {
        KillsText.text = "Kills: " + kills;
    }
}
