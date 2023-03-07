using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConditionHumansScaredNear : ICondition
{
    [SerializeField]
    private float sphereRadius = 2f;

    [SerializeField]
    private LayerMask layerMask;

    private bool humansScared;

    public override bool Test()
    {
        return CheckForScaredHumans();
    }

    private bool CheckForScaredHumans()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sphereRadius, layerMask);

        //Check for any human that is scared depeding on their actual state

        if (colliders.Length > 0)
            humansScared = colliders.Any(x => x.gameObject.GetComponentInParent<StateController>().GetActualState() == States.Flee);
        else
            return false;

        return humansScared;
    }
}