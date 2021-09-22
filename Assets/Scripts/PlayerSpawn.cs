using System;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class PlayerSpawn : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    public override void NetworkStart()
    {
        Debug.Log("Network Start");
        SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(ulong clientId)
    {
        Debug.Log("ihjoij");
        var spawnedPlayer = Instantiate(playerPrefab);
        var spawnedPlayerNetworkObject = spawnedPlayer.GetComponent<NetworkObject>();
        spawnedPlayerNetworkObject.SpawnAsPlayerObject(clientId);
    }
}