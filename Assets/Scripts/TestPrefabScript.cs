using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPrefabScript : MonoBehaviour
{
    public GameObject Arena;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Arena, this.transform.position, Quaternion.identity);
        Instantiate(enemy, this.transform.position - new Vector3(7f,0f,0f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
