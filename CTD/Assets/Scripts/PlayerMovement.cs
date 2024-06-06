using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float PlayerAcceleration, PlayerMaxSpeed, PlayerRunSpeed;
    Rigidbody PlayerRigidBody;

    private float horizontalInput, verticalInput, PlayerRunSpeedFake = 1;
    private bool canMove = true;
    Vector3 playerVelocity;

    private void Start()
    {
        PlayerRigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Dash();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        playerVelocity = PlayerRigidBody.velocity;
        playerVelocity.x = Mathf.Clamp(playerVelocity.x, -PlayerMaxSpeed * PlayerRunSpeedFake, PlayerMaxSpeed * PlayerRunSpeedFake);
        playerVelocity.z = Mathf.Clamp(playerVelocity.z, -PlayerMaxSpeed * PlayerRunSpeedFake, PlayerMaxSpeed * PlayerRunSpeedFake);
        playerVelocity.y = 0;
        if (horizontalInput == 0f || verticalInput == 0f)
        {
            PlayerRunSpeedFake = 1;
        }
        PlayerRigidBody.velocity = playerVelocity;

        if (canMove)
        {
            PlayerRigidBody.AddForce(Vector3.right * horizontalInput * PlayerAcceleration * PlayerRunSpeedFake, ForceMode.Force);
            PlayerRigidBody.AddForce(0, 0, verticalInput * PlayerAcceleration * PlayerRunSpeedFake, ForceMode.Force);
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            PlayerRunSpeedFake = PlayerRunSpeed;
        }
    }
}
