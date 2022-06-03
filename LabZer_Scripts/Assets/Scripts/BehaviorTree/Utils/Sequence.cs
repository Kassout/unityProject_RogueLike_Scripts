using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// Class <c>Selector</c> is a BehaviorTree <c>Node</c> instance used to find and execute the first child that has not yet succeeded.
    /// </summary>
    public class Sequence : Node
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Sequence() : base() { }

        /// <summary>
        /// Constructor with list of child nodes as parameters.
        /// </summary>
        public Sequence(List<Node> children) : base(children) { }

        /// <summary>
        /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
        /// </summary>
        /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        state = NodeState.Failure;
                        return state;
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = NodeState.Success;
                        return state;
                }
            }

            state = anyChildIsRunning ? NodeState.Running : NodeState.Success;
            return state;
        }
    }
}

