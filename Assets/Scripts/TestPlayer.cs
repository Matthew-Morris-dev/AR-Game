using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera;
    [SerializeField]
    private float speed;
    [SerializeField]
    private MobileJoystickController _jsc;
    [SerializeField]
    private Rigidbody _rb;
    // Start is called before the first frame update
    private bool _grounded = true;
    private bool _falling;
    private float _maxJumpHeight = 3.0f;
    [SerializeField]
    private float _jumpSpeed = 9.0f;
    private float _fallSpeed = 12.0f;
    private float _groundHeight;
    void Start()
    {
        _jsc = FindObjectOfType<MobileJoystickController>();
        _rb = GetComponent<Rigidbody>();
        _groundHeight = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetMouseButtonDown(0))
        {
            Dodge();
            Debug.Log("Jumping");
        }*/
    }

    private void FixedUpdate()
    {
        if (_grounded)
        {
            if (_jsc.GetJoystickActive())
            {
                Vector3 camForward = _mainCamera.transform.forward;
                Vector3 camRight = _mainCamera.transform.right;
                camForward.y = 0f;
                camRight.y = 0f;
                camForward = camForward.normalized;
                camRight = camRight.normalized;

                Vector3 vel = ((camForward * _jsc.inputDirection.y + camRight * _jsc.inputDirection.x) * speed * Time.deltaTime);
                _rb.velocity = vel;
                Debug.Log("velocity vector:" + vel);
            }
            else
            {
                _rb.velocity = Vector3.zero;
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, Vector3.down, out hit, (this.transform.localScale.y / 2 + 0.05f)))
            {
                if (hit.transform.gameObject.tag == "Arena")
                {
                    _grounded = true;
                }
            }
        }
    }

    public void Dodge()
    {
        _rb.AddForce(Vector3.up * _jumpSpeed + Vector3.back * _jumpSpeed);
        _grounded = false;
        //Vector3 DodgeRoll = this.transform.forward * -1 * _dodgeForce;
        //_rb.AddForce(DodgeRoll);
        //_falling = false;
        //_grounded = false;
        //StartCoroutine("JumpBackwards");
    }
    /*
    IEnumerator JumpBackwards()
{
    while (true)
    {
        if (this.transform.position.y >= _maxJumpHeight)
        {
            _falling = true;
        }
        if (_falling == false)
        {
            this.transform.Translate(Vector3.up * _jumpSpeed * Time.smoothDeltaTime);
        }
        else if (_falling)
        {
            this.transform.Translate(Vector3.down * _fallSpeed * Time.smoothDeltaTime);
            if (this.transform.position.y < _groundHeight)
            {
                Vector3 tempPos = this.transform.position;
                tempPos.y = _groundHeight;
                this.transform.position = tempPos;
                _grounded = true;
                StopAllCoroutines();
            }
        }

        yield return new WaitForEndOfFrame();
    }
}*/
}
