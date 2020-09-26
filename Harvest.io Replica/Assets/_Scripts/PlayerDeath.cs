using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public delegate void OnPlayerDeath();
    public OnPlayerDeath onPlayerDeath;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cargo"))
        {
            onPlayerDeath?.Invoke();
        }
    }
}
