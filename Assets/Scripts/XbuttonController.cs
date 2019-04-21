using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class XbuttonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Text XbuttonText;
    [SerializeField]
    private Image XbuttonImage;
    [SerializeField]
    private characterController _cc;
    [SerializeField]
    private PlayerController _pc;
    // Start is called before the first frame update
    void Start()
    {
        XbuttonImage.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData evt)
    {
        XbuttonImage.color = Color.gray;
        _pc.Attack();
    }

    public void OnPointerUp(PointerEventData evt)
    {
        XbuttonImage.color = Color.white;
    }

    public void SetPlayerController(GameObject obj)
    {
        _pc = obj.GetComponent<PlayerController>();
    }

    public void SetCharacterController(GameObject obj)
    {
        _cc = obj.GetComponent<characterController>();
    }
}
