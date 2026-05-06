using System;
using Unity.Behavior;
using UnityEngine;

namespace Enemies.BT.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "CanUseSkill", story: "[Enemy] can use [SkillNumber] to [TargetGameObject]", category: "Conditions", id: "cbd5ef26562551bb9275b44bee35952b")]
    public partial class CanUseSkillCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
        [SerializeReference] public BlackboardVariable<int> SkillNumber;
        [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;

        public override bool IsTrue()
        {
            if (Enemy.Value == null || SkillNumber.Value < 0 || TargetGameObject.Value == null)
            {
                Debug.LogError("Can use Skill 의 컨디션 조건이 잘못되었습니다.");
                return false;
            }
            return Enemy.Value.SkillModule.CanUseSkill(SkillNumber.Value, TargetGameObject.Value);
        }
    }
}
