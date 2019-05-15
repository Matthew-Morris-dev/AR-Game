using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialEnemy : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _animationSpeed;

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

    //Health stuff
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private float _currentHealth;
    [SerializeField]
    private Image _hpBar;
    [SerializeField]
    private Text _dummyText;

    //End Tutorial Stuff
    [SerializeField]
    private WaveController WC;

    // Start is called before the first frame update
    private void Awake()
    {
        //_gm = FindObjectOfType<GameManager>();
        WC = FindObjectOfType<WaveController>();
        _currentHealth = _maxHealth;
        _animator.SetFloat("Health", _currentHealth);
    }

    void Start()
    {
        //_gm = FindObjectOfType<GameManager>();
        _currentHealth = _maxHealth;
        _animator.SetFloat("Health", _currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //Find GM or scale
        if (_gm == null)
        {
            _gm = FindObjectOfType<GameManager>();
        }
        else if (_scaled == false)
        {
            this.transform.localScale = new Vector3(_gm.GetGameWorldScale() * _xScaleFactor, _gm.GetGameWorldScale() * _yScaleFactor, _gm.GetGameWorldScale() * _zScaleFactor);
            _scaled = true;
        }
        */
        if(WC == null)
        {
            WC = FindObjectOfType<WaveController>();
        }

        _hpBar.fillAmount = _currentHealth / _maxHealth;
        _rb.velocity = Vector3.zero;
        _animator.SetFloat("Speed", _rb.velocity.magnitude);
    }

    private void LateUpdate()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
        {
            _animator.speed = _animationSpeed;
        }
    }


    public void TakeDamage(float damage)
    {
        _animator.speed = _animationSpeed / 2;
        _animator.SetTrigger("TakeDamage");
        _rb.velocity = Vector3.zero;
        this._currentHealth -= damage;
        Debug.Log(this.gameObject.name + " hit for: " + damage);
        if (this.gameObject.GetComponent<TutorialEnemy>()._currentHealth <= 0)
        {
            _dummyText.enabled = false;
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        _animator.SetFloat("Health", _currentHealth);
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        WC.setTutorialOver(true);
        Destroy(this.gameObject);
    }
}
