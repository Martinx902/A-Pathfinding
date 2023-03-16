using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidObstacleWithPathfinding : Steering
{
    private Transform target;

    private Vector3[] path;

    [SerializeField]
    private float nextWaypointDistance = 2f;

    private int targetIndex;

    private bool pathFound = false;

    private Vector3 currentMovement;

    private TargetController targetController;

    private void Awake()
    {
        targetController = GetComponent<TargetController>();
    }

    public override SteeringData GetSteering(SteeringBehaviourBase steeringbase)
    {
        SteeringData steering = new SteeringData();

        if (targetController.GetTarget() != null)
        {
            target = targetController.GetTarget();
        }
        else
        {
            return steering;
        }

        if (GetWeight() == 0)
        {
            EndPath();
        }

        if (pathFound)
        {
            steering.linear = currentMovement;
            steering.linear.Normalize();
            steering.linear *= steeringbase.maxAcceleration;
            //steering.angular = 0;
        }
        else
        {
            if (!pathFound && GetWeight() == 1)
            {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                pathFound = true;
            }
        }

        return steering;
    }

    private void OnPathFound(Vector3[] newPath, bool pathSuccesful)
    {
        //Has found the best way to get to the target

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
        StopCoroutine("NewPath");
        path = null;
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