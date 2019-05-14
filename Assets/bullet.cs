using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float initialForce;
    [SerializeField]
    private float damage;
    [SerializeField]
    private GameObject wallHitEffect;
    [SerializeField]
    private float scaleFactor;
    
    private Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor) * FindObjectOfType<GameManager>().GetGameWorldScale();
        this.transform.Rotate(90f, 0f, 0f);
        Vector3 temp = FindObjectOfType<rayCastIcon>().transform.position;
        Debug.Log("temp: " + temp);
        temp.y = this.transform.position.y;
        Vector3 dir = temp - this.transform.position;

        rb.AddForce(dir * initialForce);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("we are colliding");
        if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else if(collision.gameObject.CompareTag("Arena"))
        {
            //Instantiate(wallHitEffect, collision.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
