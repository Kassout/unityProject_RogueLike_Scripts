using System.Threading;
using System;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Pathfinding
{
    /// <summary>
    /// Class <c>PathRequestManager</c> is a Unity script used to manage the path request behavior of a game object.
    /// </summary>
    public class PathRequestManager : MonoBehaviour
    {
        #region Singleton

        // Singleton
        public static PathRequestManager Instance { get; private set; }

        #endregion

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Singleton
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// Instance field <c>results</c> is an array of Pathfinding <c>PathResult</c> structure representing the different properties of a pathfinding path result.
        /// </summary>
        private Queue<PathResult> _results = new Queue<PathResult>();

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (_results.Count > 0)
            {
                int itemsInQueue = _results.Count;

                lock (_results)
                {
                    for (int i = 0; i < itemsInQueue; i++)
                    {
                        PathResult result = _results.Dequeue();
                        result.callback(result.path, result.success);
                    }
                }
            }
        }

        /// <summary>
        /// This function is responsible for requesting a path in a new thread.
        /// </summary>
        public static void RequestPath(PathRequest request)
        {
#if UNITY_STANDALONE_WIN
            ThreadStart threadStart = delegate
            {
                request.pathFinder.FindPath(request, Instance.FinishedProcessingPath);
            };
            Thread thread = new Thread(threadStart);
            thread.Start();

            thread.Join();

            GC.Collect();
#elif UNITY_EDITOR
            ThreadStart threadStart = delegate
            {
                request.pathFinder.FindPath(request, Instance.FinishedProcessingPath);
            };
            Thread thread = new Thread(threadStart);
            thread.Start();

            thread.Join();

            GC.Collect();
#elif UNITY_WEBGL
            request.pathFinder.FindPath(request, Instance.FinishedProcessingPath);
#endif
        }

        /// <summary>
        /// This function is called on processing path finished action.
        /// </summary>
        public void FinishedProcessingPath(PathResult result)
        {
#if UNITY_STANDALONE_WIN           
            lock (_results)
            {
                _results.Enqueue(result);
            }
#elif UNITY_EDITOR
            lock (_results)
            {
                _results.Enqueue(result);
            }
#elif UNITY_WEBGL
            _results.Enqueue(result);
#endif
        }
    }
}

/// <summary>
/// <c>PathRequest</c> is a Pathfinding structure representing the different properties of a path request.
/// </summary>
public struct PathRequest
{
    /// <summary>
    /// Instance field <c>pathStart</c> is a Unity <c>Vector3</c> structure representing position coordinates of the path starting point.
    /// </summary>
    public Vector3 pathStart;

    /// <summary>
    /// Instance field <c>pathEnd</c> is a Unity <c>Vector3</c> structure representing position coordinates of the path ending point.
    /// </summary>
    public Vector3 pathEnd;

    /// <summary>
    /// Instance field <c>callback</c> represents the function to call on a processed action.
    /// </summary>
    public Action<Vector3[], bool> callback;

    public PathFinder pathFinder;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback, PathFinder _pathFinder)
    {
        pathStart = _start;
        pathEnd = _end;
        callback = _callback;
        pathFinder = _pathFinder;
    }
}

/// <summary>
/// <c>PathResult</c> is a Pathfinding structure representing the different properties of a path result.
/// </summary>
public struct PathResult
{
    /// <summary>
    /// Instance field <c>path</c> is an array of Unity <c>Vector3</c> structures representing the different position coordinates of the different points of the processed path.
    /// </summary>
    public Vector3[] path;

    /// <summary>
    /// Instance field <c>success</c> represents the success status of the processed path.
    /// </summary>
    public bool success;

    /// <summary>
    /// Instance field <c>callback</c> represents the function to call on a processed action.
    /// </summary>
    public Action<Vector3[], bool> callback;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }
}
