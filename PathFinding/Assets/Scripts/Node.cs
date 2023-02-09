using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;

    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;

    public Node parent;
    public int movementPenalty;
    private int thisHeapIndex;

    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY, int _movementPenalty)
    {
        this.walkable = _walkable;
        this.worldPosition = _worldPosition;
        this.gridY = _gridY;
        this.gridX = _gridX;
        this.movementPenalty = _movementPenalty;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public int heapIndex
    {
        get
        {
            return thisHeapIndex;
        }
        set
        {
            thisHeapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        //Compare the cost of 2 nodes
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        //If the f cost is the same, then proceed to compare the h costs
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        //If the node is lower will return - compare
        return -compare;
    }
}