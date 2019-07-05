using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMe : MonoBehaviour
{
    public PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        if(photonView.isMine)
        {
            this.gameObject.SetActive(false);
        }
    }
}
