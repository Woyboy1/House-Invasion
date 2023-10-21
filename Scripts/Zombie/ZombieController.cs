using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ZombieController : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private ZombieMovement zombieMovement;
    [SerializeField] private ZombieStats zombieStats;
    [SerializeField] private ZombieAudioController zombieAudioController;
    [SerializeField] private ZombieRotation zombieRotation;
    [SerializeField] private Animator zombieAnimator;

    [Space(10)]

    [SerializeField] private AudioClip[] zombieDeathSFX;

    [Header("Settings")]
    [SerializeField] private float attackDistance = 2.0f;

    public ZombieMovement ZombieMovement => zombieMovement;
    public ZombieStats ZombieStats => zombieStats;
    public ZombieAudioController ZombieAudioController => zombieAudioController;

    public override void OnNetworkSpawn()
    {

    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
    }

    private void Update()
    {
        CheckDistance();
        HandleRotation();
    }

    private void HandleRotation()
    {
        if (zombieRotation == null) return;

        if (zombieMovement.CurrentTarget != null)
            if (Vector3.Distance(transform.position, zombieMovement.CurrentTarget.position) <= attackDistance)
                zombieRotation.RotateToTarget(zombieMovement.CurrentTarget);
    }

    private void CheckDistance()
    {
        if (zombieMovement.CurrentTarget != null)
        {
            if (Vector3.Distance(transform.position, zombieMovement.CurrentTarget.position) <= attackDistance)
            {
                zombieAnimator.SetBool("IsAttacking", true);
            }
            else
            {
                zombieAnimator.SetBool("IsAttacking", false);
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (zombieStats.Health - damageAmount < 0) Die();

        zombieStats.Health -= damageAmount;
    }

    public void Die()
    {
        int randIndex = Random.Range(0, zombieDeathSFX.Length);
        AudioSource.PlayClipAtPoint(zombieDeathSFX[randIndex], transform.position);
        Destroy(this.gameObject);
    }
}
