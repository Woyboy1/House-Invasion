using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GunController : NetworkBehaviour
{
    [Header("Assignables")]
    [SerializeField] private GameObject clientBulletPrefab;
    [SerializeField] private GameObject serverBulletPrefab;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private GameObject muzzleFlashVFX;

    [Header("Settings")]
    [SerializeField] private float fireRate;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float muzzleFlashDuration = 0.3f;

    private float attackTime;
    private float muzzleFlashTimer;
    private PlayerController playerController;
    private bool canFire = true;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (muzzleFlashTimer > 0)
        {
            muzzleFlashTimer -= Time.deltaTime;

            if (muzzleFlashTimer <= 0)
            {
                muzzleFlashVFX.SetActive(false);
            }
        }

        if (!IsOwner) return;

        if (Input.GetMouseButton(0))
            ShootGun();
    }

    public void ShootGun()
    {
        if (!canFire) return;

        // Firerate
        if (Time.time - attackTime < fireRate)
            return; // cooldown

        attackTime = Time.time;

        // Audio
        playerController.PlayerAudioController.PlayLocalSFX("Gunshot");

        PrimaryFireServerRPC(shotPoint.position);
        SpawnDummyProjectile(shotPoint.position);
    }

    private void SpawnDummyProjectile(Vector3 spawnPos)
    {
        GameObject clientProjectileClone = Instantiate(clientBulletPrefab, spawnPos, Quaternion.identity);
        muzzleFlashVFX.SetActive(true);
        muzzleFlashTimer = muzzleFlashDuration;

        clientProjectileClone.transform.forward = shotPoint.transform.forward;

        if (clientProjectileClone.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = rb.transform.forward * projectileSpeed;
        }
    }

    [ServerRpc]
    private void PrimaryFireServerRPC(Vector3 spawnPos)
    {
        GameObject serverProjectileClone = Instantiate(serverBulletPrefab, spawnPos, Quaternion.identity);

        serverProjectileClone.transform.forward = shotPoint.transform.forward;

        if (serverProjectileClone.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = rb.transform.forward * projectileSpeed;
        }

        SpawnDummyProjectileClientRPC(shotPoint.position);
    }

    // actual visuals for the player:
    [ClientRpc]
    private void SpawnDummyProjectileClientRPC(Vector3 spawnPos)
    {
        if (IsOwner) return;

        SpawnDummyProjectile(spawnPos);
    }
}
