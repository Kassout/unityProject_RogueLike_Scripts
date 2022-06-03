using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>TaskAssertUnitSwarmCirclePosition</c> is a BehaviorTree <c>Node</c> used to define an assert unit swarm circle position task behavior.
/// </summary>
public class TaskAssertUnitSwarmCirclePosition : Node
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>resizeSpeed</c> represents the speed value for the swarm's units to resize to a smaller geometry after one get lost.
    /// </summary>
    private float _resizeSpeed;

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
    public TaskAssertUnitSwarmCirclePosition(Transform transform, float resizeSpeed, List<Transform> swarmUnits)
    {
        _transform = transform;
        _resizeSpeed = resizeSpeed;
        _swarmUnits = swarmUnits;
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        for (int i = 0; i < _swarmUnits.Count; i++)
        {
            Vector3 circlePosition = new Vector3();
            circlePosition.x = _swarmUnits.Count / 4f * Mathf.Cos(Mathf.Deg2Rad * 360 / _swarmUnits.Count * i);
            circlePosition.y = _swarmUnits.Count / 4f * Mathf.Sin(Mathf.Deg2Rad * 360 / _swarmUnits.Count * i);
            circlePosition.z = 0f;

            if (_swarmUnits[i].localPosition != circlePosition)
            {
                _swarmUnits[i].localPosition = Vector3.MoveTowards(_swarmUnits[i].localPosition, circlePosition, _resizeSpeed * Time.deltaTime);
            }
        }

        state = NodeState.Running;
        return state;
    }

    #endregion
}