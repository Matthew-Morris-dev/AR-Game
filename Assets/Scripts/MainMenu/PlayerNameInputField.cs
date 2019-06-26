using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    static string playerNamePrefKey = "PlayerName";

    // Start is called before the first frame update
    void Start()
    {
        string defaultName = "";
        TMP_InputField _inputField = this.GetComponent<TMP_InputField>();
        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }

        PhotonNetwork.playerName = defaultName;
    }

    public void SetPlayerName(string value)
    {
        PlayerNetwork.Instance.SetName(value);
        PhotonNetwork.playerName = value + " ";// force a trailing space, in case alue is empty string, else playername would not be updated
        PlayerPrefs.SetString(playerNamePrefKey, value);
    }
}
