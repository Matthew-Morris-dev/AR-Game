using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyToSpawn;
    //Spawn Locations
    [SerializeField]
    private GameObject _spawnLocation1;
    [SerializeField]
    private GameObject _spawnLocation2;
    [SerializeField]
    private GameObject _spawnLocation3;
    //Manages spawns based on number;
    [SerializeField]
    private int _numberToSpawn;
    [SerializeField]
    private int _numberSpawned;
    [SerializeField]
    private bool usingNumbersToSpawn = false;
    //Manages spawns based on timer
    [SerializeField]
    private float _spawnTimer;
    [SerializeField]
    private float _timeSinceSpawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timeSinceSpawn += Time.deltaTime;
        if(usingNumbersToSpawn)
        {

        }
    }

    public void SpawnEnemySpecificLocation(int location)
    {
        if(location == 1)
        {
            Instantiate(_enemyToSpawn, _spawnLocation1.transform.position, _enemyToSpawn.transform.rotation);
        }
        else if(location == 2)
        {
            Instantiate(_enemyToSpawn, _spawnLocation2.transform.position, _enemyToSpawn.transform.rotation);
        }
        else
        {
            Instantiate(_enemyToSpawn, _spawnLocation3.transform.position, _enemyToSpawn.transform.rotation);
        }
    }

    public void SpawnEnemyRandomLocation()
    {
        int locationDecider = Random.Range(0, 3);
        Vector3 location = Vector3.zero;
        if (locationDecider == 0)
        {
            location = _spawnLocation1.transform.position; 
        }
        else if (locationDecider == 1)
        {
            location = _spawnLocation2.transform.position;
        }
        else
        {
            location = _spawnLocation3.transform.position;
        }
        Instantiate(_enemyToSpawn, location, _enemyToSpawn.transform.rotation);
    }
}
