using System;
using Agents;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "WaitForAnimation", story: "[Enemy] wait for animation", category: "Action/Animation", id: "f437fca2c175bc64dc335a47942fb03e")]
    public partial class WaitForAnimationAction : Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;

        private AgentTrigger _agentTrigger;
        private IRenderer _agentRenderer;
        private bool _isAnimationEnd;
        
        protected override Status OnStart()
        {
            if (Enemy.Value == null || Enemy.Value.Trigger == null)
                return Status.Failure;
            
            _isAnimationEnd = false;
            _agentTrigger = Enemy.Value.Trigger;
            _agentRenderer = Enemy.Value.AgentRenderer;
            _agentTrigger.OnAnimationEnd += HandleAnimationEnd;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            return _isAnimationEnd ? Status.Success : Status.Running;
        }

        protected override void OnEnd()
        {
            if (_agentTrigger != null)
                _agentTrigger.OnAnimationEnd -= HandleAnimationEnd;
        }

        private void HandleAnimationEnd()
        {
            if (_agentRenderer.Animator.IsInTransition(0)) return;
            _isAnimationEnd = true;
        }
    }
}

