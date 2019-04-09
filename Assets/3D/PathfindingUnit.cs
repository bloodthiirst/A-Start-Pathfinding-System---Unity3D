using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingUnit : MonoBehaviour
{
    public Transform Target;
    List<Node> CurrentPath;

    float PathTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        PathTimer += Time.deltaTime;

        if (PathTimer > 1.5f)
        {
            PathTimer = 0;

            PathFinderManager.GetInstance().RegiesterPathRequest(new FindPathRequest() { Start = transform.position, Target = Target.position, Callback = OnPathfound });
        }
    }

    void OnPathfound(Node[] Path)
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

public static class Utils
{
    public static Vector3 Vector2Swizzle(this Vector3 vec)
    {
        return new Vector3(vec.x, vec.z, vec.y);
    }

    public static Vector2 Vector3To2(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    } 
}
