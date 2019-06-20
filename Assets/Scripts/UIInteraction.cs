using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInteraction : MonoBehaviour
{
    public bool leftController;
    public LayerMask rayMask;
    public LineRenderer LR;
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        LR.SetPosition(0, this.transform.position);
        if(gm == null)
        {
            gm = FindObjectOfType<GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.GetPaused())
        {
            LR.enabled = true;
            LR.SetPosition(0, this.transform.position);
            RaycastHit hit;
            Debug.DrawRay(this.transform.position, this.transform.forward, Color.blue);
            if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 100f, rayMask))
            {
                //LR.SetPosition(1, hit.point);
                LR.SetPosition(1, this.transform.position + this.transform.forward * 50);
                if (hit.collider.GetComponent<Button>())
                {
                    if ((OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && (!leftController)) || ((OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && (leftController))))
                    {
                        hit.collider.GetComponent<Button>().onClick.Invoke();
                    }
                }
            }
            else
            {
                LR.SetPosition(1, this.transform.position + this.transform.forward * 50);
            }
        }
        else
        {
            LR.enabled = false;
        }
    }
}
