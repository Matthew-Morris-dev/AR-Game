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
        Vector3 endDestination = FindObjectOfType<characterController>().getLatestRaycastHit();
        Debug.Log("endDestination: " + endDestination);
        Vector3 temp = endDestination - this.transform.position;
        Vector3 dir = new Vector3(temp.x, 0f, temp.z);
        rb.AddForce(dir * initialForce);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("we are colliding with" + collision.gameObject.name);
        if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else if(collision.gameObject.CompareTag("TutorialEnemy"))
        {
            Debug.Log("we hit dummy");
            collision.gameObject.GetComponent<TutorialEnemy>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else if(collision.gameObject.CompareTag("Arena"))
        {
            Vector3 collisionNormal = collision.contacts[0].normal;
            Debug.Log("Collision normal: " + collisionNormal);
            Quaternion hitWallEffectRotation = Quaternion.LookRotation(collisionNormal);
            Instantiate(wallHitEffect, collision.contacts[0].point, hitWallEffectRotation);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
