using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInstantiationData : MonoBehaviour
{
    public TextMeshPro floatingName;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.name = GetComponent<PhotonView>().instantiationData[0].ToString();
        floatingName.text = GetComponent<PhotonView>().instantiationData[0].ToString();
    }
}