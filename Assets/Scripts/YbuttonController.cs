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
        /*if(_cc == null)
        {
            return;
        }
        else
        {*/
            YbuttonText.text = "Y Button Pressed: true"; 
        //}
    }

    public void SetCharacterController(GameObject obj)
    {
        _cc = obj.GetComponent<characterController>();
    }
}
