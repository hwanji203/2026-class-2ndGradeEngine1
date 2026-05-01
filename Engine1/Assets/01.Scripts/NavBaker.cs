using Unity.AI.Navigation;
using UnityEngine;

namespace _01.Scripts
{
    public class NavBaker : MonoBehaviour
    {
        private NavMeshSurface _surface;

        private void Awake()
        {
            _surface = GetComponent<NavMeshSurface>();
        }

        public void ReBakeMesh()
        {
            _surface.BuildNavMesh();
        }
    }
}