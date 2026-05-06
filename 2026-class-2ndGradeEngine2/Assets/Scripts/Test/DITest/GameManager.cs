using GGMLib.DISystems;
using UnityEngine;

namespace Test.DITest
{
    public class GameManager : MonoBehaviour, IDependencyProvider
    {
        [Provide]
        public GameManager GenerateGameManager()
        {
            return this;
        }
    }
}