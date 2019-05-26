using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEffect : MonoBehaviour
{
    [SerializeField]
    private float duration;
    private float timer = 0f;

    private GameManager _gm;
    private bool scaled = false;

    private void Start()
    {
        _gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if(_gm == null)
        {
            _gm = FindObjectOfType<GameManager>();
        }
        else if(scaled == false)
        {
            this.transform.localScale = new Vector3( _gm.GetGameWorldScale(), _gm.GetGameWorldScale(), _gm.GetGameWorldScale());
            this.transform.parent = null;
            Invoke("DiE", duration);
            scaled = true;
        }
    }
    

    private void DiE()
    {
        Destroy(this.gameObject);
    }
}
