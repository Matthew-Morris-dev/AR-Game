using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerListing : MonoBehaviour
{
    public PhotonPlayer PhotonPlayer { get; private set; }

    [SerializeField]
    private TMP_Text _playerName;
    private TMP_Text PlayerName
    {
        get { return _playerName; }
    }

    public void ApplyPhotonPLayer(PhotonPlayer photonPlayer)
    {
        PhotonPlayer = photonPlayer;
        PlayerName.text = photonPlayer.NickName;
    }
}
