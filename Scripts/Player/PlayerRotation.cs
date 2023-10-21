using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerRotation : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;

    void Update()
    {
        if (!IsOwner) return;

        RotatePlayer();
    }

    void RotatePlayer()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Vector3 location = new Vector3(point.x, transform.position.y, point.z);

            transform.LookAt(location);
        }
    }
}
