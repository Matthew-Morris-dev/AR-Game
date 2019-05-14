using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyToSpawn;
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
        Instantiate(_enemyToSpawn, this.transform.position, Quaternion.identity);
    }
    
}
