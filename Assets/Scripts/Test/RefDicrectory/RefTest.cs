using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Test.RefDicrectory
{
    [Serializable]
    public class MyClass
    {
        public int age;
        public string name;
        private int number = 0;

        // public MyClass(int age, string name, int number)
        // {
        //     this.age = age;
        //     this.name = name;
        //     this.number = number;
        // }
        //
        public void Introduce()
        {
            Debug.Log($"나이: {age}, 이름: {name}");
            number++;
        }
    }
    
    public class RefTest : MonoBehaviour
    {
        [SerializeField] private MyClass myClass = new();
        [SerializeField] private int ageValue;
        [SerializeField] private string nameValue;

        //
        // [SerializeField] private float _strength;
        // [SerializeField] private float _damage;
        // [SerializeField] private float _health;
        // [SerializeField] private float _moveSpeed;
        //
        // [SerializeField] private TestUpgradeSO upgrade;
        //
        // [ContextMenu("Execute upgrade")]
        // private void ExecuteUpgrade()
        // {
        //     upgrade.Upgrade(this);
        // }
        
        [ContextMenu("Test Execute")]
        private void TestExecute()
        {
            Type t = typeof(MyClass);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int dummy = 40;
            for (int i = 0; i < 50000; i++)
            {
                (myClass.age, dummy) = (dummy, myClass.age); // 튜플을 이용한 값 교환
            }
            stopwatch.Stop();

            Debug.Log($"경과 시간: {stopwatch.ElapsedMilliseconds}ms, 경과 틱: {stopwatch.ElapsedTicks}");
            
                FieldInfo fieldInfo = t.GetField("age");
            stopwatch.Start();
            for (int i = 0; i < 50000; i++)
            {
                // 기본 자료형은 스택에서 값 복사가 일어남
                int temp = (int)fieldInfo.GetValue(myClass); // 리플렉션을 이용한 값 가져오기
                fieldInfo.SetValue(myClass, dummy); // 리플렉션을 이용한 값 설정
                dummy = temp;
            }
            stopwatch.Stop();
            Debug.Log($"<color=red> 리플렉션</color> - 경과 시간: {stopwatch.ElapsedMilliseconds}ms, 경과 틱: {stopwatch.ElapsedTicks}");

            // Type myClassType = Type.GetType("Test.RefDicrectory.MyClass"); // 오타 시 존나게 위험함
            // myClass = Activator.CreateInstance(myClassType, ageValue, nameValue, 0) as MyClass; // 오브젝트 배열이기에 생성자 매개변수가 하나일 때 오브젝트 배열로 캐스팅 할 때 에러남 -> 오브젝트 배열로 캐스팅할 때는 매개변수 하나라도 배열로 만들어야 함
            // myClass = new MyClass(ageValue, nameValue, 0);



            // MyClass myC = new MyClass { age = 30, name = "홍길동" };
            // Type type = myClass.GetType(); // Type: 타입 정보 저장

            // 필드 이름, 자료형, 접근제한자, 값 X (값은 인스턴스)
            // FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance); // 비트 연산으로 여러 플래그를 조합하여 사용
            // foreach (var field in fields)
            // {
            //      Debug.Log($"<Field 이름>: {field.Name}, <Field 타입>: {field.FieldType.Name}"); 
            // }
            //
            // FieldInfo ageField = type.GetField("age");
            // Debug.Log(ageField.GetValue(myClass)); // myClass 인스턴스의 ageField의 값을 가져옴
            // ageField.SetValue(myClass, ageValue); // myClass 인스턴스에 ageField의 값을 ageValue로 변경
        }
    }
}