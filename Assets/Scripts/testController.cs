using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testController : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private MobileJoystickController _jsc;
    [SerializeField]
    private Camera _ARCamera;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        _jsc = FindObjectOfType<MobileJoystickController>().GetComponent<MobileJoystickController>();
    }

    void Update()
    {

        Debug.Log("Grounded:" + characterController.isGrounded);
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes
            if (_jsc.GetJoystickActive())
            {
                Vector3 camForward = _ARCamera.transform.forward;
                Vector3 camRight = _ARCamera.transform.right;
                camForward.y = 0f;
                camRight.y = 0f;
                camForward = camForward.normalized;
                camRight = camRight.normalized;

                moveDirection = ((camForward * _jsc.inputDirection.y + camRight * _jsc.inputDirection.x) * speed * Time.deltaTime);
                Debug.Log("moveDir: " + moveDirection);

            }
            else
            {
                moveDirection = Vector3.zero;
            }
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
}

