using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class TrainingTotem : MonoBehaviour
{
    [SerializeField]
    private int _hitPoints = int.MaxValue;
    //private int _armour = 100;
    [SerializeField]
    private GameObject _centerOfMass;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int value)
    {
        _hitPoints -= value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
    }
}
