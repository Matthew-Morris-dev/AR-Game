using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    [SerializeField]
    private float _attackDamage;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("we enter here");
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<characterController>().TakeDamage(_attackDamage);
            //Debug.Log("Skeleton Hit Player");
        }
    }

    public void TakeDamage(int damage)
    {
        //hp - damage blah blah
        Debug.Log("we applied damage to the skeletons sword :/");
    }
}
