using UnityEngine;
using System.Collections;

/// <summary>
/// Checks if "target" is visible (raycasting) from transform.
/// It relies on ability to cast rays, so opaque objects
/// should have colliders.
/// </summary>
public class ConditionCanSee : ICondition
{
    private Transform target;

    /// <summary>
    /// The layer mask of entities visible
    /// </summary>
    public int obstaclesLayerMask;

    private TargetController targetController;

    private void Awake()
    {
        targetController = FindObjectOfType<TargetController>();
    }

    public override bool Test()
    {
        target = targetController.GetTarget();

        if (target == null)
            return false;

        RaycastHit hit;

        Vector3 direction = targetController.GetTarget().position - transform.position;

        Physics.Raycast(transform.position, direction, out hit);

        if (hit.collider == null)
            return false;

        if (hit.collider.gameObject.layer == obstaclesLayerMask)
        {
            return true;
        }
        else
            return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (target == null)
            return;

        Gizmos.DrawRay(transform.position, targetController.GetTarget().position - transform.position);
    }
}