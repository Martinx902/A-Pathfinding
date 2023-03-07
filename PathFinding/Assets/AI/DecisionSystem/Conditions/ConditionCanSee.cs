using UnityEngine;
using System.Collections;

/// <summary>
/// Checks if "target" is visible (raycasting) from transform.
/// It relies on ability to cast rays, so opaque objects
/// should have colliders.
/// </summary>
public class ConditionCanSee : ICondition
{
    public Transform target;

    /// <summary>
    /// The layer mask of entities visible
    /// </summary>
    public LayerMask obstaclesLayerMask;

    public override bool Test()
    {
        if (target == null)
            return false;

        RaycastHit hit;

        Physics.Raycast(transform.position, target.position, out hit);

        if (hit.collider.gameObject.layer == obstaclesLayerMask.value)
        {
            return true;
        }
        else
            return false;
    }
}