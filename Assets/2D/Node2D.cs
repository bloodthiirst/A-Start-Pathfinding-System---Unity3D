using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Node2D : IComparable
{
    public GridController2D Grid;

    public Node2D Parent { get; set; }

    public bool Walkable { get; set; }

    public float WorldOffsetX { get; set; }
    public float WorldOffsetZ { get; set; }
    public Vector2 WorldPosition
    {
        get
        {
            return Grid.BottomLeftCornerWorldSpace() 
                + (Grid.transform.right.Vector3To2() * ((x * Grid.NodeDiameter) + Grid.NodeRadius)) 
                + (Grid.transform.up.Vector3To2() * ((y * Grid.NodeDiameter) + Grid.NodeRadius));

        }
    }

    public float gCost { get; set; }
    public float hCost { get; set; }
    public float fCost { get { return gCost + hCost; } }

    public int x { get; set; }
    public int y { get; set; }
    public List<Node2D> Neighbors = new List<Node2D>();

    public List<Node2D> BlurKernelList()
    {
        List<Node2D> Neighbors = new List<Node2D>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                int indexX = x + i;
                int indexY = y + j;

                if (isBetween(indexX, 0, Grid.GridX - 1) && isBetween(indexY, 0, Grid.GridY - 1))
                {
                    Neighbors.Add(Grid.Nodes[indexX, indexY]);
                }

            }
        }

        return Neighbors;

    }

    public List<Node2D> NeighborsList()
    {
        Neighbors.Clear();

        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if(i == 0 && j == 0)
                {
                    continue;
                }

                int indexX = x + i;
                int indexY = y + j;

                if (isBetween(indexX, 0, Grid.GridX - 1) && isBetween(indexY, 0, Grid.GridY - 1))
                {
                    if (indexX == 0 && indexY == 0)
                    {
                        continue;
                    }
                     
                    Neighbors.Add(Grid.Nodes[indexX, indexY]);
                }

            }
        }
        return Neighbors;

    }

    bool isBetween(int value, int min, int max)
    {
        return (value >= min && value <= max);
    }

    public static float GetDistance(Node2D A , Node2D B)
    {
        int distX = Mathf.Abs(A.x - B.x);
        int distY = Mathf.Abs(A.y - B.y);

        if(distX < distY)
        {
            return (14 * distX) + (10 * (distY - distX));
        }

        return (14 * distY) + (10 * (distX - distY));

    }

    public int CompareTo(object obj)
    {
        var tmp = obj as Node2D;
        if (tmp == null)
            return 0;
        if (this.fCost < tmp.fCost || this.fCost == tmp.fCost && this.hCost < tmp.hCost)
            return -1;
        return 1;
    }
}
