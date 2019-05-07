using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyToSpawn;

    public void SpawnEnemy()
    {
        Instantiate(_enemyToSpawn, this.transform.position, Quaternion.identity);
    }
    
}
