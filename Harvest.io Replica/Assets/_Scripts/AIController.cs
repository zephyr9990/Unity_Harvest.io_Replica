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
    [SerializeField] private string respawn;
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float turnSpeed = 5.0f;

    private List<GameObject> grains;
    private List<GameObject> obstacles;
    private Quaternion lookDirection;
    private bool isDead;

    private void Awake()
    {
        lookDirection = Quaternion.identity;
        isDead = false;
    }

    private void Start()
    {
        grains = GameObject.FindGameObjectsWithTag("Grain").ToList();
        tractor.GetComponentInChildren<TractorDeath>().onTractorDeath += Die;

        // Finds a new destination every 2 secs.
        InvokeRepeating("GoToNewDestination", .5f, 3f);
    }

    private void Update()
    {
        if (!isDead)
        {
            // Finds all obstacles to avoid and avoids them
            if (ObstaclesToAvoid())
                lookDirection = GetDirectionAwayFromObstacles();

            MoveForward();
            LookTowardsMovement();
        }
    }

    /// <summary>
    /// Checks to see if there are obstacles to avoid.
    /// </summary>
    /// <returns>True if there are obstacles to avoid, false if not.</returns>
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

            lookDirection = GetMovementTowardsClosestGrain();
        }
    }

    /// <summary>
    /// Turns the object away from obstacles.
    /// </summary>
    /// <returns>A vector that points in the opposite direction of the obstacle.</returns>
    private Quaternion GetDirectionAwayFromObstacles()
    {
        Vector3 vectorSum = Vector3.zero;
        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle == null)
                continue;

            Vector3 toObstacle = obstacle.transform.position - tractor.transform.position;
            vectorSum += toObstacle; // Get resultant vector.
        }

        vectorSum.y = 0;
        Vector3 awayFromObstacles = vectorSum;
        Quaternion targetRotation = Quaternion.LookRotation(awayFromObstacles, Vector3.up);
        
        targetRotation.y *= -(targetRotation.y / targetRotation.y); // Changes the rotation to away from target.
        Debug.Log(targetRotation.ToString());
        
        return targetRotation;
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
        tractor.transform.rotation = Quaternion.Slerp(tractor.transform.rotation, lookDirection, Time.deltaTime * 2);
    }

    /// <summary>
    /// Finds the closest grain and gets a vector towards it.
    /// </summary>
    /// <returns>A vector pointing in the direction of the grain.</returns>
    private Quaternion GetMovementTowardsClosestGrain()
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

            // Do not move towards grains that are behind tractor.
            if (IsBeyondAcceptableTurnAngle(toGrain))
            {
                continue;
            }
            if (toGrain.magnitude < distanceToClosestGrain)
            {
                closestGrain = grains[i];
                distanceToClosestGrain = toGrain.magnitude;
            }
        }
        Vector3 toClosestGrain = closestGrain.transform.position - (tractor.transform.position+ tractor.transform.forward * 5);
        toClosestGrain.y = 0;
        return Quaternion.LookRotation( toClosestGrain, Vector3.up);
    }

    /// <summary>
    /// Checks to see if position is beyond acceptable angle.
    /// </summary>
    /// <param name="position">The position to check.</param>
    /// <returns>True if is beyond acceptable turn angle, false if not. </returns>
    private bool IsBeyondAcceptableTurnAngle(Vector3 position)
    {
        return Vector3.Angle(position, tractor.transform.forward * 5) > 170;
    }

    /// <summary>
    /// Code that is called on death.
    /// </summary>
    private void Die()
    {
        DropAllCargo();
        // Reset score
        CargoController cargoController = GetComponent<CargoController>();
        cargoController.ResetGrainCount();

        Respawn(cargoController);
        Destroy(gameObject);
    }

    /// <summary>
    /// Respawns this AI.
    /// </summary>
    /// <param name="cargoController">The cargoController that holds the playerIndex.</param>
    private void Respawn(CargoController cargoController)
    {
        GameObject spawn = GameObject.FindGameObjectWithTag("Respawn").transform.GetChild(cargoController.GetPlayerIndex() - 1).gameObject;
        Instantiate(Resources.Load(respawn), spawn.transform.position, spawn.transform.rotation);
    }

    /// <summary>
    /// Drops all cargo.
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

