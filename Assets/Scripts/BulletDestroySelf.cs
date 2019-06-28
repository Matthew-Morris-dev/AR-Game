using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroySelf : MonoBehaviour
{
    public float destroyTime = 0f;
    public float delayTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        destroyTime = Time.time + delayTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= destroyTime)
        {
            Destroy(this);
        }
    }
}
