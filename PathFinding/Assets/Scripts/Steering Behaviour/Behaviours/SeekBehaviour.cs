using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringBehaviourBase))]
public class SeekBehaviour : Steering
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

        //Gets the direction of the player and aims towards it

        if (targetController.GetTarget() == null)
        {
            return steering;
        }
        else
        {
            target = targetController.GetTarget();
            steering.linear = target.transform.position - transform.position;
            steering.linear.Normalize();
            steering.linear *= steeringbase.maxAcceleration;
            steering.angular = 0;
        }

        return steering;
    }
}