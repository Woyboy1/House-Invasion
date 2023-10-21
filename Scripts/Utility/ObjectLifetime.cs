using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLifetime : MonoBehaviour
{
    [SerializeField] private float timeSpawn = 4.5f;

    private void Start()
    {
        Destroy(gameObject, timeSpawn);
    }
}
