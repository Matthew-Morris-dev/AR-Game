using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayoutGroup : Photon.PunBehaviour
{
    [SerializeField]
    private GameObject _playerListingPrefab;
    private GameObject PlayerListingPrefab
    {
        get { return _playerListingPrefab; }
    }

    [SerializeField]
    private PhotonView photonView;

    private List<PlayerListing> _playerListings = new List<PlayerListing>();
    private List<PlayerListing> PlayerListings
    {
        get { return _playerListings; }
    }

    private void OnJoinedRoom()
    {
        Debug.LogWarning("a player has joined the room");
        Debug.LogWarning(PhotonNetwork.playerList);
        PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;
        for(int i = 0; i < photonPlayers.Length; i++)
        {
            photonView.RPC("PlayerJoinedRoom", PhotonTargets.All, photonPlayers[i]);
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer photonPlayer)
    {
        PlayerLeftRoom(photonPlayer);
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer)
    {
        PlayerLeftRoom(photonPlayer);
    }

    [PunRPC]
    private void PlayerJoinedRoom(PhotonPlayer photonPlayer)
    {
        if(photonPlayer == null)
        {
            return;
        }

        PlayerLeftRoom(photonPlayer);

        GameObject playerListingObj = Instantiate(PlayerListingPrefab);
        playerListingObj.transform.SetParent(transform, false);

        PlayerListing playerListing = playerListingObj.GetComponent<PlayerListing>();
        playerListing.ApplyPhotonPLayer(photonPlayer);

        PlayerListings.Add(playerListing);
    }

    private void PlayerLeftRoom(PhotonPlayer photonPlayer)
    {
        int index = PlayerListings.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if(index != -1)
        {
            Destroy(PlayerListings[index].gameObject);
            PlayerListings.RemoveAt(index);
        }
    }
}
