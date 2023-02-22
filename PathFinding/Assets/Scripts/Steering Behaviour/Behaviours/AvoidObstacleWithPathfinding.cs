using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidObstacleWithPathfinding : Steering
{
    public Transform target;

    [SerializeField]
    private Transform bigoteIzq;

    [SerializeField]
    private Transform bigoteDrc;

    [SerializeField]
    private Transform centro;

    [SerializeField]
    private float speed = 5f;

    private Vector3[] path;

    [SerializeField]
    private float lookAhead = 2f;

    [SerializeField]
    private int obstacleLayer;

    [SerializeField]
    private float nextWaypointDistance = 2f;

    private PursueBehaviour pursueBehaviour;

    private int targetIndex;

    private bool pathFound = false;

    private Vector3 currentMovement;

    private Vector3[] rayVector = new Vector3[3];

    private void Awake()
    {
        pursueBehaviour = GetComponent<PursueBehaviour>();
    }

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

        if (pathFound)
        {
            steering.linear = currentMovement;
            steering.linear.Normalize();
            steering.linear *= steeringbase.maxAcceleration;
            steering.angular = 0;
        }
        else
        {
            for (int i = 0; i < rayVector.Length; i++)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, rayVector[i], out hit, lookAhead))
                {
                    if (hit.collider.gameObject.layer == obstacleLayer)
                    {
                        if (!pathFound)
                        {
                            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                            pursueBehaviour.SetWeight(0);
                            pathFound = true;
                        }
                    }
                }
            }
        }

        return steering;
    }

    private void OnPathFound(Vector3[] newPath, bool pathSuccesful)
    {
        if (pathSuccesful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("NewPath");
            StartCoroutine("NewPath");
        }
    }

    private IEnumerator NewPath()
    {
        //Start Point
        if (path == null) yield break;

        Vector3 currentWaypoint = path[0];

        while (true)
        {
            //If our position equals the waypoint we gotta be in then
            if ((currentWaypoint - transform.position).magnitude < nextWaypointDistance)
            {
                //Update the index of the waypoints
                targetIndex++;
                //If we are on the last point, then we have reached our destination
                if (targetIndex >= path.Length)
                {
                    EndPath();
                    yield break;
                }
                //If not, next waypoint
                currentWaypoint = path[targetIndex];
            }

            //Update the linear movement of the steering
            currentMovement = currentWaypoint - transform.position;

            yield return null;
        }
    }

    public void EndPath()
    {
        pursueBehaviour.SetWeight(1);
        pathFound = false;
        currentMovement = Vector3.zero;
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;

                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}