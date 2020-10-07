using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrainCollector : MonoBehaviour
{
    
    public delegate void OnGrainCollected();
    public OnGrainCollected onGrainCollected;


    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grain"))
        {
            onGrainCollected?.Invoke();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("CargoBox"))
        {
            for (int i = 0; i < 30; i++)
            {
                onGrainCollected?.Invoke();
            }
            Destroy(other.gameObject);
        }
    }
}
