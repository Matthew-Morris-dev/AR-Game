using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyToSpawn;
    [SerializeField]
    private float spawnDelay;
    [SerializeField]
    private GameObject spawnEffect;
    [SerializeField]
    private GameManager _gm;

    private void Start()
    {
        _gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if(_gm = null)
        {
            _gm = FindObjectOfType<GameManager>();
        }
    }

    public void SpawnEnemy()
    {
        if (PhotonNetwork.isMasterClient)
        {
            GameObject temp = PhotonNetwork.InstantiateSceneObject("SpawnParticleEffect", this.transform.position, Quaternion.identity, 0, null);
            temp.transform.parent = GameObject.Find("World").gameObject.transform;
            Invoke("instantiateEnemy", spawnDelay);
        }
    }

    private void instantiateEnemy()
    {
        GameObject temp = PhotonNetwork.InstantiateSceneObject("Skeleton_desktop", this.transform.position, Quaternion.identity,0,null);
        temp.transform.parent = GameObject.Find("World").gameObject.transform;
    }
    
}
