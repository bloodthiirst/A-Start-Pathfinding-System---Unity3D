using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PathFinderManager2D : MonoBehaviour
{
    static PathFinderManager2D instance;
    public Queue<FindPathRequest2D> RequestQueue { get; private set; } = new Queue<FindPathRequest2D>();
    public PathFinder2D PF;

    private readonly object PFLock = new object();

    private void Awake()
    {
        instance = this;
    }

    public static PathFinderManager2D GetInstance() => instance;

    // Update is called once per frame
    void Update()
    {
        while (RequestQueue.Count != 0)
        {
            FindPathRequest2D Request = RequestQueue.Dequeue();

            lock (PFLock)
            {
                Thread thread = new Thread(() => ProcessRequest(Request));
                thread.Start();
            }
            
        }

    }

    void ProcessRequest(FindPathRequest2D Request)
    {
        Request.Callback(PF.FindPath(Request.Start, Request.Target));
    }

    public void RegiesterPathRequest(FindPathRequest2D Request)
    {
        lock (PFLock)
        {
            RequestQueue.Enqueue(Request);
        }
    }
}



public struct FindPathRequest2D
{
    public Vector2 Start { get; set; }
    public Vector2 Target { get; set; }
    public Action<Node2D[]> Callback { get; set; }
}
