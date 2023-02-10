using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;

    [SerializeField]
    private float speed = 5f;

    private Vector3[] path;

    private int targetIndex;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            //Get the position of the click on the map and set the target to that position

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider != null)
                {
                    target.position = hit.point;

                    PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                }
            }
        }
    }

    private void OnPathFound(Vector3[] newPath, bool pathSuccesful)
    {
        if (pathSuccesful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    private IEnumerator FollowPath()
    {
        //Start Point
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            //If our position equals the waypoint we gotta be in then
            if (transform.position == currentWaypoint)
            {
                //Update the index of the waypoints
                targetIndex++;
                //If we are on the last point, then we have reached our destination
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                //If not, next waypoint
                currentWaypoint = path[targetIndex];
            }
            //Move towards the waypoint
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
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