using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Joystick joystick;
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float turnSpeed = 5.0f;

    private Rigidbody playerRigidBody;
    private Vector3 movementDirection;
    private Vector3 lastMovementDirection;
    private bool playerDead;

    private void Awake()
    {
        playerRigidBody = player.GetComponent<Rigidbody>();
        movementDirection = player.transform.forward;
        lastMovementDirection = player.transform.forward;
        playerDead = false;
    }

    private void Start()
    {
        player.GetComponent<PlayerDeath>().onPlayerDeath += PlayerDied;
    }

    private void Update()
    {
        if (!playerDead)
        {
            movementDirection = GetMovementInput();
            LookTowardsMovement();
            MoveForward();
        } 
    }

    private void MoveForward()
    {
        player.transform.Translate(player.transform.forward * movementSpeed * Time.deltaTime, Space.World);
    }

    private void LookTowardsMovement()
    {
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }

    private Vector3 GetMovementInput()
    {
        Vector3 movement = new Vector3(joystick.Horizontal, 0.0f, joystick.Vertical);
        if (movement == Vector3.zero)
            return lastMovementDirection;

        lastMovementDirection = movement;
        return movement;
    }

    private void PlayerDied()
    {
        playerDead = true;
        MeshRenderer[] meshRenderers = player.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false;
        }
    }
}
