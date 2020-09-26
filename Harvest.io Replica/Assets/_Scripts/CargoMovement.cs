using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CargoMovement : MonoBehaviour
{
    [SerializeField] private GameObject backCargoFollowPosition;
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private GameObject cargoToDrop;

    private GameObject followTarget;

    private void Start()
    {
        FindObjectOfType<PlayerDeath>().onPlayerDeath += DropCargo;
    }

    public GameObject GetNextCargoPosition()
    {
        return backCargoFollowPosition;
    }

    public void SetFollowTarget(GameObject target)
    {
        followTarget = target;
    }

    private void Update()
    {
        if (followTarget != null)
        {
            MoveToTarget();

            RotateTowardsTarget();
        }
    }

    private void RotateTowardsTarget()
    {
        Vector3 toFollowPosition = followTarget.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(toFollowPosition, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * movementSpeed);
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.Lerp(transform.position, followTarget.transform.position, Time.deltaTime * movementSpeed);
    }

    private void DropCargo()
    {
        Instantiate(cargoToDrop, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
