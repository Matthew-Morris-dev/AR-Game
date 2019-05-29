using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTracker : MonoBehaviour
{
    [SerializeField]
    private bool usedEscape = true;
    [SerializeField]
    private bool fastSkipMainMenu = false;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetFastSkipMainMenu(bool value)
    {
        fastSkipMainMenu = value;
    }

    public bool GetFastSkipMainMenu()
    {
        return fastSkipMainMenu;
    }

    public void SetUsedEscape(bool value)
    {
        usedEscape = value;
    }

    public bool GetUsedEscape()
    {
        return usedEscape;
    }
}
