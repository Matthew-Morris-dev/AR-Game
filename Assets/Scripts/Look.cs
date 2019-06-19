using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour {
    public enum RotationAxis
    {
        MouseX = 1,
        MouseY
    }

    public RotationAxis axis = RotationAxis.MouseX;

    public float minYRot = -45.0f;
    public float maxYRot = 45.0f;

    public float ySensitivity = 10.0f;
    public float xSensitivity = 10.0f;

    private float _rotX = 0;

    private GameManager _gm;

    private void Start()
    {
        _gm = FindObjectOfType<GameManager>();
    }
    // Update is called once per frame
    void Update () {
        if (_gm == null)
        {

        }
        else
        {
                if (axis == RotationAxis.MouseX)
                {
                    Vector2 movementXY = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
                    transform.Rotate(0, movementXY.x * xSensitivity, 0f);
                }
                else if (axis == RotationAxis.MouseY)
                {
                    _rotX -= Input.GetAxis("Mouse Y") * ySensitivity;
                    _rotX = Mathf.Clamp(_rotX, minYRot, maxYRot);

                    float rotY = transform.localEulerAngles.y;

                    transform.localEulerAngles = new Vector3(_rotX, rotY, 0f);
                }
        }
	}
}
