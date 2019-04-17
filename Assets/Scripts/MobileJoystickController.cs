using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileJoystickController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Image _joystickBackground;
    [SerializeField]
    private Image _joystick;

    public Vector3 inputDirection;
    private bool _joystickActive = false;

    // Start is called before the first frame update
    void Start()
    {
        _joystickBackground = GetComponent<Image>();
        _joystick = this.transform.GetChild(0).GetComponent<Image>();
        inputDirection = Vector3.zero;
    }

    // Update is called once per frame
    public void OnDrag(PointerEventData evt)
    {
        Vector2 pos = Vector2.zero;

        //get input direction
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickBackground.rectTransform, evt.position, evt.pressEventCamera, out pos);
        pos.x = /*evt.position.x - _joystickBackground.rectTransform.anchoredPosition.x;*/(pos.x / _joystickBackground.rectTransform.sizeDelta.x);
        pos.y = /*evt.position.y - _joystickBackground.rectTransform.anchoredPosition.y;*/(pos.y / _joystickBackground.rectTransform.sizeDelta.y);

        float x = pos.x * 2;//(_joystickBackground.rectTransform.pivot.x == 1f) ? pos.x * 2 + 1 : pos.x * 2 - 1;
        float y = pos.y * 2;//(_joystickBackground.rectTransform.pivot.y == 1f) ? pos.y * 2 + 1 : pos.y * 2 - 1;

        inputDirection = new Vector3(x, y, 0f);
        inputDirection = (inputDirection.magnitude > 1) ? inputDirection.normalized : inputDirection;

        _joystick.rectTransform.anchoredPosition = new Vector2(inputDirection.x * (_joystickBackground.rectTransform.sizeDelta.x/3),inputDirection.y * (_joystickBackground.rectTransform.sizeDelta.y/3));
    }

    public void OnPointerDown(PointerEventData evt)
    {
        OnDrag(evt);
        _joystickActive = true;
    }

    public void OnPointerUp(PointerEventData evt)
    {
        inputDirection = Vector3.zero;
        _joystick.rectTransform.anchoredPosition = Vector3.zero;
        _joystickActive = false;
    }

    public bool GetJoystickActive()
    {
        return _joystickActive;
    }
}
