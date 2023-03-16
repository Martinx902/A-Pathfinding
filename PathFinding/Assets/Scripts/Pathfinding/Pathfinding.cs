using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour
{
    private Grid grid;

    private PathRequestManager requestManager;

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    private IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        //Get a start node and a target node
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        //Quick check to see if the path is viable
        if (startNode.walkable && targetNode.walkable)
        {
            //Create an open and closed list
            //Nodes to evaluate
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize());
            //Evaluated Nodes
            HashSet<Node> closedSet = new HashSet<Node>();

            //Add the first node to the open list, the starting position one.
            openSet.Add(startNode);

            //Loop through the open list looking for the best next node
            while (openSet.Count() > 0)
            {
                //We establish a current node, in this case the first one of the open list.
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                //if the current node is the target node, then we are done!
                if (currentNode == targetNode)
                {
                    sw.Stop();
                    //print("Path found in: " + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    break;
                }

                //We look through the neightbours of the current node to see which ones we have now abeilable
                foreach (Node neighbour in grid.GetNeightbours(currentNode))
                {
                    //If the node is not walkable, or it's not contained on the closed set, we continue
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    //We calculate the cost of moving to the nearest neighbour.
                    //That cost would be the current g cost + the distance between the current node and our neighbour
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;

                    //If the cost of that movement is less that others neighbour cost and it hasnt been evaluated yet, the we proceed
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        //We update the G and H costs of the neighbour, making it update the F cost
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        //And we set the neighbour parent to the current node, so that we can keep track of the path we are stableshing
                        neighbour.parent = currentNode;

                        //Then, finally, we add that neighbour to the open set, cause it has been already evaluated.
                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }

        yield return null;

        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    private Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);

            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = SimplifyPath(path);

        Array.Reverse(waypoints);

        return waypoints;
    }

    private Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }

            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        else
        {
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}