using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float PlayerAcceleration, PlayerMaxSpeed, PlayerRunSpeed, PlayerRotationSpeed;
    Rigidbody PlayerRigidBody;

    private float horizontalInput, verticalInput, PlayerRunSpeedFake = 1;
    private bool canMove = true;
    public bool inKitchen = true;
    Vector3 playerVelocity, movementInput;

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
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        if (movementInput != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementInput, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, PlayerRotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void MovePlayer()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        movementInput = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (canMove)
        {
            //PlayerRigidBody.AddForce(Vector3.right * horizontalInput * PlayerAcceleration * PlayerRunSpeedFake, ForceMode.Force);
            //PlayerRigidBody.AddForce(Vector3.forward * verticalInput * PlayerAcceleration * PlayerRunSpeedFake, ForceMode.Force);
            PlayerRigidBody.AddForce(movementInput * PlayerAcceleration * PlayerRunSpeedFake, ForceMode.Force);
        }

        //print(playerVelocity.magnitude);
        playerVelocity = PlayerRigidBody.velocity;
        playerVelocity.x = Mathf.Clamp(playerVelocity.x, -PlayerMaxSpeed * PlayerRunSpeedFake * Mathf.Abs(movementInput.x), PlayerMaxSpeed * PlayerRunSpeedFake * Mathf.Abs(movementInput.x));
        playerVelocity.z = Mathf.Clamp(playerVelocity.z, -PlayerMaxSpeed * PlayerRunSpeedFake * Mathf.Abs(movementInput.z), PlayerMaxSpeed * PlayerRunSpeedFake * Mathf.Abs(movementInput.z));
        playerVelocity.y = 0;
        //if (horizontalInput == 0f || verticalInput == 0f)
        //{
        //    PlayerRunSpeedFake = 1;
        //}
        if (horizontalInput == 0)
        {
            playerVelocity.x *= 0.5f;
        }
        if (verticalInput == 0)
        {
            playerVelocity.z *= 0.5f;
        }
        PlayerRigidBody.velocity = playerVelocity;


    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            PlayerRunSpeedFake = PlayerRunSpeed;

        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            PlayerRunSpeedFake = 1;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Door"))
        {
            Debug.Log("door");
            inKitchen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            Debug.Log("door");
            inKitchen = false;
        }
    }
}
