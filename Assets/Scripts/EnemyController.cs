using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _stoppingDistance;
    [SerializeField]
    private int _attackDamage;
    [SerializeField]
    private float _attackRange;
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

    private bool _scaled = false;

    // Start is called before the first frame update
    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
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
            Debug.Log("we run this");
            this.transform.localScale = new Vector3(_gm.GetGameWorldScale() * _xScaleFactor, _gm.GetGameWorldScale() * _yScaleFactor, _gm.GetGameWorldScale() * _zScaleFactor);
            _speed *= _gm.GetGameWorldScale();
            Debug.Log("speed = " + _speed);
            //_animationSpeed *= this.transform.localScale.magnitude;
            _stoppingDistance *= _gm.GetGameWorldScale();
            _attackRange *= _gm.GetGameWorldScale();
            _scaled = true;
        }
        //Find and move towards target
        if (_target == null)
        {
            _target = FindObjectOfType<characterController>().gameObject;
        }
        else
        {
            Vector3 lookinDirection = new Vector3(_target.transform.position.x, this.transform.position.y, _target.transform.position.z);
            this.transform.LookAt(lookinDirection);
            if(Vector3.Distance(this.transform.position, _target.transform.position) <= _attackRange)
            {
                _animator.SetTrigger("Attack");
            }
        }
        

        //Walking animation stuff
        _animator.SetFloat("Speed", _rb.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            if ((Vector3.Distance(this.transform.position, _target.transform.position) <= _stoppingDistance) || this._animator.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
            {
                this._rb.velocity = Vector3.zero;
            }
            else if(!_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
            {
                this._rb.velocity = this.transform.forward * _speed * Time.deltaTime;
                Debug.Log(this.transform.forward);
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

    public void TakeDamage(int damage)
    {
        _animator.speed = _animationSpeed / 2;
        _animator.SetTrigger("TakeDamage");
        _rb.velocity = Vector3.zero;
        //hp - damage blah blah
        Debug.Log(this.gameObject.name + " hit for: " + damage);
    }
}
