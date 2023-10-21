using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    const float damageDelay = 0.3f;

    [Header("Scripts")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerRotation playerRotation;
    [SerializeField] private PlayerCameraManager playerCameraManager;
    [SerializeField] private GunController gunController;
    [SerializeField] private PlayerAudioController playerAudioController;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerUIManager playerUIManager;

    [Header("References")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerNetworkDeath playerDeathPrefab;

    private Collider playerCollider;

    bool deathScreenSpawned = false;
    bool gameStarted = false;
    float damageTime = 0;

    public PlayerMovement PlayerMovement => playerMovement;
    public PlayerRotation PlayerRotation => playerRotation;
    public PlayerCameraManager PlayerCameraManager => playerCameraManager;
    public GunController GunController => gunController;
    public PlayerAudioController PlayerAudioController => playerAudioController;
    public PlayerStats PlayerStats => playerStats;
    public Collider PlayerCollider => playerCollider;


    public override void OnNetworkSpawn()
    {
        if (!NetworkObject.IsSpawned)
            NetworkObject.Spawn();

        playerCollider = GetComponent<Collider>();
        NetworkInit();
    }

    private void Update()
    {
        CheckRunning();
        HostStartGame();
    }
    public void NetworkPlayerDie()
    {
        // Spawn and Show Death Screen
        if (!deathScreenSpawned)
        {
            if (IsHost)
                SpawnDeathNetworkServerRpc();
            else if (IsClient)
                SpawnDeathNetworkClientRpc();

            deathScreenSpawned = true;
        }

        GameHandler.instance?.PlayerDisconnected();

        if (IsClient)
            DestroyPlayerServerRPC();

        if (IsHost)
            DestroyPlayerClientRPC();
    }

    [ServerRpc(RequireOwnership = true)]
    private void SpawnDeathNetworkServerRpc()
    {
        SpawnDeathNetworkClientRpc();
    }

    [ClientRpc]
    private void SpawnDeathNetworkClientRpc()
    {
        GameObject deathScreenInstance = Instantiate(playerDeathPrefab.gameObject, Vector3.zero, Quaternion.identity);
        deathScreenInstance.GetComponent<PlayerNetworkDeath>().EnableDeathUI(NetworkObject.OwnerClientId);
        deathScreenInstance.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    void DestroyPlayerServerRPC()
    {
        if (NetworkObject != null)
            if (NetworkObject.IsSpawned)
                NetworkObject.Despawn();
        DestroyPlayerClientRPC();
    }

    [ClientRpc]
    void DestroyPlayerClientRPC()
    {
        if (NetworkObject != null)
            if (NetworkObject.IsSpawned)
                NetworkObject.Despawn();
    }

    IEnumerator LateInit(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameHandler.instance.PlayerConnected();
    }

    void CheckRunning()
    {
        if (PlayerMovement.MoveVelocity != Vector3.zero)
        {
            playerAnimator.SetBool("IsWalking", true);
        }
        else
        {
            playerAnimator.SetBool("IsWalking", false);
        }
    }

    private void HostStartGame()
    {
        if (Input.GetKeyDown(KeyCode.F) && IsHost && gameStarted == false)
        {
            gameStarted = true;
            playerUIManager?.DisableDisplayHostUI();
            StartCoroutine(ZombieManager.instance.SpawnZombies());
        }
    }

    private void NetworkInit()
    {
        if (IsHost)
        {
            playerUIManager.UpdateJoinCode(HostSingleton.Instance.GameManager.joinCode);
            playerUIManager.EnableDisplayHostUI();
        }

        playerUIManager.UpdateRegionText(HostSingleton.Instance.GameManager.region);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyLayer") || collision.gameObject.CompareTag("Enemy"))
        {
            if (Time.time - damageTime < damageDelay)
                return; // cooldown

            damageTime = Time.time;

            playerStats.TakeDamage(collision.gameObject.GetComponent<ZombieController>().ZombieStats.ZombieDamage);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyLayer") || collision.gameObject.CompareTag("Enemy"))
        {
            if (Time.time - damageTime < damageDelay)
                return; // cooldown

            damageTime = Time.time;

            playerStats.TakeDamage(collision.gameObject.GetComponent<ZombieController>().ZombieStats.ZombieDamage);
        }
    }
}