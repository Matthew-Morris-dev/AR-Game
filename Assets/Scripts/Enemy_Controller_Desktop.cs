using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Controller_Desktop : MonoBehaviour
{
    [SerializeField]
    private PhotonView photonView;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _stoppingDistance;
    [SerializeField]
    private float _attackDamage;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private float damageDelay;
    private float initialDamageDelay;
    [SerializeField]
    private float delayTimer = 0f;
    [SerializeField]
    private AudioSource hitSFX;
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _animationSpeed;
    [SerializeField]
    private GameObject _target;

    //Scaling Stuff
    [SerializeField]
    private float _xScaleFactor;
    [SerializeField]
    private float _yScaleFactor;
    [SerializeField]
    private float _zScaleFactor;
    [SerializeField]
    private GameManager _gm;

    //Health stuff
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private float _currentHealth;
    [SerializeField]
    private Image _hpBar;

    private bool _scaled = false;
    private bool _playerDead = false;


    public PhotonGameManager PGM;
    //Wave Spawning Stuff
    [SerializeField]
    private WaveController WC;
    // Start is called before the first frame update
    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        WC = FindObjectOfType<WaveController>();
        _currentHealth = _maxHealth;
        _animator.SetFloat("Health", _currentHealth);
        _rb.drag = 0f;
        _rb.angularDrag = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (WC == null)
        {
            WC = FindObjectOfType<WaveController>();
        }
        if(PGM == null)
        {
            PGM = FindObjectOfType<PhotonGameManager>();
        }

        //Find GM or scale
        if (_gm == null)
        {
            _gm = FindObjectOfType<GameManager>();
        }
        else if (_scaled == false)
        {
            //Debug.Log("we run this");
            this.transform.localScale = new Vector3(_gm.GetGameWorldScale() * _xScaleFactor, _gm.GetGameWorldScale() * _yScaleFactor, _gm.GetGameWorldScale() * _zScaleFactor);
            _speed *= _gm.GetGameWorldScale() * 0.1f; //set speed scale to the same as the players scale
            //_animationSpeed *= _gm.GetGameWorldScale() * 1f;
            //_animator.speed = _animationSpeed;
            //damageDelay = damageDelay * (_animationSpeed);
            initialDamageDelay = damageDelay;
            _stoppingDistance *= _gm.GetGameWorldScale();
            _attackRange *= _gm.GetGameWorldScale();
            _scaled = true;
        }
        MakeMeATarget[] listOfPossibleTargets = FindObjectsOfType<MakeMeATarget>();
        //Find and move towards target
                if (_target == null)
                {
                    if (PhotonNetwork.isMasterClient)
                    {
                        string theTarget = listOfPossibleTargets[Mathf.FloorToInt(Random.Range(0, listOfPossibleTargets.Length))].gameObject.name;
                        photonView.RPC("SetTarget", PhotonTargets.All, theTarget);
                    }
                }
                else
                {
                    Vector3 lookinDirection = new Vector3(_target.transform.position.x, this.transform.position.y, _target.transform.position.z);
                    this.transform.LookAt(lookinDirection);
                    //Debug.Log("Look direction" + lookinDirection);
                    if (Vector3.Distance(this.transform.position, _target.transform.position) <= _attackRange)
                    {
                        _animator.SetTrigger("Attack");
                        //_animator.speed = _animationSpeed;
                    }
                    else
                    {
                        _animator.SetFloat("Speed", _speed);
                    }
                }

                _hpBar.fillAmount = _currentHealth / _maxHealth;

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (delayTimer >= damageDelay)
            {
                sword.GetComponent<BoxCollider>().enabled = true;
                sword.GetComponent<Sword_Desktop>().SetCanDoDamage(true);
                delayTimer = 0f;
                damageDelay = 10f;
            }
            else
            {
                delayTimer += Time.deltaTime;
            }
        }
        else
        {
            sword.GetComponent<BoxCollider>().enabled = false;
            sword.GetComponent<Sword_Desktop>().SetCanDoDamage(false);
            delayTimer = 0f;
            damageDelay = initialDamageDelay;
        }
    }

    private void FixedUpdate()
    {
        if (_playerDead == false)
        {
            if (_target != null)
            {
                if ((Vector3.Distance(this.transform.position, _target.transform.position) <= _stoppingDistance) || this._animator.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
                {
                    this._rb.velocity = Vector3.zero;
                }
                else if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
                {
                    Vector3 movDir = (new Vector3(_target.transform.position.x, 0f, _target.transform.position.z) - new Vector3(this.transform.position.x, 0f, this.transform.position.z));
                    movDir = movDir.normalized;
                    this.transform.Translate(movDir * _speed * Time.fixedDeltaTime, Space.World);
                }
            }
        }
    }
    /*
    public void TakeDamage(float damage)
    {
        Debug.LogError("We enter here");
        photonView.RPC("TellAllITakeDamage", PhotonTargets.All, damage);
    }

    [PunRPC]*/
    public void TakeDamage(float damage)
    {
        _animator.SetTrigger("TakeDamage");
        if (hitSFX.isPlaying == false)
        {
            hitSFX.Play();
        }
        _rb.velocity = Vector3.zero;
        _currentHealth -= damage;
        Debug.Log(this.gameObject.name + " hit for: " + damage);
        Debug.Log("Skeleton Current HP: " + this._currentHealth);
        if (this.gameObject.GetComponent<Enemy_Controller_Desktop>()._currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        _animator.SetFloat("Health", _currentHealth);
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        if (WC != null)
        {
            WC.decrementEnemiesAlive();
        }
        this.gameObject.GetComponent<TargetIndicator>().OnDestroy();
        if (PGM != null)
        {
            PGM.IncrementKills();
        }
        _gm.IncrementKills();
        PhotonNetwork.Destroy(this.gameObject);
    }

    [PunRPC]
    private void SetTarget(string obj)
    {
        _target = GameObject.Find(obj);
    }

    [PunRPC]
    private void SetLookDir(Vector3 lookDir)
    {
        this.transform.LookAt(lookDir);
    }

    public void SetPlayerDead(bool value)
    {
        _playerDead = value;
    }
}
