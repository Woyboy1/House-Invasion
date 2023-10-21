using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float movementSync = 0.15f;
    [SerializeField] private float targetSync = 4.0f;

    private Transform currentTarget;
    private NavMeshAgent agent;

    public Transform CurrentTarget => currentTarget;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(FindAndSetNearestTarget());
        StartCoroutine(SyncMovement());
    }

    IEnumerator FindAndSetNearestTarget()
    {
        WaitForSeconds targetInterval = new WaitForSeconds(targetSync);

        while (true)
        {
            FindNearestTarget();
            yield return targetInterval;
        }
    }

    private void FindNearestTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = float.MaxValue;

        if (players == null) return;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTarget = player.transform;
            }
        }
    }

    IEnumerator SyncMovement()
    {
        WaitForSeconds movementInterval = new WaitForSeconds(movementSync);

        while (true)
        {
            if (currentTarget != null)
            {
                agent.SetDestination(currentTarget.position);
            }
            yield return movementInterval;
        }
    }
}
