using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    [SerializeField]
    private float _attackDamage;
    [SerializeField]
    private bool canDoDamage = false;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("we enter here");
        if(canDoDamage == true && other.gameObject.tag == "Player")
        {
            other.GetComponent<characterController>().TakeDamage(_attackDamage);
            canDoDamage = false;
            //Debug.Log("Skeleton Hit Player");
        }
    }

    public void TakeDamage(int damage)
    {
        //hp - damage blah blah
        Debug.Log("we applied damage to the skeletons sword :/");
    }

    public void SetCanDoDamage(bool value)
    {
        canDoDamage = value;
    }
}
