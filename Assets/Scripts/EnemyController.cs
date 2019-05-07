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
            this.transform.localScale = new Vector3(_gm.GetGameWorldScale() * _xScaleFactor, _gm.GetGameWorldScale() * _yScaleFactor, _gm.GetGameWorldScale() * _zScaleFactor);
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
            if (Vector3.Distance(this.transform.position, _target.transform.position) <= _stoppingDistance)
            {
                _rb.velocity = Vector3.zero;
            }
            else if(!_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                _rb.velocity = this.transform.forward * _speed * Time.deltaTime;
            }
        }
    }

    public void TakeDamage()
    {
        _animator.SetTrigger("Hit");
        Debug.Log("Enemy took damage from players bullet");
    }
}
