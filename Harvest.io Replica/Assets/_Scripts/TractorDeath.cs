using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorDeath : MonoBehaviour
{
    public delegate void OnPlayerDeath();
    public OnPlayerDeath onTractorDeath;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cargo") || other.CompareTag("Tractor"))
        {
            onTractorDeath?.Invoke();
        }
    }
}
