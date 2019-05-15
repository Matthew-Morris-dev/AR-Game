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
    private bool[] wave;

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
            if((enemiesDead == 0) && (wave[0] == false))
            {
                StartWave(1);
                wave[0] = true;
            }
            else if((enemiesDead == 1 && wave[1] == false))
            {
                StartWave(2);
                wave[1] = true;
            }
            else if((enemiesDead == 3) && (wave[2] == false))
            {
                StartWave(3);
                wave[2] = true;
            }
            else if ((enemiesDead == 6) && (wave[3] == false))
            {
                StartWave(4);
                wave[3] = true;
            }
            else if ((enemiesDead == 10) && (wave[4] == false))
            {
                StartWave(5);
                wave[4] = true;
            }
            else if ((enemiesDead == 15) && (wave[5] == false))
            {
                StartWave(6);
                wave[5] = true;
            }
        }
    }

    public void setTutorialOver(bool value)
    {
        tutorialOver = value;
    }

    public void increamentEnemiesDead()
    {
        enemiesDead++;
    }

    public void StartWave(int waveId)
    {
        if(waveId == 1)
        {
            _spawners[0].SpawnEnemy();
        }
        else if(waveId == 2)
        {
            _spawners[1].SpawnEnemy();
            _spawners[2].SpawnEnemy();
        }
        else if(waveId == 3)
        {
            _spawners[0].SpawnEnemy();
            _spawners[1].SpawnEnemy();
            _spawners[2].SpawnEnemy();
        }
        else if (waveId == 4)
        {
            _spawners[0].SpawnEnemy();
            _spawners[1].SpawnEnemy();
            _spawners[2].SpawnEnemy();
            _spawners[3].SpawnEnemy();
        }
        else if (waveId == 5)
        {
            _spawners[0].SpawnEnemy();
            _spawners[1].SpawnEnemy();
            _spawners[2].SpawnEnemy();
            _spawners[3].SpawnEnemy();
            _spawners[4].SpawnEnemy();
        }
        else if (waveId == 6)
        {
            _spawners[0].SpawnEnemy();
            _spawners[1].SpawnEnemy();
            _spawners[2].SpawnEnemy();
            _spawners[3].SpawnEnemy();
            _spawners[4].SpawnEnemy();
            _spawners[5].SpawnEnemy();
        }
    }
}
