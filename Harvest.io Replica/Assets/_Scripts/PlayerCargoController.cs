using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCargoController : MonoBehaviour
{
    [SerializeField] private TractorCollector tractorCollector;
    [SerializeField] private GameObject cargo;
    [SerializeField] private GameObject initialCargoSpawnPosition;

    private GameObject lastCargoSpawned;
    private GameObject nextCargoSpawnPosition;
    private int numOfGrainsCollected;

    private void Awake()
    {
        lastCargoSpawned = null;
        nextCargoSpawnPosition = initialCargoSpawnPosition;
        numOfGrainsCollected = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        tractorCollector.onGrainCollected += IncrementGrainCount;
    }

    private void IncrementGrainCount()
    {
        numOfGrainsCollected++;
        if (CollectedEnoughToSpawnCargo())
        {
            SpawnCargo();
        }
    }

    private bool CollectedEnoughToSpawnCargo()
    {
        return numOfGrainsCollected % 20 == 0;
    }

    private void SpawnCargo()
    {
        lastCargoSpawned = Instantiate(
                        cargo,
                        nextCargoSpawnPosition.transform.position,
                        nextCargoSpawnPosition.transform.rotation
                        );

        CargoMovement cargoMovement = lastCargoSpawned.GetComponent<CargoMovement>();
        cargoMovement.SetFollowTarget(nextCargoSpawnPosition);
        nextCargoSpawnPosition = cargoMovement.GetNextCargoPosition();
    }
}
