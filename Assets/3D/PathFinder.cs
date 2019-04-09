using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public GridController Grid;

    private void Awake()
    {
        Grid = GetComponent<GridController>();
    }

    public Node[] FindPath(Vector3 Start, Vector3 Target)
    {
        Node StartNode = Grid.NodeFromWorldPoint(Start);
        Node EndNode = Grid.NodeFromWorldPoint(Target);

        if (!EndNode.Walkable)
            return new Node[0];

        MinHeap<Node> OpenSet = new MinHeap<Node>();
        List<Node> ClosedSet = new List<Node>();

        OpenSet.Insert(StartNode);

        while (OpenSet.Count > 0)
        {
            Node CurrentNode = OpenSet.ExtractMin();

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

            foreach (Node neighbor in CurrentNode.Neighbors)
            {
                if (neighbor == null)
                    continue;

                if (!neighbor.Walkable || ClosedSet.Contains(neighbor))
                    continue;

                var MovementCostToNeighbor = CurrentNode.gCost + Node.GetDistance(CurrentNode, neighbor);
                if (MovementCostToNeighbor < neighbor.gCost || !OpenSet.Contains(neighbor))
                {
                    neighbor.gCost = MovementCostToNeighbor;
                    neighbor.hCost = Node.GetDistance(neighbor, EndNode);

                    neighbor.Parent = CurrentNode;

                    if (!OpenSet.Contains(neighbor))
                    {
                        OpenSet.Insert(neighbor);
                    }
                }
            }


        }

        return new Node[0];
    }

    private bool ContainsNode(IList<Node> list, Node node)
    {
        for(int i = 0; i < list.Count; i++)
        {
            if (list[i] == node)
                return true;
        }

        return false;
    }

    Node[] RetracePath(Node Start, Node Target)
    {
        List<Node> Path = new List<Node>();

        Node TemporaryNode = Target;

        while (TemporaryNode != Start)
        {
            Path.Add(TemporaryNode);
            TemporaryNode = TemporaryNode.Parent;
        }

        Path.Reverse();

        return Path.ToArray();
    }
}
