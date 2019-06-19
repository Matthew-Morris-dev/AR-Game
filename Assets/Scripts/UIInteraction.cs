using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInteraction : MonoBehaviour
{
    public bool leftController;
    public LayerMask rayMask;
    public LineRenderer LR;
    // Start is called before the first frame update
    void Start()
    {
        LR.SetPosition(0, this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        LR.SetPosition(0, this.transform.position);
        RaycastHit hit;
        Debug.DrawRay(this.transform.position, this.transform.forward,Color.blue);
        if(Physics.Raycast(this.transform.position, this.transform.forward, out hit))
        {
            //LR.SetPosition(1, hit.point);
            LR.SetPosition(1, this.transform.position+ this.transform.forward * 10);
            Debug.LogWarning(hit.collider.name);
            if (hit.collider.GetComponent<Button>())
            {
                Debug.LogWarning("Yo, I see it");
                if ((OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && (!leftController))|| ((OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && (leftController))))
                {
                    hit.collider.GetComponent<Button>().onClick.Invoke();
                }
            }
        }
        else
            LR.SetPosition(1, this.transform.position + this.transform.forward * 10);

    }
}
