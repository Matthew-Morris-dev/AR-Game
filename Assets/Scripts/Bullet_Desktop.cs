using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Desktop : MonoBehaviour
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
    private GameObject enemyHitEffect;
    [SerializeField]
    private float scaleFactor;

    public Player_Controller_Desktop myPlayer;
    public VR_Player_Controller myVRPlayer;
    private Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        if (OVRManager.isHmdPresent)
        {
            myVRPlayer = FindObjectOfType<VR_Player_Controller>();
            //this.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor) * FindObjectOfType<GameManager>().GetGameWorldScale();
            this.transform.Rotate(90f, 0f, 0f);
            Vector3 endDestination = myVRPlayer.getLatestRaycastHit();
            //Debug.Log("endDestination: " + endDestination);
            Vector3 temp = endDestination - this.transform.position;
            // Vector3 dir = new Vector3(temp.x, 0f, temp.z);
            Vector3 dir = endDestination - this.transform.position;
            rb.AddForce(dir * initialForce);
        }
        else
        {
            myPlayer = FindObjectOfType<Player_Controller_Desktop>();
            //this.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor) * FindObjectOfType<GameManager>().GetGameWorldScale();
            this.transform.Rotate(90f, 0f, 0f);
            Vector3 endDestination = myPlayer.getLatestRaycastHit();
            //Debug.Log("endDestination: " + endDestination);
            Vector3 temp = endDestination - this.transform.position;
            // Vector3 dir = new Vector3(temp.x, 0f, temp.z);
            Vector3 dir = endDestination - this.transform.position;
            rb.AddForce(dir * initialForce);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("we are colliding with" + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy_Controller_Desktop>().TakeDamage(damage);
            Instantiate(enemyHitEffect, collision.collider.bounds.center, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Arena"))
        {
            Vector3 collisionNormal = collision.contacts[0].normal;
            Debug.Log("Collision normal: " + collisionNormal);
            Quaternion hitWallEffectRotation = Quaternion.LookRotation(collisionNormal);
            //PhotonNetwork.Instantiate("HitWallEffect", collision.contacts[0].point, hitWallEffectRotation,0);
            Instantiate(wallHitEffect, collision.contacts[0].point, hitWallEffectRotation);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
