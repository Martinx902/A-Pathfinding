using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionEntityDetected : ICondition
{
    private bool entitySeen = false;

    [SerializeField]
    private float maxDistance = 6;

    [SerializeField]
    private float sphereRadius = 2f;

    [SerializeField]
    private LayerMask layerMask;

    public override bool Test()
    {
        return CheckForEntities();
    }

    private bool CheckForEntities()
    {
        RaycastHit hit;

        entitySeen = Physics.SphereCast(transform.position, sphereRadius, transform.forward, out hit, maxDistance, layerMask);

        //Pasamos al target controller los datos de la posición del siguiente target
        //targetController.SetTarget(hit.collider.gameObject.transform);

        return entitySeen;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawWireSphere(transform.position + transform.forward * maxDistance, sphereRadius);
    }
}