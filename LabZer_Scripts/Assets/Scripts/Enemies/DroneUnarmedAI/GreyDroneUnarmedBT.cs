using System.Collections.Generic;
using BehaviorTree;

/// <summary>
/// Class <c>GreyDroneUnarmedBT</c> is a BehaviorTree <c>Tree</c> instance representing the general behavior of a grey unarmed drone game object.
/// </summary>
public class GreyDroneUnarmedBT : Tree
{
    /// <summary>
    /// This function is responsible for setting up the tree, defining the game object behavior by assembling action & control flow nodes together.
    /// </summary>
    /// <returns>A BehaviorTree <c>Node</c> instance representing the setup node.</returns>
    protected override Node SetupTree()
    {
        Node root = new Sequence(new List<Node>
        {
            new CheckEnemyDeath(transform),
            new TaskApplyEnemyDeath(transform)
        });

        return root;
    }
}
