using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRotation : MonoBehaviour
{
    const float rotationTime = 10f;

    Transform target;

    private void Update()
    {
        HandleRotation();
    }

    public void RotateToTarget(Transform target)
    {
        this.target = target;
    }

    private void HandleRotation()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationTime * Time.deltaTime);
    }
}
