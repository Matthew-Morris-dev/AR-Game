using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstantiationData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.name = GetComponent<PhotonView>().instantiationData[0].ToString();
    }
}