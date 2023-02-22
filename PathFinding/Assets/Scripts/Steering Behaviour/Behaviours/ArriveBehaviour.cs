using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveBehaviour : Steering
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float targetRadius = 2f;

    [SerializeField]
    private float slowRadius = 5f;

    public override SteeringData GetSteering(SteeringBehaviourBase steeringbase)
    {
        SteeringData steering = new SteeringData();

        Vector3 direction = target.position - transform.position;

        float distance = direction.magnitude;

        if (distance < targetRadius)
            steeringbase.GetComponent<Rigidbody>().velocity = Vector3.zero;

        float targetSpeed;

        if (distance > slowRadius)
        {
            //if we are far away enough from the point, we can go fast
            targetSpeed = steeringbase.maxAcceleration;
        }
        else
        {
            //As we get closer to the point, we slow down the speed
            targetSpeed = steeringbase.maxAcceleration * (distance / slowRadius);
        }

        //Implement the seeker behaviour to the arrive method

        Vector3 targetVelocity = direction;

        targetVelocity.Normalize();

        targetVelocity *= targetSpeed;

        steering.linear = targetVelocity - steeringbase.GetComponent<Rigidbody>().velocity;

        if (steering.linear.magnitude > steeringbase.maxAcceleration)
        {
            steering.linear.Normalize();
            steering.linear *= steeringbase.maxAcceleration;
        }

        //steering.angular = 0;

        return steering;
    }
}