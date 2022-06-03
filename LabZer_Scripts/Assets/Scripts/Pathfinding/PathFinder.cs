using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    /// <summary>
    /// Class <c>PathFinder</c> is a Unity script used to manage the pathfinding behavior of a game object.
    /// </summary>
    public class PathFinder : MonoBehaviour
    {
        #region Fields / Properties

        /// <summary>
        /// Instance field <c>_grid</c> is a Pathfinding <c>Grid</c> script representing the pathfinding grid behavior.
        /// </summary>
        private Grid _grid;

        #endregion

        #region MonoBehavior

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _grid = GetComponent<Grid>();
        }

        #endregion

        #region Private

        /// <summary>
        /// This function is responsible for finding the path of the game object over the grid from start to target position.
        /// </summary>
        public void FindPath(PathRequest request, Action<PathResult> callback)
        {
            Vector3[] wayPoints = new Vector3[0];
            bool pathSuccess = false;

            Node startNode = _grid.GetNodeFromWorldPoint(request.pathStart);
            Node targetNode = _grid.GetNodeFromWorldPoint(request.pathEnd);
            startNode.parent = startNode;

            if (startNode.walkable && targetNode.walkable)
            {
                // Initialize set of nodes to be evaluated
                Heap<Node> openSet = new Heap<Node>(_grid.MaxSize);
                // Initialize set of nodes already evaluated
                HashSet<Node> closeSet = new HashSet<Node>();
                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirst();
                    closeSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        pathSuccess = true;
                        break;
                    }

                    foreach (Node neighbour in _grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.walkable || closeSet.Contains(neighbour))
                        {
                            continue;
                        }

                        int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                        if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, targetNode);
                            neighbour.parent = currentNode;

                            if (!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                            else 
                            {
                                openSet.UpdateItem(neighbour);
                            }
                        }
                    }
                }
            }

            if (pathSuccess)
            {
                wayPoints = RetracePath(startNode, targetNode);
                pathSuccess = wayPoints.Length > 0;
            }

            callback(new PathResult(wayPoints, pathSuccess, request.callback));
        }

        /// <summary>
        /// This function is responsible for retracing the game object path from given start to end nodes.
        /// </summary>
        private Vector3[] RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();

            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);

            return waypoints;
        }

        /// <summary>
        /// This function is responsible for simplifying the result path by deleting some useless points.
        /// </summary>
        private Vector3[] SimplifyPath(List<Node> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;

            for (int i = 1; i < path.Count; i++)
            {
                Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);

                if (directionNew != directionOld)
                {
                    waypoints.Add(path[i].worldPosition);
                }

                directionOld = directionNew;
            }

            return waypoints.ToArray();
        }

        /// <summary>
        /// This function is responsible for computing the distance between the two given nodes.
        /// </summary>
        private int GetDistance(Node nodeA, Node nodeB)
        {
            int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            if (distanceX > distanceY)
            {
                return 14 * distanceY + 10 * (distanceX - distanceY);
            }
            else
            {
                return 14 * distanceX + 10 * (distanceY - distanceX);
            }
        }

        #endregion
    }
}