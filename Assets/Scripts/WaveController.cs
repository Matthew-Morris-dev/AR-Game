﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField]
    private PhotonView photonView;

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
    [SerializeField]
    private AudioSource spawnSFX;
    [SerializeField]
    private float waveDelay;

    private bool waveStarted = false;

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

        Enemy_Controller_Desktop[] Enemies = FindObjectsOfType<Enemy_Controller_Desktop>();
            if((Enemies.Length == 0) && waveTracker == 0 && waveStarted == false)
            {
                waveStarted = true;
                spawnSFX.Play();
                StartCoroutine(StartWave(1, waveDelay));
                waveTracker++;
                photonView.RPC("AnnounceWaveToAllClients", PhotonTargets.All, waveTracker);
            }
            else if((Enemies.Length == 0 && waveTracker == 1) && waveStarted == false)
            {
                waveStarted = true;
                spawnSFX.Play();
                StartCoroutine(StartWave(2, waveDelay));
                waveTracker++;
                photonView.RPC("AnnounceWaveToAllClients", PhotonTargets.All, waveTracker);
            }
            else if((Enemies.Length == 0 && waveTracker == 2) && waveStarted == false)
            {
                waveStarted = true;
                spawnSFX.Play();
                StartCoroutine(StartWave(3, waveDelay));
                waveTracker++;
                photonView.RPC("AnnounceWaveToAllClients", PhotonTargets.All, waveTracker);
            }
            else if ((Enemies.Length == 0 && waveTracker == 3) && waveStarted == false)
            {
                waveStarted = true;
                spawnSFX.Play();
                StartCoroutine(StartWave(4, waveDelay));
                waveTracker++;
                photonView.RPC("AnnounceWaveToAllClients", PhotonTargets.All, waveTracker);
            }
            else if ((Enemies.Length == 0 && waveTracker == 4) && waveStarted == false)
            {
                waveStarted = true;
                spawnSFX.Play();
                StartCoroutine(StartWave(5, waveDelay));
                waveTracker++;
                photonView.RPC("AnnounceWaveToAllClients", PhotonTargets.All, waveTracker);
            }
            else if ((Enemies.Length == 0 && waveTracker >= 5) && waveStarted == false)
            {
                waveStarted = true;
                spawnSFX.Play();
                StartCoroutine(StartWave(6, waveDelay));
                waveTracker++;
                photonView.RPC("AnnounceWaveToAllClients", PhotonTargets.All, waveTracker);
            }
            else if(Enemies.Length > 0)
            {
                waveStarted = false;
            }
    }



    public void setTutorialOver(bool value)
    {
        tutorialOver = value;
    }

    public void decrementEnemiesAlive()
    {
        enemiesAlive--;
    }

    IEnumerator StartWave(int waveId, float delay)
    {
        yield return new WaitForSeconds(delay);
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

    [PunRPC]
    public void AnnounceWaveToAllClients(int wave)
    {
        _gm.AnnounceWave(wave);
    }

    public void UpdateWaveTracker(int currentWave)
    {
        photonView.RPC("UpdateWaveTrackerRPC", PhotonTargets.All, currentWave);
    }

    [PunRPC]
    private void UpdateWaveTrackerRPC(int value)
    {
        waveTracker = value;
    }
}
