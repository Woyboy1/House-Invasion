using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;

    [Header("Assignables")]
    [SerializeField] private ZombieManager sceneZombieManager;

    public GameState gameState { get; set; }

    int playerConnected = 0;

    private void Awake()
    {
        instance = this;
    }

    public void ResetPlayersConnection()
    {
        playerConnected = 0;
    }

    public void PlayerConnected()
    {
        playerConnected++;
        sceneZombieManager.AdjustSpawnInterval(playerConnected);
    }

    public void PlayerDisconnected()
    {
        playerConnected--;
        sceneZombieManager.AdjustSpawnInterval(playerConnected);
    }

    public void EndGame()
    {
        gameState = GameState.Over;
    }
}

public enum GameState
{
    Active,
    Over
}
