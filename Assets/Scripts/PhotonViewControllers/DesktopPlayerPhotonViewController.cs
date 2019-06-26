using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopPlayerPhotonViewController : Photon.PunBehaviour
{
    [SerializeField]
    private PhotonView photonView;

    private void Start()
    {
        if (!photonView.isMine)
        {
            this.gameObject.GetComponent<Look>().enabled = false;
            this.gameObject.GetComponent<AudioSource>().enabled = false;
            this.gameObject.GetComponent<Player_Controller_Desktop>().DisableMyCamera();
            Destroy(this.gameObject.GetComponent<Player_Controller_Desktop>());
        }
    }
}
