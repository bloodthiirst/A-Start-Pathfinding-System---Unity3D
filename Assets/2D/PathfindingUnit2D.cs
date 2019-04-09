using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingUnit2D : MonoBehaviour
{
    public Transform Target;
    List<Node2D> CurrentPath;

    float PathTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        PathTimer += Time.deltaTime;

        if (PathTimer > 1.5f)
        {
            PathTimer = 0;
                PathFinderManager2D.GetInstance().RegiesterPathRequest(new FindPathRequest2D() { Start = transform.position, Target = Target.position, Callback = OnPathfound });
        }
    }

    void OnPathfound(Node2D[] Path)
    {
        CurrentPath = Path.ToList();
    }

    private void OnDrawGizmos()
    {
        if (CurrentPath == null)
            return;

        for (int i = 1; i < CurrentPath.Count; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(CurrentPath[i - 1].WorldPosition, CurrentPath[i].WorldPosition);
        }
    }


}
