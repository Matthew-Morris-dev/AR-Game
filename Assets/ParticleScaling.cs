using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScaling : MonoBehaviour
{
    public Vector3 initialScale;

    private void Start()
    {
        initialScale = this.transform.localScale;
    }

    private void Update()
    {
        this.gameObject.transform.localScale = new Vector3 (this.transform.parent.parent.localScale.x * initialScale.x, this.transform.parent.parent.localScale.y * initialScale.y, this.transform.parent.parent.localScale.z * initialScale.z);
    }
}
