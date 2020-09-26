using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorCollector : MonoBehaviour
{
    public delegate void OnGrainCollected();
    public OnGrainCollected onGrainCollected;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grain"))
        {
            onGrainCollected?.Invoke();
            Destroy(other.gameObject);
        }
    }
}
