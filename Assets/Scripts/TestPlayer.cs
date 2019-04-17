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
    void Start()
    {
        _jsc = FindObjectOfType<MobileJoystickController>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 camForward = _mainCamera.transform.forward;
        Vector3 camRight = _mainCamera.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward = camForward.normalized;
        camRight = camRight.normalized;
        
        Vector3 vel = ((camForward * _jsc.inputDirection.y + camRight * _jsc.inputDirection.x) * speed * Time.deltaTime);
        _rb.velocity = vel;
    }
}
