using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : MonoBehaviour
{
    public static PlayerNetwork Instance;

    private string playerName;

    private void Awake()
    {
        Instance = this;
    }

    public void SetName(string text)
    {
        playerName = text;
    }

    public string GetName()
    {
        return playerName;
    }
}
