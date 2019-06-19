using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEnable : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
