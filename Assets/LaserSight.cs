using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRender;
    [SerializeField]
    private float _scaleFactor;
    [SerializeField]
    private GameManager _gm;

    private bool _scaled = false;
    // Start is called before the first frame update
    void Update()
    {
        if (_gm == null)
        {
            _gm = FindObjectOfType<GameManager>();
        }
        else if (_scaled == false)
        {
            Debug.Log(_gm.GetGameWorldScale() * _scaleFactor);
            lineRender.startWidth = _gm.GetGameWorldScale() * _scaleFactor;
            lineRender.endWidth = _gm.GetGameWorldScale() * _scaleFactor;
            _scaled = true;
        }
        lineRender.SetPosition(0,(this.GetComponentInParent<Transform>().position + new Vector3(0f, -1f, -1.1f) * _scaleFactor * _gm.GetGameWorldScale()));
    }

    public void SetLaserSightEnd(Vector3 destination)
    {
        lineRender.SetPosition(1, destination);
    }
}
