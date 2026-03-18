using System;
using System.Reflection;
using UnityEngine;

namespace Test.ReflectionTest
{
    [CreateAssetMenu(fileName = "Test upgrade", menuName = "Test/Ugrade", order = 0)]
    public class TestUpgradeSO : ScriptableObject
    {
        public string targetFieldName;
        public float upgradeValue;
        public BindingFlags bindingFlags;

        public void Upgrade(RefTest target)
        {
            Type targetType = target.GetType();
            FieldInfo targetField = targetType.GetField(targetFieldName, bindingFlags);

            targetField.SetValue(target, upgradeValue);
        }
    }
}

