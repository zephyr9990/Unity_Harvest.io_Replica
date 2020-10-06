using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [SerializeField] private GameObject tractor;
    [SerializeField] private ObstacleDetection detectionBox;
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float turnSpeed = 5.0f;

    private List<GameObject> grains;
    private List<GameObject> obstacles;
    private Vector3 movementDirection;
    private bool isDead;

    private void Awake()
    {
        movementDirection = tractor.transform.forward;
        isDead = false;
    }

    private void Start()
    {
        grains = GameObject.FindGameObjectsWithTag("Grain").ToList();
        tractor.GetComponentInChildren<PlayerDeath>().onPlayerDeath += Died;

        // Finds a new destination every 2 secs.
        InvokeRepeating("GoToNewDestination", .5f, 3f);
    }

    private void Update()
    {
        if (!isDead)
        {
            // Finds all obstacles to avoid and avoids them
            if (ObstaclesToAvoid())
                movementDirection = GetDirectionAwayFromObstacles();


            MoveForward();
            LookTowardsMovement();
        }
    }

    private bool ObstaclesToAvoid()
    {
        obstacles = detectionBox.GetObstacles();
        return obstacles.Count > 0;
    }

    /// <summary>
    /// Find a new destination towards grains.
    /// </summary>
    private void GoToNewDestination()
    {
        if (!isDead)
        {
            if (ObstaclesToAvoid())
            {
                return; // Update handles obstacle avoidance.
            }

            movementDirection = GetMovementTowardsClosestGrain();
        }
    }

    /// <summary>
    /// Turns the object away from obstacles.
    /// </summary>
    /// <returns>A vector that points in the opposite direction of the obstacle.</returns>
    private Vector3 GetDirectionAwayFromObstacles()
    {
        GameObject closest = null;
        float distanceToClosest = Mathf.Infinity;
        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle == null)
                continue;

            Vector3 toObstacle = obstacle.transform.position - transform.position;
            if (toObstacle.magnitude < distanceToClosest)
            {
                closest = obstacle;
                distanceToClosest = toObstacle.magnitude;
            }
        }

        Vector3 awayFromObstacle = tractor.transform.position + (tractor.transform.position - closest.transform.position);
        awayFromObstacle.y = 0; // Do not need to move upward or downward.
        return awayFromObstacle.normalized;
    }

    /// <summary>
    /// Moves the tractor forward.
    /// </summary>
    private void MoveForward()
    {
        tractor.transform.Translate(tractor.transform.forward * movementSpeed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// Rotates the tractor towards the desired movement direction.
    /// </summary>
    private void LookTowardsMovement()
    {
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        tractor.transform.rotation = Quaternion.Lerp(tractor.transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }

    /// <summary>
    /// Finds the closest grain and gets a vector towards it.
    /// </summary>
    /// <returns>A vector pointing in the direction of the grain.</returns>
    private Vector3 GetMovementTowardsClosestGrain()
    {
        GameObject closestGrain = null;
        float distanceToClosestGrain = Mathf.Infinity;
        for (int i = 0; i < grains.Count; i++)
        {
            // Remove grains that have been collected.
            if (grains[i] == null)
            {
                grains.RemoveAt(i);
                continue;
            }

            Vector3 toGrain = grains[i].transform.position - (tractor.transform.position + tractor.transform.forward * 5);
            if (toGrain.magnitude < distanceToClosestGrain)
            {
                closestGrain = grains[i];
                distanceToClosestGrain = toGrain.magnitude;
                
            }
        }
        Vector3 toClosestGrain = closestGrain.transform.position - (tractor.transform.position+ tractor.transform.forward *5);
        toClosestGrain.y = 0;
        return toClosestGrain.normalized;
    }

    private void Died()
    {
        isDead = true;
        MeshRenderer[] meshRenderers = tractor.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false;
        }
        Destroy(gameObject, .5f);

    }
}

