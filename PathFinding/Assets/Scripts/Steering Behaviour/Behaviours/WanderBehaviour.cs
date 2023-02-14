using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBehaviour : Steering
{
    [SerializeField]
    private float wanderRate = 0.4f;

    [SerializeField]
    private float wanderOffset = 1.5f;

    [SerializeField]
    private float wanderRadius = 4f;

    private float wanderOrientation = 0f;

    private float RandomBinomial()
    {
        return Random.value - Random.value;
    }

    private Vector3 OrientationToVector(float orientation)
    {
        return new Vector3(Mathf.Cos(orientation), 0, Mathf.Sign(orientation));
    }

    public override SteeringData GetSteering(SteeringBehaviourBase steeringbase)
    {
        SteeringData steering = new SteeringData();

        //Creates a random orientation

        wanderOrientation += RandomBinomial() * wanderRate;

        //Gets the orientation of the character

        float characterOrientation = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;

        //Aligns the character orientation with the supposed one of the wander movement

        float targetOrientation = wanderOrientation + characterOrientation;

        //Sets a target position based on our position + a little offset

        Vector3 targetPosition = transform.position + wanderOffset * OrientationToVector(characterOrientation);

        //Adds the target position + the radius to create an imaginary temporary target to move to

        targetPosition += wanderRadius * OrientationToVector(targetOrientation);

        steering.linear = targetPosition - transform.position;

        steering.linear.Normalize();

        steering.linear *= steeringbase.maxAcceleration;

        return steering;
    }
}