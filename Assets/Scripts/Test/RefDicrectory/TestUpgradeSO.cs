using System;
using System.Reflection;
using UnityEngine;

namespace Test.RefDicrectory
{
    [CreateAssetMenu(fileName = "Test Upgrade", menuName = "Test/Upgrade", order = 0)]
    public class TestUpgradeSO : ScriptableObject
    {
        public string targetFieldName;
        public float upgradeValue;
        public BindingFlags bindingFlags;

        public void Upgrade(RefTest target)
        {
            Type targetType = target.GetType(); // 타겟의 타입을 가져옴
            FieldInfo targetField = targetType.GetField(targetFieldName, bindingFlags); // 타겟 타입에서 필드를 가져옴
            
            float previousValue = (float)targetField.GetValue(target);
            
            targetField.SetValue(target, previousValue + upgradeValue); // 타겟 인스턴스의 필드 값을 업그레이드 값으로 설정
        }
        
    }
}