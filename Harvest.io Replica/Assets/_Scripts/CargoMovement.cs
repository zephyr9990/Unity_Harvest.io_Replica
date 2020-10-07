using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CargoMovement : MonoBehaviour
{
    [SerializeField] private GameObject backCargoFollowPosition;
    [SerializeField] private GameObject cargoToDrop;
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float rotateSpeed = 10f;

    private GameObject followTarget;

    private void Update()
    {
        if (followTarget != null)
        {
            MoveToTarget();

            RotateTowardsTarget();
        }
    }

    /// <summary>
    /// Returns the gameobject containing the position of the next cargo spawn location.
    /// </summary>
    /// <returns>The gameobject that serves as the spawn point for the next cargo to spawn.</returns>
    public GameObject GetNextCargoPosition()
    {
        return backCargoFollowPosition;
    }

    /// <summary>
    /// Sets the follow target of this cargo.
    /// </summary>
    /// <param name="target">The target to follow.</param>
    public void SetFollowTarget(GameObject target)
    {
        followTarget = target;
    }
    /// <summary>
    /// Rotate towards the target.
    /// </summary>
    private void RotateTowardsTarget()
    {
        Vector3 toFollowPosition = followTarget.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(toFollowPosition, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, UnityEngine.Time.deltaTime * rotateSpeed);
    }

    /// <summary>
    /// Moves towards the target.
    /// </summary>
    private void MoveToTarget()
    {
        transform.position = Vector3.Lerp(transform.position, followTarget.transform.position, UnityEngine.Time.deltaTime * movementSpeed);
    }

    /// <summary>
    /// On death, drop cargo.
    /// </summary>
    public void DropCargo()
    {
        Instantiate(cargoToDrop, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
