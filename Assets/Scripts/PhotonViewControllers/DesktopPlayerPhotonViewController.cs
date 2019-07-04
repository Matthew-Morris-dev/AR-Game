using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopPlayerPhotonViewController : Photon.PunBehaviour
{
    [SerializeField]
    private PhotonView photonView;

    public LayerMask defaultLayer;

    [SerializeField]
    private GameObject fpsBarrel;
    [SerializeField]
    private GameObject fpsGun;
    [SerializeField]
    private GameObject tpsBarrel;
    [SerializeField]
    private GameObject tpsGun;

    [SerializeField]
    private GameObject ARlaserSight;

    private void Start()
    {
        if (!photonView.isMine)
        {
            if (this.gameObject.GetComponent<Player_Controller_Desktop>())
            {
                this.gameObject.GetComponent<Look>().enabled = false;
                this.gameObject.GetComponent<AudioSource>().enabled = false;
                this.gameObject.GetComponent<Player_Controller_Desktop>().DisableMyCamera();
                SetLayerRecursively(this.gameObject, defaultLayer);
                Destroy(this.gameObject.GetComponent<Player_Controller_Desktop>());
            }
            else if(this.gameObject.GetComponent<VR_Player_Controller>())
            {
                this.gameObject.transform.GetChild(this.gameObject.transform.childCount - 1).gameObject.SetActive(false);
                this.gameObject.GetComponent<AudioSource>().enabled = false;
                SetLayerRecursively(this.gameObject, defaultLayer);
                Destroy(this.gameObject.GetComponent<VR_Player_Controller>());
            }
            else if (this.gameObject.GetComponent<characterController>())
            {
                ARlaserSight.SetActive(false);
                Destroy(this.gameObject.GetComponent<characterController>());
            }
        }

        //test
        if (photonView.isMine && !Application.isMobilePlatform)
        {
            fpsBarrel.SetActive(true);
            fpsGun.SetActive(true);
            tpsBarrel.SetActive(false);
            tpsGun.SetActive(false);
        }
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
