using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner[] _spawners;

    private GameManager _gm;
    private bool _spawned;
    // Start is called before the first frame update
    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gm == null)
        {
            _gm = FindObjectOfType<GameManager>();
        }
        else if (_spawned == false)
        {
            for(int i = 0; i < _spawners.Length; i++)
            {
                _spawners[i].SpawnEnemy();
            }
            _spawned = true;
        }
    }
}
