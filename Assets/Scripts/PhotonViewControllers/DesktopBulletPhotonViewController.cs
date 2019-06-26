using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopBulletPhotonViewController : MonoBehaviour
{
    [SerializeField]
    private PhotonView photonView;

    private void Start()
    {
        if (!photonView.isMine)
        {
            this.gameObject.GetComponent<Bullet_Desktop>().enabled = false;
        }
    }
}
