using Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StopAgent", story: "Stop [Enemy]", category: "Action/Navigation", id: "12eb9b9c9460997451abc0a95c75b1c2")]
public partial class StopAgentAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;

    protected override Status OnStart()
    {
        if (Enemy.Value == null || Enemy.Value.NavMovement == null)
            return Status.Failure;
        
        Enemy.Value.NavMovement.StopImmediately();
        return Status.Running;
    }
}

