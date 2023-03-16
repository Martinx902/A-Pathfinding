using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionEntityDetected : ICondition
{
    [SerializeField]
    private float sphereRadius = 20f;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private TargetController targetController;

    public override bool Test()
    {
        return CheckForEntities();
    }

    private bool CheckForEntities()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, sphereRadius, layerMask);

        if (hits.Length > 0)
        {
            //We should select the closest entity
            targetController.SetTarget(ReturnClosestEntity(hits).transform);
            //targetController.SetTarget(hits[0].transform);
            return true;
        }
        else if (hits.Length <= 0)
        {
            return false;
        }

        return false;
    }

    /// <summary>
    /// Tomado prestado del sistema de interacción del juego del gato :3
    /// </summary>
    /// <param name="hits"></param>
    /// <returns></returns>
    private GameObject ReturnClosestEntity(Collider[] hits)
    {
        GameObject closestEntity;

        Collider[] entities = hits;

        Vector3 distance;

        Vector3 tempDistance;

        if (entities.Length == 0) return null;
        else
        {
            closestEntity = entities[0].gameObject;
        }

        distance = entities[0].transform.position - transform.position;

        foreach (Collider collider in entities)
        {
            //Then with those objects (Colliders due to the function only returning those) we make a basic sorting algorithm
            // where it returns the closest one to the player

            tempDistance = collider.transform.position - transform.position;

            if (tempDistance.magnitude <= distance.magnitude)
            {
                closestEntity = collider.gameObject;
                distance = tempDistance;
            }
        }

        return closestEntity;
    }

    private void OnDrawGizmos()
    {
        if (this.gameObject.CompareTag("Human"))
        {
            Gizmos.color = new Color(0, 0, 1, 0.35f);
        }
        else
        {
            Gizmos.color = new Color(1, 0, 0, 0.35f);
        }

        Gizmos.DrawSphere(transform.position, sphereRadius);
    }
}