using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrainCollector : MonoBehaviour
{
    
    public delegate void OnGrainCollected();
    public OnGrainCollected onGrainCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grain"))
        {
            CollectGrain(other);
        }

        if (other.CompareTag("CargoBox"))
        {
            CollectCargo(other);
        }
    }

    /// <summary>
    /// Collects cargo container.
    /// </summary>
    /// <param name="other">The collider of the cargo.</param>
    private void CollectCargo(Collider other)
    {
        for (int i = 0; i < 80; i++)
        {
            onGrainCollected?.Invoke();
        }
        Destroy(other.gameObject);
    }

    /// <summary>
    /// Collects grain.
    /// </summary>
    /// <param name="other">The collider of the grain.</param>
    private void CollectGrain(Collider other)
    {
        onGrainCollected?.Invoke();
        Destroy(other.gameObject);
    }
}
