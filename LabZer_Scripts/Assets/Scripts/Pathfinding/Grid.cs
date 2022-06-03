using System;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace Pathfinding
{
    /// <summary>
    /// Class <c>Grid</c> is a Unity script used to manage the different pathfinding properties and behaviors of a level ground game object.
    /// </summary>
    public class Grid : MonoBehaviour
    {
        #region Fields / Properties

        /// <summary>
        /// Instance field <c>displayGridGizmos</c> represents the display grid gizmos status of the grid.
        /// </summary>
        [SerializeField]
        private bool _displayGridGizmos;

        /// <summary>
        /// Instance field <c>unwalkableMask</c> is a Unity <c>LayerMask</c> structure representing the different layers to consider to find unwalkable game objects. 
        /// </summary>
        [SerializeField]
        private LayerMask _unwalkableMask;

        /// <summary>
        /// Instance field <c>nodeRadius</c> represents the radius value of a grid's node.
        /// </summary>
        [SerializeField]
        private float _nodeRadius;

        /// <summary>
        /// Instance field <c>grid</c> is an array of Pathfinding <c>Node</c> instances representing the different nodes of the grid.
        /// </summary>
        private Node[,] _grid;

        /// <summary>
        /// Instance field <c>nodeDiameter</c> represents the diameter value of a grid's node.
        /// </summary>
        private float _nodeDiameter;

        /// <summary>
        /// Instance field <c>gridSizeX</c> represents size of the grid over the X axis.
        /// </summary>
        private int _gridSizeX;

        /// <summary>
        /// Instance field <c>gridSizeY</c> represents size of the grid over the Y axis.
        /// </summary>
        private int _gridSizeY;

        /// <summary>
        /// Instance field <c>_gridOffset</c> is a Unity <c>Vector2</c> structure representing the position coordinates of the grid.
        /// </summary>
        private Vector2 _gridOffset;

        /// <summary>
        /// Instance field <c>gridWorldSize</c> is a Unity <c>Vector2</c> structure representing the width and length of the grid.
        /// </summary>
        public Vector2 gridWorldSize;

        /// <summary>
        /// Instance field <c>MaxSize</c> represents the max size number of points of the pathfinding grid.
        /// </summary>
        public int MaxSize
        {
            get
            {
                return _gridSizeX * _gridSizeY;
            }
        }

        #endregion

        #region MonoBehavior

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _gridOffset = transform.position;
            _nodeDiameter = _nodeRadius * 2;
            // Compute number of grid points alongside X and Y axis.
            _gridSizeX = Mathf.RoundToInt(gridWorldSize.x / _nodeDiameter);
            _gridSizeY = Mathf.RoundToInt(gridWorldSize.y / _nodeDiameter);

            CreateGrid();
        }
        

        /// <summary>
        /// Callback to draw gizmos that are pickable and always drawn.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

            if (_grid != null && _displayGridGizmos)
            {
                foreach (Node node in _grid)
                {
                    Gizmos.color = node.walkable ? Color.white : Color.red;
                    Gizmos.DrawSphere(node.worldPosition, _nodeRadius);
                }
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// This function is responsible for creating the grid.
        /// </summary>
        private void CreateGrid()
        {
            _grid = new Node[_gridSizeX, _gridSizeY];

            Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3.up * (y * _nodeDiameter + _nodeRadius);
                    bool walkable = !Physics2D.OverlapCircle(worldPoint, _nodeRadius, _unwalkableMask);

                    _grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// This function is responsible for getting the neighbour nodes of the given node.
        /// </summary>
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                    {
                        neighbours.Add(_grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

        /// <summary>
        /// This function is responsible for getting the grid's node instance from a given world point position.
        /// </summary>
        public Node GetNodeFromWorldPoint(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x - _gridOffset.x) / gridWorldSize.x + 0.5f;
            float percentY = (worldPosition.y - _gridOffset.y) / gridWorldSize.y + 0.5f;

            int x = Mathf.FloorToInt(Mathf.Clamp((_gridSizeX) * percentX, 0, _gridSizeX - 1));
            int y = Mathf.FloorToInt(Mathf.Clamp((_gridSizeY) * percentY, 0, _gridSizeY - 1));

            return _grid[x, y];
        }

        /// <summary>
        /// <c>TerrainType</c> is a Pathfinding structure used to store the different level terrain type properties.
        /// </summary>
        [Serializable]
        public struct TerrainType
        {
            public LayerMask terrainMask;
            public int terrainPenalty;
        }

        /// <summary>
        /// This function is called on level obstacles destroyed.
        /// </summary>
        public void OnObstacleDestroyed(BoxCollider2D obstacleCollider)
        {
            float minXWorldPoint = obstacleCollider.bounds.min.x;
            float minYWorldPoint = obstacleCollider.bounds.min.y;

            float maxXWorldPoint = obstacleCollider.bounds.max.x + _nodeDiameter;
            float maxYWorldPoint = obstacleCollider.bounds.max.y + _nodeDiameter;

            float x = minXWorldPoint;
            while (x <= maxXWorldPoint)
            {
                float y = minYWorldPoint;
                while (y <= maxYWorldPoint)
                {
                    Node node = GetNodeFromWorldPoint(new Vector3(x, y, 0f));
                    node.walkable = true;

                    y += _nodeDiameter;
                }
                x += _nodeDiameter;
            }
        }

        #endregion
    }
}

