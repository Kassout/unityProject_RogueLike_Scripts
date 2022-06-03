using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// Enum field <c>NodeState</c> represents the different state taken by a node.
    /// </summary>
    public enum NodeState
    {
        Running,
        Success,
        Failure
    }

    /// <summary>
    /// Class <c>Node</c> is a BehaviorTree component used to define a task or a control flow.
    /// </summary>
    public abstract class Node
    {
        #region Fields / Properties

        /// <summary>
        /// Field <c>state</c> is a BehaviorTree <c>NodeState</c> instance representing the current state of the node.
        /// </summary>
        protected NodeState state;

        /// <summary>
        /// Field <c>parent</c> is a BehaviorTree <c>Node</c> instance representing the parent's node of the node.
        /// </summary>
        public Node parent;

        /// <summary>
        /// Field <c>children</c> is a list of BehaviorTree <c>Node</c> instances representing the different children's node of the node.
        /// </summary>
        protected List<Node> children = new List<Node>();

        /// <summary>
        /// Instance field <c>dataContext</c> represents a data dictionnary of the different information context of the node.
        /// </summary>
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Node()
        {
            parent = null;
        }

        /// <summary>
        /// Constructor with list of child nodes as parameters.
        /// </summary>
        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                _Attach(child);
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// This function is responsible for attaching a node as a child to the node.
        /// </summary>
        /// <param name="node">A Pathfinding <c>Node</c> instance to attach as a child of the current node.</param>
        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        #endregion

        #region Public

        /// <summary>
        /// This function is responsible for setting up information context to the data dictionnary of the node.
        /// </summary>
        /// <param name="key">A string value representing the name of the object data to store inside the node context.</param>
        /// <param name="value">An object representing data to store inside the node context.</param>
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        /// <summary>
        /// This function is responsible for setting up information context to the data dictionnary of the root node.
        /// </summary>
        /// <param name="key">A string value representing the name of the object data to store inside the root node context.</param>
        /// <param name="value">An object representing data to store inside the root node context.</param>
        public void SetDataRoot(string key, object value)
        {
            Node rootNode = this;
            while (rootNode.parent != null)
            {
                rootNode = rootNode.parent;
            }
            rootNode._dataContext[key] = value;
        }

        /// <summary>
        /// This function is responsible for getting information context of the data dictionnary of the node, given his key name.
        /// </summary>
        /// <param name="key">A string value representing the name of the object data to look for inside the node context.</param>
        /// <returns>An object representing the node information context associated with the given key.</returns>
        public object GetData(string key)
        {
            object value = null;

            if (_dataContext.TryGetValue(key, out value))
            {
                return value;
            }

            Node node = parent;

            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                {
                    return value;
                }

                node = node.parent;
            }

            return null;
        }

        /// <summary>
        /// This function is responsible for getting information context of the given type, of the data dictionnary of the node, given his key name.
        /// </summary>
        /// <param name="key">A string value representing the name of the object data to look for inside the node context.</param>
        /// <returns>A T type object representing the node information context associated with the given key.</returns>
        public T GetData<T>(string key)
        {
            object value = null;
            T result = default(T);

            if (_dataContext.TryGetValue(key, out value))
            {
                try
                {
                    result = (T)value;

                    return result;
                }
                catch (InvalidCastException)
                {
                    Debug.LogError(key + " object is not of a appropriate requested type.");
                    throw;
                }
            }

            Node node = parent;

            while (node != null)
            {
                result = node.GetData<T>(key);
                if (result != null)
                {
                    return result;
                }

                node = node.parent;
            }

            return default(T);
        }

        /// <summary>
        /// This function is responsible for trying to get information context of the given type, of the data dictionnary of the node, given his key name.
        /// </summary>
        /// <param name="key">A string value representing the name of the object data to look for inside the node context.</param>
        /// <param name="result">An object representing the data associated with the given name inside the node context.</param>
        /// <returns>A boolean value representing the result existance status.</returns>
        public bool TryGetData<T>(string key, out T result)
        {
            result = default(T);
            object value = null;
            bool foundData = false;

            if (_dataContext.TryGetValue(key, out value))
            {
                try
                {
                    result = (T)value;

                    foundData = true;
                    return foundData;
                }
                catch (InvalidCastException)
                {
                    result = default(T);
                }
            }

            Node node = parent;

            while (node != null)
            {
                foundData = node.TryGetData<T>(key, out result);
                if (foundData)
                {
                    return foundData;
                }

                node = node.parent;
            }

            return false;
        }

        /// <summary>
        /// This function is responsible for clearing information context of the data dictionnary of the node, given his key name.
        /// </summary>
        /// <param name="key">A string value representing the name of the object data to clear from the node context.</param>
        /// <returns>A boolean value representing success status of the clearing operation.</returns>
        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;

            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                {
                    return true;
                }

                node = node.parent;
            }

            return false;
        }

        #endregion

        #region Virtual

        /// <summary>
        /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
        /// </summary>
        /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
        public virtual NodeState Evaluate() => NodeState.Failure;

        #endregion
    }
}