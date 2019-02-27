using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingUnit : MonoBehaviour
{
    public Transform Target;
    List<Node> CurrentPath;

    // Update is called once per frame
    void Update()
    {
        PathFinderManager.GetInstance().RegiesterPathRequest(new FindPathRequest() { Start = transform.position, Target = Target.position, Callback = OnPathfound });
    }

    void OnPathfound(Node[] Path)
    {
        CurrentPath = Path.ToList();
    }

    private void OnDrawGizmos()
    {
        if (CurrentPath == null)
            return;

        for(int i = 1; i < CurrentPath.Count;i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(CurrentPath[i - 1].WorldPosition, CurrentPath[i].WorldPosition);
        }
    }


}
