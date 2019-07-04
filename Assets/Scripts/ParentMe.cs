using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentMe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 initialScale = this.transform.localScale;
        this.transform.parent = GameObject.Find("World").gameObject.transform;
        this.transform.localScale = initialScale;
    }
}
