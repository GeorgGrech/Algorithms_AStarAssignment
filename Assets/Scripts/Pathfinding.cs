using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour
{
    Grid grid;

    public Transform seeker, target;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Start()
    {
        StartCoroutine(FindPath(seeker.position, target.position));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos); //Get start and end nodes

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize); //Optimised with heap
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst(); //Set current and remove from heap

            closedSet.Add(currentNode);
            currentNode.UpdateColourVisual(NodeVisual.NodeState.ClosedSet);

            if (currentNode == targetNode) //Target reached
            {
                RetracePath(startNode, targetNode);
                break;
            }

            foreach(Node neighbour in grid.GetNeighbours(currentNode)) //Get neighbours
            {
                if(!neighbour.walkable || closedSet.Contains(neighbour)) //If obstacle or already checked, ignore
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour); //Recalculate costs based on new path being checked
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) //If new path to this node is ideal
                {
                    neighbour.gCost = newMovementCostToNeighbour; //Save the found (superior) gCost
                    neighbour.hCost = GetDistance(neighbour, targetNode); //Get this node's hCost to target
                    neighbour.parent = currentNode; //Set parent, used to retrace our steps

                    neighbour.UpdateCostsVisual();

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour); //If node has been added to closedSet as part of previous path check, add it again to openSet
                        neighbour.UpdateColourVisual(NodeVisual.NodeState.OpenSet);
                    }
                }
                yield return null;
            }
        }
    }


    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        currentNode.UpdateColourVisual(NodeVisual.NodeState.Path);
        while(currentNode != startNode) //Keep looping until we've reached the start
        {
            path.Add(currentNode);
            currentNode = currentNode.parent; //Move to next in path
            currentNode.UpdateColourVisual(NodeVisual.NodeState.Path);
        }

        path.Reverse(); //Reverse to get in correct order

        grid.path = path;
    }

    int GetDistance(Node nodeA, Node nodeB) //diagonal + horizontal/vertical moves
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX); //How many nodes away on x axis
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY); //How many nodes away on y axis

        //The lowest value indicates number of diagonals needed. Highest minus lowest gives remaining horizontal/vertical moves
        //10 is cost of horizontal/vertical movement. Diagonal, being longer, costs 14
        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
       
    }
}
