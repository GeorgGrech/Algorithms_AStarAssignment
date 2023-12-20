using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Grid : MonoBehaviour
{
    Node[,] grid; //Grid - Array of nodes
    public Vector2 gridWorldSize; //Size of pathfinding grid
    public float nodeRadius; //Size of node
    public LayerMask unwalkableLayer; //Obstacle layer

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    [SerializeField] private GameObject nodeVisual;

    private void Start()
    {
        nodeDiameter = nodeRadius*2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }


    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY]; //Generate grid with correct dimensions

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2; //Get bottom left node pos

        for (int x = 0; x < gridSizeX; x++) 
        {
            for (int y = 0; y < gridSizeY; y++) 
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius); //Find node position
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwalkableLayer)); //is there collision? if so node is not walkable

                NodeVisual instantiatedNodeVisual = Instantiate(nodeVisual,worldPoint,Quaternion.identity).GetComponent<NodeVisual>();

                grid[x, y] = new Node(walkable,worldPoint,x,y,instantiatedNodeVisual); //Create node with properties
                if (!grid[x, y].walkable) grid[x, y].UpdateColourVisual(NodeVisual.NodeState.Unwalkable);
            }
        } 
    }

    public List<Node> GetNeighbours(Node n)
    {
        List<Node> neighbours = new List<Node>();
        for(int x = -1; x <= 1; x++) // -1 to 1 to get neighbouring indexes
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x==0 && y == 0) // This is the original node, ignore
                {
                    continue;
                }

                int checkX = n.gridX + x; //grid Positions to check
                int checkY = n.gridY + y;

                if(checkX>=0 && checkX < gridSizeX &&  checkY >=0 && checkY < gridSizeY) //Check that given position is inside the grid
                {
                    neighbours.Add(grid[checkX,checkY]); //Add to neighbours list
                }
            }
        }

        return neighbours;
    }



    public Node NodeFromWorldPoint(Vector3 worldPosition) //Method to find specific node 
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x; //Get percentage of X Position
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y; //Get percentage of Y Position (using z worldPosition)
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX-1) * percentX); //Convert to int from within grid size
        int y = Mathf.RoundToInt((gridSizeX-1) * percentY);

        return grid[x, y];


    }

    public List<Node> path;
}
