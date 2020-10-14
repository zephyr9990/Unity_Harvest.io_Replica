using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetection : MonoBehaviour
{
    private List<GameObject> obstacles;
    private void Awake()
    {
        obstacles = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tractor")
            || (other.CompareTag("Cargo")))
        {
            obstacles.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tractor")
            || other.CompareTag("Cargo") ||
            other.CompareTag("Bounds"))
        {
            obstacles.Remove(other.gameObject);
        }
    }

    /// <summary>
    /// Get all detected obstacles.
    /// </summary>
    /// <returns>A list containing all the detected obstacles.</returns>
    public List<GameObject> GetObstacles()
    {
        return obstacles;
    }
}
