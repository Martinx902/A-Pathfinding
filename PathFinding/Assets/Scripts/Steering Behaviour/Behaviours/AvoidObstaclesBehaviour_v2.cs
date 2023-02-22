using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidObstaclesBehaviour_v2 : Steering
{
    [SerializeField]
    private Transform bigoteIzq;

    [SerializeField]
    private Transform bigoteDrc;

    [SerializeField]
    private Transform centro;

    [SerializeField]
    private float avoidDistance = 1f;

    [SerializeField]
    private float lookAhead = 2f;

    [SerializeField]
    private int obstacleLayer;

    [SerializeField]
    private Vector3 offset;

    private Vector3[] rayVector = new Vector3[3];

    public override SteeringData GetSteering(SteeringBehaviourBase steeringbase)
    {
        SteeringData steering = new SteeringData();

        rayVector = new Vector3[3];

        rayVector[0] = centro.position - transform.position;
        rayVector[0].Normalize();
        rayVector[0] *= lookAhead;

        rayVector[1] = bigoteDrc.position - transform.position;
        rayVector[1].Normalize();
        rayVector[1] *= lookAhead / 2;

        rayVector[2] = bigoteIzq.position - transform.position;
        rayVector[2].Normalize();
        rayVector[2] *= lookAhead / 2;

        for (int i = 0; i < rayVector.Length; i++)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, rayVector[i], out hit, lookAhead))
            {
                if (hit.collider.gameObject.layer == obstacleLayer)
                {
                    Vector3 target = hit.point + (hit.normal * avoidDistance) + offset;
                    steering.linear = target - transform.position;
                    steering.linear.Normalize();
                    steering.linear *= steeringbase.maxAcceleration;
                    break;
                }
            }
        }

        return steering;
    }

    private void OnDrawGizmos()
    {
        foreach (Vector3 vector in rayVector)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawRay(transform.position, vector);
        }
    }
}