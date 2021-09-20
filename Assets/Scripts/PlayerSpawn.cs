using System;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class PlayerSpawn : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(ulong clientId)
    {
        var spawnedPlayer = Instantiate(playerPrefab);
        var spawnedPlayerNetworkObject = spawnedPlayer.GetComponent<NetworkObject>();
        spawnedPlayerNetworkObject.SpawnAsPlayerObject(clientId);
    }
}