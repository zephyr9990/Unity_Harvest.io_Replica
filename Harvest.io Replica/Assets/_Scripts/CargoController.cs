using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Analytics;

public class CargoController : MonoBehaviour
{
    [SerializeField] private GameObject grainCollector;
    [SerializeField] private int playerIndex;
    [SerializeField] private GrainCollector tractorCollector;
    [SerializeField] private GameObject cargo;
    [SerializeField] private GameObject initialCargoSpawnPosition;
    [SerializeField] private int numOfGrainsForCargo = 30;
    private int numOfGrainsToLevelUp;

    private GameObject lastCargoSpawned;
    private Scoreboard scoreboard;
    private GameObject nextCargoSpawnPosition;
    private int numOfGrainsCollected;
    private int level = 1;
    private int maxLevel = 3;

    private void Awake()
    {
        lastCargoSpawned = null;
        nextCargoSpawnPosition = initialCargoSpawnPosition;
        numOfGrainsCollected = 0;
        numOfGrainsToLevelUp = numOfGrainsForCargo * 5;
    }

    void Start()
    {
        tractorCollector.onGrainCollected += IncrementGrainCount;

        scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<Scoreboard>();

    }

    public void ResetGrainCount()
    {
        numOfGrainsCollected = 0;
        scoreboard.UpdateScore(playerIndex, numOfGrainsCollected);
    }

    /// <summary>
    /// Increments the grain count and if enough grain is collected,
    /// spawns an additional cargo.
    /// </summary>
    private void IncrementGrainCount()
    {
        numOfGrainsCollected++;
        scoreboard.UpdateScore(playerIndex, numOfGrainsCollected);
        if (CollectedEnoughToSpawnCargo())
        {
            SpawnCargo();
        }

        if (level == maxLevel)
        {
            return;
        }

        if (CollectedEnoughToLevelUp())
        {
            LevelUp();
        }
    }

    /// <summary>
    /// Levels up the tractor and increases the grain collector size.
    /// </summary>
    private void LevelUp()
    {
        grainCollector.transform.localScale = new Vector3(grainCollector.transform.localScale.x + 1, 1f, 1f);
        numOfGrainsToLevelUp += numOfGrainsToLevelUp;
        level++;
    }

    /// <summary>
    /// Checks to see if the player has collected enough to level up.
    /// </summary>
    /// <returns>True if player has collected enough to level up, false if not.</returns>
    private bool CollectedEnoughToLevelUp()
    {
        return numOfGrainsCollected % numOfGrainsToLevelUp == 0;
    }

    /// <summary>
    /// Sees if the player has collected enough grains to spawn cargo.
    /// </summary>
    /// <returns>True if player has collected enough, or false if not.</returns>
    private bool CollectedEnoughToSpawnCargo()
    {
        return numOfGrainsCollected % numOfGrainsForCargo == 0;
    }

    /// <summary>
    /// Spawns a cargo.
    /// </summary>
    private void SpawnCargo()
    {
        if (nextCargoSpawnPosition)
        {
            if (nextCargoSpawnPosition != initialCargoSpawnPosition)
                nextCargoSpawnPosition.transform.GetChild(0).gameObject.SetActive(true);
            lastCargoSpawned = Instantiate(cargo, nextCargoSpawnPosition.transform.position, nextCargoSpawnPosition.transform.rotation);
            lastCargoSpawned.transform.parent = transform;

            // Initialize behavior by setting follow target.
            CargoMovement cargoMovement = lastCargoSpawned.GetComponent<CargoMovement>();
            cargoMovement.SetFollowTarget(nextCargoSpawnPosition);


            // Set next spawn location of the next cargo to spawn.
            nextCargoSpawnPosition = cargoMovement.GetNextCargoPosition();
        }
    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    }
}
