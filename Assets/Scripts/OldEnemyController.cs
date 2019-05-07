using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class OldEnemyController : MonoBehaviour
{
    //Movement Stuff
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _stoppingDistance;
    private Vector3 _moveDirection;
    //Attack Stuff
    [SerializeField]
    private float _attackDamage;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _attackCooldown;
    private float _attackTimer;
    //Health Stuff
    [SerializeField]
    private int _currentHealth = 100;
    private int _maxHealth;
    [SerializeField]
    private Image _healthBarImage;

    //[SerializeField]
    //private NavMeshAgent _navAgent;
    [SerializeField]
    private Rigidbody _rb;
    private PlayerController _player;
    private GameManager _gm;
    private float _scale;
    private Vector3 _targetDestination;

    //Debugging
    
    private Text _ARCText;
    private Text _JSCText;
    // Start is called before the first frame update
   
    void Start()
    {
        //Debugging
        //_ARCText = GameObject.Find("PlayerARCText").GetComponent<Text>();
        //_JSCText = GameObject.Find("PlayerJSC").GetComponent<Text>();
        //Set Health
        _maxHealth = _currentHealth;

        //Set Controllers
        _player = FindObjectOfType<PlayerController>();
        if (_player != null)
        {
            _player.SetTarget(this.gameObject);
        }
        /*
        if(_player != null)
        {
            _player.AddEnemy(this.gameObject);
        }
        */
        _gm = FindObjectOfType<GameManager>();
        //_scale = _gm.GetArenaScale();
        //_navAgent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        Debug.Log("Player scale mag: " + _player.transform.localScale.z);
        Debug.Log("this scale mag: " + this.transform.localScale.z);
        //_stoppingDistance = _player.transform.localScale.z + this.transform.localScale.z;
        //_JSCText.text = "_gm = " + _gm;
        //this.transform.localScale = new Vector3(_scale, _scale, _scale) * 0.08f;
        //_speed *= _scale * 0.2f;
        //_stoppingDistance *= _scale * 0.15f;
        //_attackRange = _scale * 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(_gm == null)
        {
            _gm = FindObjectOfType<GameManager>();
            this.transform.localScale = new Vector3(_gm.GetArenaScale(), _gm.GetArenaScale(), _gm.GetArenaScale()) * 0.1f;
            _speed *= _gm.GetArenaScale() * 0.1f;
            _stoppingDistance *= _gm.GetArenaScale() * 0.1f;
            _attackRange = _stoppingDistance * 1.2f;
        }
        if(_ARCText == null)
        {
            _ARCText.text = "_player = null";
        }
        else
        {
            _ARCText.text = "_player =" + _player;
        }
        /*
        if (_JSCText == null)
        {
            _JSCText.text = "_gm = null";
        }
        else
        {
            _JSCText.text = "_attackTimer = " + _attackTimer;
        }
        */
        if (_player != null)
        {
            //_targetDestination = _player.transform.position;
            _moveDirection = _player.transform.position - this.transform.position;
            //_navAgent.SetDestination(_targetDestination);

            //Attack player
            if ((_attackTimer >= _attackCooldown) && (Vector3.Distance(_player.transform.position, this.transform.position) <= _attackRange))
            {
                Attack();
                _attackTimer = 0f;
            }
            else
            {
                _attackTimer += Time.deltaTime;
            }
        }
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(_player.transform.position, this.transform.position) > _stoppingDistance)
        {
            _rb.MovePosition(_rb.position + _moveDirection.normalized * _speed * Time.fixedDeltaTime);
        }
    }

    public void Attack()
    {
        _player.SendMessage("TakeDamage", _attackDamage);
        /*
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, _attackRange))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                hit.transform.gameObject.SendMessage("TakeDamage", _attackDamage);
            }
        }
        */
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        UpdateHealthBar();
        if(_currentHealth <= 0)
        {
            //_gm.SpawnEnemy();
            Death();
        }
    }

    private void Death()
    {
        //_player.RemoveEnemy(this.gameObject);
        Destroy(this.gameObject);
    }

    private void UpdateHealthBar()
    {
        float hpbarFill = (float)_currentHealth / (float)_maxHealth;
        _healthBarImage.fillAmount = hpbarFill;
    }

    public void SetPlayer(GameObject obj)
    {
        _player = obj.GetComponent<PlayerController>();
    }
}
