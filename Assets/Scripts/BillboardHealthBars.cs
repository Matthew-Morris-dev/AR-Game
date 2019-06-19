using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;
public class BillboardHealthBars : MonoBehaviour
{
    [SerializeField]
    private Camera _ARCamera;
    // Start is called before the first frame update
    void Start()
    {
        if(_ARCamera == null)
        {
            //_ARCamera = FindObjectOfType<ARCoreSession>().transform.GetChild(0).GetComponent<Camera>();
            _ARCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(_ARCamera.transform.position);
    }
}
