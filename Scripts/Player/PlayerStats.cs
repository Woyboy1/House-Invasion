using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> health = new NetworkVariable<int>(20, readPerm:NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    PlayerController playerController;

    public override void OnNetworkSpawn()
    {
        playerController = GetComponent<PlayerController>();
        base.OnNetworkSpawn();
    }

    public void TakeDamage(int damageAmount)
    {
        if (!IsOwner) return;

        if (health.Value - damageAmount < 0) Die();

        health.Value -= damageAmount;
    }

    public void Die()
    {
        playerController?.PlayerAudioController.PlayClipAtPosition("Scream", transform);
        playerController?.NetworkPlayerDie();
    }
}
