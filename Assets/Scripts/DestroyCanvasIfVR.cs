using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCanvasIfVR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (OVRManager.isHmdPresent)
        {
            Destroy(this.gameObject);
        }
    }
}
