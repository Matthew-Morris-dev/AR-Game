using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Desktop : MonoBehaviour
{

    [SerializeField]
    private float _attackDamage;
    [SerializeField]
    private bool canDoDamage = false;
    [SerializeField]
    private GameObject playerDamageEffect;
    [SerializeField]
    private AudioSource playerHitSFX;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("we enter here");
        if(canDoDamage == true && other.gameObject.tag == "Player")
        {
            other.GetComponent<Player_Controller_Desktop>().TakeDamage(_attackDamage);
            canDoDamage = false;
            playerHitSFX.Play();
            Instantiate(playerDamageEffect, other.ClosestPointOnBounds(this.transform.position), Quaternion.identity);
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
