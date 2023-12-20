using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static NodeVisual;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;

    public int gridX; //Let node know its own position in grid
    public int gridY;

    public int gCost; //Distance cost from start
    public int hCost; //Distance cost from target

    public Node parent; //Previous node in path

    int heapIndex;

    private NodeVisual nodeVisual;

    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY,NodeVisual _nodeVisual)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;

        nodeVisual = _nodeVisual;
    }

    public int fCost //Total cost
    { 
        get
        { 
            return gCost+hCost; 
        } 
    }

    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare==0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost); //if fcost is equal, check hcost
        }
        return -compare;
    }

    public void UpdateCostsVisual()
    {
        nodeVisual.UpdateCosts(gCost, hCost, fCost);
    }

    public void UpdateColourVisual(NodeState state)
    {
        nodeVisual.UpdateColour(state);
    }
}
