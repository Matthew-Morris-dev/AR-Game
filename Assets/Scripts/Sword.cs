using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("we enter here");
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("TakeDamage");
            Debug.Log("Skeleton Hit Player");
        }
    }
}
