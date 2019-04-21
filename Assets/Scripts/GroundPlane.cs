using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlane : MonoBehaviour
{
    private GameManager _gm;
    // Start is called before the first frame update
    void Start()
    {
        _gm = FindObjectOfType<GameManager>();

        this.transform.localScale = new Vector3(_gm.detectedPlane.ExtentX * 0.1f, 1f, _gm.detectedPlane.ExtentZ * 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
