using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingUnit2D : MonoBehaviour
{
    public Transform Target;

    [SerializeField]
    Camera _cam;

    [SerializeField]
    Rigidbody2D _rb;

    [SerializeField]
    [Range(0, 200)]
    float Speed = 0.2f;



    List<Node2D> CurrentPath;
    Vector2 Direction;

    public bool hasPath = false;

    int i = 0;

    Vector2 PreviousPos;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hasPath = false;

            var MouseToVector = _cam.ScreenToWorldPoint(Input.mousePosition);
            MouseToVector.z = 0;

            PathFinderManager2D.GetInstance().RegiesterPathRequest(new FindPathRequest2D() { Start = transform.position, Target = MouseToVector, Callback = OnPathfound });
        }

        if (hasPath)
        {
            MoveTowards();
        }
    }

    void OnPathfound(Node2D[] Path)
    {
        CurrentPath = Path.ToList();
        i = 0;
        hasPath = true;
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

    void MoveTowards()
    {

        if (i == CurrentPath.Count)
        {
            hasPath = false;
            _rb.velocity = Vector2.zero;
            return;
        }

        if (i == 0)
        {
            PreviousPos = _rb.position ;
        }

        if (Vector2.Distance(_rb.position, CurrentPath[i].WorldPosition) > Speed )
        {
            Direction = (CurrentPath[i].WorldPosition - PreviousPos);
            _rb.position += Direction.normalized * Speed;


        }
        else
        {
            _rb.position = CurrentPath[i].WorldPosition;
            PreviousPos = _rb.position;
            i++;
        }



    }


}
