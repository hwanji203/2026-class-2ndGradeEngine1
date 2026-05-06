using System;
using CombatSystem;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "UseSkill", story: "[Enemy] use [SkillNumber] to [TargetGameObject]", category: "Action/Combat", id: "e8fdbb072513fd780107f3627db38e88")]
    public partial class UseSkillAction : Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
        [SerializeReference] public BlackboardVariable<int> SkillNumber;
        [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;

        private ISkillModule _skillModule;
        private bool _isSkillEnd;
        
        protected override Status OnStart()
        {
            if (Enemy.Value == null || SkillNumber.Value < 0)
            {
                Debug.LogError("Can use skill의 기본 값이 설정되지 않았습니다.");
                return Status.Failure;
            }
            
            _skillModule = Enemy.Value.SkillModule;
            
            _isSkillEnd = false;
            _skillModule.OnCurrentSkillEnd += HandleSkillEnd;
            _skillModule.UseSkill(SkillNumber.Value, TargetGameObject.Value);
            return Status.Running;
        }

        private void HandleSkillEnd()
        {
            _skillModule.OnCurrentSkillEnd -= HandleSkillEnd;
            _isSkillEnd = true;
        }

        protected override Status OnUpdate()
        {
            return _isSkillEnd ? Status.Success : Status.Running; //Fail 아니다
        }

        protected override void OnEnd()
        {
            if (_skillModule != null)
                _skillModule.OnCurrentSkillEnd -= HandleSkillEnd;
        }
    }
}

