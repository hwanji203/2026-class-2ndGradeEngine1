using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace Test.ReflectionTest
{
    [Serializable]
    public class MyClass
    {
        public int age;
        public string name;
        private int number = 0;

        public void Introduce()
        {
            UnityEngine.Debug.Log($"나이 : {age}, 이름 : {name}");
            number++;
        }
    }

    public class RefTest : MonoBehaviour
    {
        [SerializeField] private MyClass myClass = new();
        [SerializeField] private int ageValue;
        [SerializeField] private string nameValue;

        [ContextMenu("Test excute")]
        private void TestExcute()
        {
            Type t = typeof(MyClass);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int dummy = 40;
            for (int i = 0; i < 50000; i++)
            {
                (myClass.age, dummy) = (dummy, myClass.age);
            }
            stopwatch.Stop();

            UnityEngine.Debug.Log($"경과 시간 : {stopwatch.ElapsedMilliseconds}ms, 경과틱 : {stopwatch.ElapsedTicks}");

            stopwatch.Start();

            FieldInfo fieldInfo = t.GetField("age");
            for (int i = 0; i < 50000; i++)
            {
                int temp = (int)fieldInfo.GetValue(myClass);
                fieldInfo.SetValue(myClass, dummy);
                dummy = temp;
            }

            stopwatch.Stop();
            UnityEngine.Debug.Log($"<color=red>리플렉션</color> - 경과 시간 : {stopwatch.ElapsedMilliseconds}ms, 경과틱 : {stopwatch.ElapsedTicks}");

            //fieldInfo.GetValue()
        }
    }

}

