using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class YbuttonController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private Text YbuttonText;
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
        //_cc.Dodge();
        if (_pc.GetIsGrounded())
        {
            _pc.Dodge();
        }
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
