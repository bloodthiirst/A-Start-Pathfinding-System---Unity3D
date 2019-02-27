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

        List<Node> OpenSet = new List<Node>();
        List<Node> ClosedSet = new List<Node>();

        OpenSet.Add(StartNode);

        while (OpenSet.Count > 0)
        {
            Node CurrentNode = OpenSet[0];

            for (int i = 1; i < OpenSet.Count; i++)
            {
                if (OpenSet[i].fCost < CurrentNode.fCost || OpenSet[i].fCost == CurrentNode.fCost && OpenSet[i].hCost < CurrentNode.hCost)
                {
                    CurrentNode = OpenSet[i];
                }
            }

            OpenSet.Remove(CurrentNode);
            ClosedSet.Add(CurrentNode);

            if (CurrentNode == EndNode)
            {
                return RetracePath(StartNode, EndNode);              
            }

            foreach (Node neighbor in CurrentNode.NeighborsList())
            {
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
                        OpenSet.Add(neighbor);
                    }
                }
            }


        }

        return new Node[0];
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
