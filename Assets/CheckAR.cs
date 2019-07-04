using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!Application.isMobilePlatform)
        {
            Destroy(this.gameObject);
        }
    }
}
