using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class XbuttonController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private Text XbuttonText;
    [SerializeField]
    private characterController _cc;
    [SerializeField]
    private PlayerController _pc;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData evt)
    {
        //_cc.Attack();
        _pc.Attack();
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
