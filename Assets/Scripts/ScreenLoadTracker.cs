using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLoadTracker : MonoBehaviour
{
    [SerializeField]
    private bool usedEscape = false;

    public void SetHowWeChangedScene(bool value)
    {
        usedEscape = value;
    }

    public bool GetHowWeChangedScene()
    {
        return usedEscape;
    }
}
