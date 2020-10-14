using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject followTarget;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float followSpeed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        if (followTarget)
        {
            Vector3 toTarget = followTarget.transform.position - transform.position;
            transform.position = Vector3.Lerp(transform.position, followTarget.transform.position + offset, Time.deltaTime * followSpeed);
        }
    }
}
