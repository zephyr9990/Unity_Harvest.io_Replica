﻿using System;
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
    private TimeHandler timeHandler;

    private Rigidbody playerRigidBody;
    private Vector3 movementDirection;
    private Vector3 lastMovementDirection;
    private bool playerDead;

    private void Awake()
    {
        timeHandler = GameObject.FindGameObjectWithTag("TimeHandler").GetComponent<TimeHandler>();
        playerRigidBody = player.GetComponent<Rigidbody>();
        movementDirection = player.transform.forward;
        lastMovementDirection = player.transform.forward;
        playerDead = false;
    }

    private void Start()
    {
        player.GetComponentInChildren<TractorDeath>().onTractorDeath += PlayerDied;
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

    /// <summary>
    /// Continuously moves the player forward at a set speed.
    /// </summary>
    private void MoveForward()
    {
        player.transform.Translate(player.transform.forward * movementSpeed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// Rotates towards the intended movement direction.
    /// </summary>
    private void LookTowardsMovement()
    {
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }

    /// <summary>
    /// Gets the movement direction based on the joystick input.
    /// </summary>
    /// <returns>A vector containing the desired direction of travel
    /// based on input from the joystick.</returns>
    private Vector3 GetMovementInput()
    {
        Vector3 movement = new Vector3(joystick.Horizontal, 0.0f, joystick.Vertical);
        if (movement == Vector3.zero)
            return lastMovementDirection; // Continue moving in last movement direction.

        lastMovementDirection = movement;
        return movement;
    }

    /// <summary>
    /// The player has died.
    /// </summary>
    private void PlayerDied()
    {
        GetComponent<CargoController>().ResetGrainCount();
        DropAllCargo();
        timeHandler.EndGame();
        Destroy(gameObject);
    }

    /// <summary>
    /// Drops all carried cargo.
    /// </summary>
    private void DropAllCargo()
    {
        CargoMovement[] cargoMovements = transform.GetComponentsInChildren<CargoMovement>();
        foreach (CargoMovement cargoMovement in cargoMovements)
        {
            cargoMovement.DropCargo();
        }
    }
}
