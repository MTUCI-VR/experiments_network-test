using System.Collections;
using MLAPI;
using UnityEngine;

public class ClientInitialization : MonoBehaviour
{
    [SerializeField] private PlayerSpawn playerSpawn;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(1);
        playerSpawn.SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
    }
}