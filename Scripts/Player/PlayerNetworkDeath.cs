using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerNetworkDeath : NetworkBehaviour
{
    const string MainMenuScene = "MainMenu";

    [Header("Assignables")]
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private int ownerPriority = 20;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public void EnableDeathUI(ulong id)
    {
        NetworkObject.NetworkShow(id);

        deathScreen.SetActive(true);
        virtualCamera.gameObject.SetActive(true);
        virtualCamera.Priority = ownerPriority;
    }

    // Buttons
    public void ReturnToMenu()
    {
        if (IsHost)
        {
            GameHandler.instance.EndGame();
            NetworkManager.Singleton.Shutdown();
        }

        if (IsClient)
        {
            ReturnClientToMenuServerRPC(NetworkObject.OwnerClientId);
        }

        SceneManager.LoadScene(MainMenuScene);
    }

    [ServerRpc] 
    void ReturnClientToMenuServerRPC(ulong clientId)
    {
        NetworkManager.DisconnectClient(clientId);
    }

    [ClientRpc]
    void ReturnClientToMenuClientRPC(ulong clientId)
    {
        NetworkManager.DisconnectClient(clientId);
    }
}
