using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Controller_Desktop : MonoBehaviour
{
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
        if (_gm.GetPaused() == false)
        {
            if (_playerDead == false)
            {
                //Find and move towards target
                if (_target == null)
                {
                    _target = FindObjectOfType<Player_Controller_Desktop>().gameObject;
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

                //Walking animation stuff
                //_animator.SetFloat("Speed", _rb.velocity.magnitude);
            }
            else
            {
                _rb.velocity = Vector3.zero;
                _animator.SetFloat("Speed", 0f);
            }
        }
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
        if (_playerDead == false && _gm.GetPaused() == false)
        {
            _animator.speed = _animationSpeed;
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
        else if(_gm.GetPaused() && _playerDead == false)
        {
            _animator.speed = 0f;
        }
        else if (_gm.GetPaused() && _playerDead)
        {
            _animator.speed = 1f;
        }
    }

    /*
    private void LateUpdate()
    {
        if(!_animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
        {
            _animator.speed = _animationSpeed;
        }
    }
    */

    public void TakeDamage(float damage)
    {
        //_animator.speed = _animationSpeed;
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
        Debug.LogWarning("<color=red>We Execute here 1</color>");
        yield return new WaitForSeconds(2.5f);

        if (WC != null)
        {
            //WC.increamentEnemiesDead();
            WC.decrementEnemiesAlive();
        }
        //this.gameObject.GetComponent<TargetIndicator>().OnDestroy();
        Debug.LogWarning("We Execute here 2");
        _gm.IncrementKills();
        Debug.LogWarning(this.gameObject.name);
        Destroy(this.gameObject);
    }

    public void SetPlayerDead(bool value)
    {
        _playerDead = value;
    }
}
