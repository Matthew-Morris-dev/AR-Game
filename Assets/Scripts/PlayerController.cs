using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject _target;
    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        //Always face enemy
        if (_target != null)
        {
            this.gameObject.transform.LookAt(_target.transform.position);
        }
    }
}
