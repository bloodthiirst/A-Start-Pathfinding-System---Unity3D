using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PathFinderManager : MonoBehaviour
{
    static PathFinderManager instance;
    public Queue<FindPathRequest> RequestQueue { get; set; }
    public PathFinder PF;

    private readonly object PFLock = new object();

    private void Awake()
    {
        instance = this;

        instance.RequestQueue = new Queue<FindPathRequest>();
    }

    public static PathFinderManager GetInstance() => instance;

    // Update is called once per frame
    void Update()
    {
        while (RequestQueue.Count > 0)
        {
            ThreadStart th = new ThreadStart( () => ProcessRequest(RequestQueue.Dequeue()) );
            th.Invoke();
        }

    }

    void ProcessRequest( FindPathRequest Request )
    {
        lock (PFLock)
        {
            Request.Callback(PF.FindPath(Request.Start, Request.Target));
        }
    }

    public void RegiesterPathRequest(FindPathRequest Request)
    {
        RequestQueue.Enqueue(Request);
    }
}



    public struct FindPathRequest
{
    public Vector3 Start { get; set; }
    public Vector3 Target { get; set; }
    public Action<Node[]> Callback { get; set; }
}
