using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonGameManager : MonoBehaviour
{
    public int kills;

    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gm == null)
        {
            gm = FindObjectOfType<GameManager>();
        }
    }

    public void IncrementKills()
    {
        kills++;
        gm.UpdateKills(kills);
    }
}
