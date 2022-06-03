using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// Class <c>Tree</c> is a BehaviorTree component used to build a structure of nodes defining a game object behavior.
    /// </summary>
    public abstract class Tree : MonoBehaviour
    {
        #region Fields / Properties

        /// <summary>
        /// Field <c>root</c> is a BehaviorTree <c>Node</c> instance representing the root node of the tree.
        /// </summary>
        private Node _root = null;

        #endregion

        #region MonoBehavior

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected void Start()
        {
            _root = SetupTree();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (_root != null)
            {
                _root.Evaluate();
            }
        }

        #endregion

        #region Abstract

        /// <summary>
        /// This function is responsible for setting up the tree, defining the game object behavior by assembling action & control flow nodes together.
        /// </summary>
        /// <returns>A BehaviorTree <c>Node</c> instance representing the setup node.</returns>
        protected abstract Node SetupTree();

        #endregion
    }
}