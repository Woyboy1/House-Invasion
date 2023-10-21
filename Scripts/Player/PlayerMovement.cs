using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 6f;

    Rigidbody rb;
    Vector3 moveInput;
    Vector3 moveVelocity;

    public Vector3 MoveVelocity => moveVelocity;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!IsOwner) return;

        HandleInputs();
        LimitSpeed();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        Move();
    }

    void HandleInputs()
    {
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * movementSpeed;
    }

    private void Move()
    {
        // rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        rb.AddForce(moveVelocity.normalized * movementSpeed * 10f, ForceMode.Force);
    }

    void LimitSpeed()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit Velocity
        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 limitdVel = flatVel.normalized * movementSpeed;
            rb.velocity = new Vector3(limitdVel.x, rb.velocity.y, limitdVel.z);
        }
    }
}