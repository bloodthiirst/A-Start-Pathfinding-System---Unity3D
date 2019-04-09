using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder2D : MonoBehaviour
{
    public GridController2D Grid;

    private void Awake()
    {
        Grid = GetComponent<GridController2D>();
    }

    public Node2D[] FindPath(Vector2 Start, Vector2 Target)
    {
        Node2D StartNode = Grid.NodeFromWorldPoint(Start);
        Node2D EndNode = Grid.NodeFromWorldPoint(Target);

        if (!EndNode.Walkable)
            return new Node2D[0];

        MinHeap<Node2D> OpenSet = new MinHeap<Node2D>();
        List<Node2D> ClosedSet = new List<Node2D>();

        OpenSet.Insert(StartNode);

        while (OpenSet.Count > 0)
        {
            Node2D CurrentNode = OpenSet.ExtractMin();

            /*
            for (int i = 1; i < OpenSet.Count; i++)
            {
                if (OpenSet[i].fCost < CurrentNode.fCost || OpenSet[i].fCost == CurrentNode.fCost && OpenSet[i].hCost < CurrentNode.hCost)
                {
                    CurrentNode = OpenSet[i];
                }
            }
            */
            ClosedSet.Add(CurrentNode);

            if (CurrentNode == EndNode)
            {
                return RetracePath(StartNode, EndNode);              
            }

            foreach (Node2D neighbor in CurrentNode.Neighbors)
            {
                if (neighbor == null)
                    continue;

                if (!neighbor.Walkable || ClosedSet.Contains(neighbor))
                    continue;

                var MovementCostToNeighbor = CurrentNode.gCost + Node2D.GetDistance(CurrentNode, neighbor);
                if (MovementCostToNeighbor < neighbor.gCost || !OpenSet.Contains(neighbor))
                {
                    neighbor.gCost = MovementCostToNeighbor;
                    neighbor.hCost = Node2D.GetDistance(neighbor, EndNode);

                    neighbor.Parent = CurrentNode;

                    if (!OpenSet.Contains(neighbor))
                    {
                        OpenSet.Insert(neighbor);
                    }
                }
            }


        }

        return new Node2D[0];
    }

    private bool ContainsNode(IList<Node2D> list, Node2D node)
    {
        for(int i = 0; i < list.Count; i++)
        {
            if (list[i] == node)
                return true;
        }

        return false;
    }

    Node2D[] RetracePath(Node2D Start, Node2D Target)
    {
        List<Node2D> Path = new List<Node2D>();

        Node2D TemporaryNode = Target;

        while (TemporaryNode != Start)
        {
            Path.Add(TemporaryNode);
            TemporaryNode = TemporaryNode.Parent;
        }

        Path.Reverse();

        return Path.ToArray();
    }
}
