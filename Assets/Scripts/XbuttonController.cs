using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class XbuttonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Text XbuttonText;
    [SerializeField]
    private Image XbuttonImage;
    [SerializeField]
    private characterController _player;
    // Start is called before the first frame update
    void Start()
    {
        XbuttonImage.color = Color.white;
        _player = FindObjectOfType<characterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_player == null)
        {
            _player = FindObjectOfType<characterController>();
        }
    }

    public void OnPointerDown(PointerEventData evt)
    {
        XbuttonImage.color = Color.gray;
        _player.setShoot(true);
    }

    public void OnPointerUp(PointerEventData evt)
    {
        XbuttonImage.color = Color.white;
        _player.setShoot(false);
    }

    public void OnPointerEnter(PointerEventData evt)
    {
        XbuttonImage.color = Color.gray;
        _player.setShoot(true);
    }

    public void OnPointerExit(PointerEventData evt)
    {
        XbuttonImage.color = Color.white;
        _player.setShoot(false);
    }
}
