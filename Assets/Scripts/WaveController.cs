using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner[] _spawners;

    [SerializeField]
    private bool tutorialOver = false;

    [SerializeField]
    private int enemiesDead = 0;
    [SerializeField]
    private int enemiesAlive = 0;
    [SerializeField]
    private int waveTracker = 0;

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
        else if (tutorialOver)
        {
            if((enemiesAlive == 0) && waveTracker == 0)
            {
                StartWave(1);
                waveTracker++;
            }
            else if((enemiesAlive == 0 && waveTracker == 1))
            {
                StartWave(2);
                waveTracker++;
            }
            else if((enemiesAlive == 0 && waveTracker == 2))
            {
                StartWave(3);
                waveTracker++;
            }
            else if ((enemiesAlive == 0 && waveTracker == 3))
            {
                StartWave(4);
                waveTracker++;
            }
            else if ((enemiesAlive == 0 && waveTracker == 4))
            {
                StartWave(5);
                waveTracker++;
            }
            else if ((enemiesAlive == 0 && waveTracker == 5))
            {
                StartWave(6);
            }
        }
    }

    public void setTutorialOver(bool value)
    {
        tutorialOver = value;
    }
    /*
    public void increamentEnemiesDead()
    {
        enemiesDead++;
    }
    */
    public void decrementEnemiesAlive()
    {
        enemiesAlive--;
    }

    public void StartWave(int waveId)
    {
        if(waveId == 1)
        {
            _spawners[0].SpawnEnemy();
            enemiesAlive = 1;
        }
        else if(waveId == 2)
        {
            _spawners[1].SpawnEnemy();
            _spawners[2].SpawnEnemy();
            enemiesAlive = 2;
        }
        else if(waveId == 3)
        {
            _spawners[0].SpawnEnemy();
            _spawners[1].SpawnEnemy();
            _spawners[2].SpawnEnemy();
            enemiesAlive = 3;
        }
        else if (waveId == 4)
        {
            _spawners[0].SpawnEnemy();
            _spawners[1].SpawnEnemy();
            _spawners[2].SpawnEnemy();
            _spawners[3].SpawnEnemy();
            enemiesAlive = 4;
        }
        else if (waveId == 5)
        {
            _spawners[0].SpawnEnemy();
            _spawners[1].SpawnEnemy();
            _spawners[2].SpawnEnemy();
            _spawners[3].SpawnEnemy();
            _spawners[4].SpawnEnemy();
            enemiesAlive = 5;
        }
        else if (waveId == 6)
        {
            _spawners[0].SpawnEnemy();
            _spawners[1].SpawnEnemy();
            _spawners[2].SpawnEnemy();
            _spawners[3].SpawnEnemy();
            _spawners[4].SpawnEnemy();
            _spawners[5].SpawnEnemy();
            enemiesAlive = 6;
        }
    }
}
