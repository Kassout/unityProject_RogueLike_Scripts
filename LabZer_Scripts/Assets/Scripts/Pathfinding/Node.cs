
using UnityEngine;

namespace Pathfinding
{
    /// <summary>
    /// <c>Node</c> is a Pathfinding class containing the different elements defining the walkable ability of a position on a pathfinding grid.
    /// </summary>
    public class Node : IHeapItem<Node>
    {
        #region Fields / Properties

        /// <summary>
        /// Instance field <c>heapIndex</c> represents the index number of the current node inside his heap container.
        /// </summary>az
        private int _heapIndex;

        /// <summary>
        /// Instance field <c>walkable</c> represents the walkable status of the node.
        /// </summary>
        public bool walkable;

        /// <summary>
        /// Instance field <c>worldPosition</c> is a Unity <c>Vector3</c> structure representing the world position coordinates of the node.
        /// </summary>
        public Vector3 worldPosition;

        /// <summary>
        /// Instance field <c>gridX</c> represents the x coordinate value of the node on the grid.
        /// </summary>
        public int gridX;

        /// <summary>
        /// Instance field <c>gridY</c> represents the y coordinate value of the node on the grid.
        /// </summary>
        public int gridY;

        /// <summary>
        /// Instance field <c>parent</c> is a Pathfinding <c>Node</c> structure representing the parent node of the current node.
        /// </summary>
        public Node parent;

        /// <summary>
        /// Instance field <c>gCost</c> represents the exact cost value of the path from the starting node to the current node.
        /// </summary>
        public int gCost;

        /// <summary>
        /// Instance field <c>hCost</c> represents the heuristic estimated cost value from the current node to the target node.
        /// </summary>
        public int hCost;

        /// <summary>
        /// Instance property <c>fCost</c> represents an estimated cost of the path from starting node to target node by addition of the gCost and the hCost.
        /// </summary>
        private int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
        {
            walkable = _walkable;
            worldPosition = _worldPos;
            gridX = _gridX;
            gridY = _gridY;
        }

        #endregion

        #region IHeapItem

        /// <summary>
        /// Instance property <c>HeapIndex</c> represents the index number of the current node inside the heap container.
        /// </summary>
        public int HeapIndex
        {
            get
            {
                return _heapIndex;
            }
            set
            {
                _heapIndex = value;
            }
        }

        /// <summary>
        /// This function is responsible for compairing two nodes, return the node with the best values.
        /// </summary>
        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }

            return -compare;
        }

        #endregion
    }
}

