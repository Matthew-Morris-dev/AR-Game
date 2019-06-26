using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    #region Public Variables

    #endregion

    #region Private Variables

    [SerializeField]
    private GameObject _loginCanvas;

    [SerializeField]
    private GameObject _lobbyCanvas;

    [SerializeField]
    private Button _startGameButton; //This must only be visable to the MasterClient

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _loginCanvas.SetActive(true);
        _lobbyCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
