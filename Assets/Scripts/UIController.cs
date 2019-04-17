using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameManager _gm;
    [SerializeField]
    private Player _p;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnemyToTotem()
    {
        GameObject obj = _gm.GetEnemy(0);
        _p.SetSpawnObject(obj);
    }
}
