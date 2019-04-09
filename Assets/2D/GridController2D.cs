using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class GridController2D : MonoBehaviour
{
    public PathFinder2D PF;


    public Node2D[,] Nodes { get; set; }
    public Vector2 GridDimensions;
    public float NodeRadius;
    public LayerMask ObstaclesMask;

    public Vector2 GridAnchor;


    [Header("Grounds Types")]
    public List<GroundWeight> TypesOfGround;

    [Header("Update Grid")]
    public bool isDynamic;
    


    public float NodeDiameter { get { return NodeRadius * 2; } }
    public int GridX;
    public int GridY;

    private void Awake()
    {
       
        GridX = Mathf.RoundToInt(GridDimensions.x / NodeDiameter);
        GridY = Mathf.RoundToInt(GridDimensions.y / NodeDiameter);

        CreateGrid();
        GenerateNeighbors();
    }

    private void GenerateNeighbors()
    {
        for(int x = 0; x < GridX; x++)
        {
            for (int y = 0;  y < GridY; y++)
            {
                Nodes[x, y].Neighbors = Nodes[x, y].NeighborsList();
            }
        }
    }

    void CreateGrid()
    {
        GridAnchor = transform.position;

        Nodes = new Node2D[GridX, GridY];

        for(int x = 0; x < GridX; x++)
        {
            for(int y = 0; y < GridY; y++)
            {
                //Vector3 NodeWorldPos = BottomLeftCornerWorldSpace() + transform.right * ( (x * NodeDiameter) + NodeRadius) + transform.forward * ((y * NodeDiameter) + NodeRadius);

                Node2D node = new Node2D() { x = x, y = y, Grid = this };
                node.Walkable = Physics2D.OverlapCircle(node.WorldPosition, NodeRadius, ObstaclesMask.value) == null;
                Nodes[x, y] = node;
            }
        }
    }

    void BlurGrid()
    {
        var BlurredGrid = new Node2D[GridX, GridY];

        for (int x = 0; x < GridX; x++)
        {
            for (int y = 0; y < GridY; y++)
            {
                //Vector3 NodeWorldPos = BottomLeftCornerWorldSpace() + transform.right * ( (x * NodeDiameter) + NodeRadius) + transform.forward * ((y * NodeDiameter) + NodeRadius);

                Node2D node = Nodes[x, y];

                var Kernel = node.BlurKernelList();
                node.hCost = Kernel.Select(n => n.hCost).Average();
                node.gCost = Kernel.Select(n => n.gCost).Average();

                BlurredGrid[x, y] = node;
            }
        }

        Nodes = BlurredGrid;
    }

    public Vector2 BottomLeftCornerWorldSpace()
    {
        return GridAnchor 
            - (transform.right.Vector3To2() * (GridDimensions.x / 2)) 
            - (transform.up.Vector3To2() * (GridDimensions.y / 2));
    }

    public Node2D NodeFromWorldPoint(Vector2 WordPos)
    {
        float PercentX = ((WordPos.x - GridAnchor.x) + GridDimensions.x / 2) / GridDimensions.x;
        float PercentY = ((WordPos.y - GridAnchor.y) + GridDimensions.y / 2) / GridDimensions.y;

        PercentX = Mathf.Clamp01(PercentX);
        PercentY = Mathf.Clamp01(PercentY);

        int indexX = Mathf.RoundToInt(PercentX * (GridX - 1));
        int indexY = Mathf.RoundToInt(PercentY * (GridY - 1));

        return Nodes[indexX, indexY];
    }

    private void OnDrawGizmos()
    {
        DrawGridShape();
        DrawNodes();
    }

    void DrawGridShape()
    {
        Gizmos.color = new Color(1, 1, 1 , 0.4f);
        Gizmos.DrawCube(transform.position, new Vector3(GridDimensions.x, GridDimensions.y , 0.1f));
    }

    void DrawNodes()
    {
        if (Nodes == null)
            return;

        foreach(var node in Nodes)
        {
            Gizmos.color = Color.yellow;

            if ( !node.Walkable )
            {
                Gizmos.color = Color.red;
            }
                

            Gizmos.DrawWireCube(node.WorldPosition, new Vector3(NodeDiameter, NodeDiameter , 0.1f ));
        }
    }
    
}



[Serializable]
public class GroundWeight
{
    public LayerMask GroundType;
    public float Weight;
}