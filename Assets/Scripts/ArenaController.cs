using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    [SerializeField]
    private GameManager _gm;

    private float _size;
    // Start is called before the first frame update
    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _size = _gm.GetArenaScale();
        this.transform.localScale = new Vector3(_size, _size/2, _size) * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
