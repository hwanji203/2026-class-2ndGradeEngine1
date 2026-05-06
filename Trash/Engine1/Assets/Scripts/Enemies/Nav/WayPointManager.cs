using UnityEngine;

namespace Enemies.Nav
{
    public class WayPointManager : MonoBehaviour
    {
        [SerializeField] private WayPoint[] wayPoints;
        
        public WayPoint this[int index] => wayPoints[index];

        public int GetClosestPointIndexFromPosition(Vector3 position)
        {
            float minDistance = Mathf.Infinity;
            int closestIndex = -1;

            for (int i = 0; i < wayPoints.Length; i++)
            {
                Vector3 wayPointPosition = wayPoints[i].Position;
                float distance = (wayPointPosition - position).sqrMagnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }
        
        public int GetNextWayPointIndex(int currentIndex) => (currentIndex + 1) % wayPoints.Length;
    }
}