using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>TaskRotateSwarm</c> is a BehaviorTree <c>Node</c> used to define a rotate swarm task behavior.
/// </summary>
public class TaskRotateSwarm : Node
{
    #region Fields / Properties
    
    /// <summary>
    /// Instance field <c>rotationSpeed</c> represents the speed value of the swarm rotation.
    /// </summary>
    private float _rotationSpeed;

    /// <summary>
    /// Instance field <c>transform</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the swarm.
    /// </summary>
    private Transform _transform;

    /// <summary>
    /// Instance field <c>swarmUnits</c> is a list of Unity <c>Trasnform</c> component representing the different position, rotation and scale of the swarm's units.
    /// </summary>
    private List<Transform> _swarmUnits;

    #endregion

    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public TaskRotateSwarm(Transform transform, float rotationSpeed, List<Transform> swarmUnits)
    {
        _transform = transform;
        _rotationSpeed = rotationSpeed;
        _swarmUnits = swarmUnits;
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        _transform.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime);

        for (int i = 0; i < _swarmUnits.Count; i++)
        {
            _swarmUnits[i].Rotate(0f, 0f, -_rotationSpeed * Time.deltaTime);
        }

        state = NodeState.Running;
        return state;
    }

    #endregion
}