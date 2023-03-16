using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringBehaviourBase))]
public class FleeBehaviour : Steering
{
    private Transform target;

    private TargetController targetController;

    private void Awake()
    {
        targetController = GetComponent<TargetController>();
    }

    public override SteeringData GetSteering(SteeringBehaviourBase steeringbase)
    {
        SteeringData steering = new SteeringData();

        //Gets the direction of the player and goes to the opposite direction

        if (targetController.GetTarget() != null)
        {
            target = targetController.GetTarget();
            steering.linear = transform.position - target.transform.position;
            steering.linear.Normalize();
            steering.linear *= steeringbase.maxAcceleration;
            steering.angular = 0;
        }

        return steering;
    }
}