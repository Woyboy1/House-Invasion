using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Netcode;
using UnityEngine;

public class ZombieManager : NetworkBehaviour
{
    public static ZombieManager instance;

    [Header("References")]
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private GameObject zombiePrefab;

    private float spawnTimerInterval = 5.5f;
    private const float timeMultiplier = 1.0f;

    bool canSpawn = true;
    public bool CanSpawn
    {
        get { return canSpawn; }
        set { canSpawn = value; }
    }

    public float SpawnTimerInterval
    {
        get { return spawnTimerInterval; }
        set { spawnTimerInterval = value; }
    }

    private void Awake()
    {
        instance = this;
    }

    public void AdjustSpawnInterval(int playersConnected)
    {
        float adjustedTimeInterval = spawnTimerInterval / playersConnected + timeMultiplier;
        spawnTimerInterval = adjustedTimeInterval;
        // Debug.Log("Spawn Interval: " + spawnTimerInterval);
    }

    public IEnumerator SpawnZombies()
    {
        WaitForSeconds delay = new WaitForSeconds(spawnTimerInterval);

        while (canSpawn && GameHandler.instance.gameState != GameState.Over)
        {
            if (!IsServer) yield return null;

            int randSpawn = Random.Range(0, spawnLocations.Length);

            GameObject zombieInstace = Instantiate(zombiePrefab, spawnLocations[randSpawn]);
            NetworkObject zombieObject = zombieInstace.GetComponent<NetworkObject>();

            zombieObject.Spawn();

            yield return delay;

        }
    }
}
