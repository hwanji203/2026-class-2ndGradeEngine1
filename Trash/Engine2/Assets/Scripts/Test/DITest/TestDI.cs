using GGMLib.DISystems;
using UnityEngine;

namespace Test.DITest
{
    public class TestDI : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private EnemyManager enemyManager;

        [Inject]
        public void Init(GameManager gameManager, EnemyManager enemyManager)
        {
            this.gameManager = gameManager;
            this.enemyManager = enemyManager;
        }
    }
}