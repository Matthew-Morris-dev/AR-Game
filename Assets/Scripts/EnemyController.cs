using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
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
    [SerializeField]
    private float delayTimer = 0f;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(WC == null)
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
            Debug.Log("we run this");
            this.transform.localScale = new Vector3(_gm.GetGameWorldScale() * _xScaleFactor, _gm.GetGameWorldScale() * _yScaleFactor, _gm.GetGameWorldScale() * _zScaleFactor);
            _speed *= _gm.GetGameWorldScale();
            Debug.Log("speed = " + _speed);
            //_animationSpeed *= this.transform.localScale.magnitude;
            _stoppingDistance *= _gm.GetGameWorldScale();
            _attackRange *= _gm.GetGameWorldScale();
            _scaled = true;
        }
        if (_playerDead == false)
        {
            //Find and move towards target
            if (_target == null)
            {
                _target = FindObjectOfType<characterController>().gameObject;
            }
            else
            {
                Vector3 lookinDirection = new Vector3(_target.transform.position.x, this.transform.position.y, _target.transform.position.z);
                this.transform.LookAt(lookinDirection);
                if (Vector3.Distance(this.transform.position, _target.transform.position) <= _attackRange)
                {
                    _animator.SetTrigger("Attack");
                }
            }

            _hpBar.fillAmount = _currentHealth / _maxHealth;

            //Walking animation stuff
            _animator.SetFloat("Speed", _rb.velocity.magnitude);
        }
        else
        {
            _rb.velocity = Vector3.zero;
            _animator.SetFloat("Speed", _rb.velocity.magnitude);
        }

        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if(delayTimer >= damageDelay)
            {
                sword.GetComponent<BoxCollider>().enabled = true;
                sword.GetComponent<Sword>().SetCanDoDamage(true);
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
            sword.GetComponent<Sword>().SetCanDoDamage(false);
            delayTimer = 0f;
            damageDelay = 0.8f;
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
                    this._rb.velocity = this.transform.forward * _speed * Time.deltaTime;
                    Debug.Log(this.transform.forward);
                }
            }
        }
    }

    
    private void LateUpdate()
    {
        if(!_animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
        {
            _animator.speed = _animationSpeed;
        }
    }
    

    public void TakeDamage(float damage)
    {
        _animator.speed = _animationSpeed / 2;
        _animator.SetTrigger("TakeDamage");
        _rb.velocity = Vector3.zero;
        _currentHealth -= damage;
        Debug.Log(this.gameObject.name + " hit for: " + damage);
        if(this.gameObject.GetComponent<EnemyController>()._currentHealth <= 0)
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
            //WC.increamentEnemiesDead();
            WC.decrementEnemiesAlive();
        }
        Destroy(this.gameObject);
    }

    public void SetPlayerDead(bool value)
    {
        _playerDead = value;
    }
}
