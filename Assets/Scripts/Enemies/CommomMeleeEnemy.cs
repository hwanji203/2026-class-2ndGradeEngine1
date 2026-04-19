using UnityEngine;

namespace Enemies
{
    public class CommonMeleeEnemy : AbstractEnemy
    {
        [SerializeField] private Vector3[] pointsOfInterest; //패트롤 할 지점들

        private int _currentPointIndex;

        private void Start()
        {
            NavMovement.SetDestination(pointsOfInterest[_currentPointIndex]);
        }

        private void Update()
        {
            if (NavMovement.IsArrived)
            {
                _currentPointIndex = (_currentPointIndex + 1) % pointsOfInterest.Length;
                NavMovement.SetDestination(pointsOfInterest[_currentPointIndex]);
            }
        }
    }
}