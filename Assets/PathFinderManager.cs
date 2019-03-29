using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PathFinderManager : MonoBehaviour
{
    static PathFinderManager instance;
    public Queue<FindPathRequest> RequestQueue { get; private set; } = new Queue<FindPathRequest>();
    public PathFinder PF;

    private readonly object PFLock = new object();

    private void Awake()
    {
        instance = this;
    }

    public static PathFinderManager GetInstance() => instance;

    // Update is called once per frame
    void Update()
    {
        while (RequestQueue.Count != 0)
        {
            FindPathRequest Request = RequestQueue.Dequeue();

            lock (PFLock)
            {
                Thread thread = new Thread(() => ProcessRequest(Request));
                thread.Start();
            }
            
        }

    }

    void ProcessRequest(FindPathRequest Request)
    {
        Request.Callback(PF.FindPath(Request.Start, Request.Target));
    }

    public void RegiesterPathRequest(FindPathRequest Request)
    {
        lock (PFLock)
        {
            RequestQueue.Enqueue(Request);
        }
    }
}



public struct FindPathRequest
{
    public Vector3 Start { get; set; }
    public Vector3 Target { get; set; }
    public Action<Node[]> Callback { get; set; }
}
