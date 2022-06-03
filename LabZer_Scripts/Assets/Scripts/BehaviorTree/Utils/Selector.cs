using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// Class <c>Selector</c> is a BehaviorTree <c>Node</c> instance used to find and execute the first child node that does not fail.
    /// </summary>
    public class Selector : Node
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Selector() : base() { }

        /// <summary>
        /// Constructor with list of child nodes as parameters.
        /// </summary>
        public Selector(List<Node> children) : base(children) { }

        /// <summary>
        /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
        /// </summary>
        /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        continue;
                    case NodeState.Success:
                        state = NodeState.Success;
                        return state;
                    case NodeState.Running:
                        state = NodeState.Running;
                        return state;
                    default:
                        continue;
                }
            }

            state = NodeState.Failure;
            return state;
        }
    }

}
