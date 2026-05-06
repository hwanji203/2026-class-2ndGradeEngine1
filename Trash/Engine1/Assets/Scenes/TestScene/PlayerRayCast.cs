using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scenes.TestScene
{
    public class PlayerRayCast : MonoBehaviour
    {
        [SerializeField] private float rayCastLength = 1000;
        private LineRenderer _lineRenderer;
        private List<(Vector3, Vector3)> _gizmoPosition = new();
        private Vector3[] _linePositions;
        
        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        public void RayCast(Ray firstRay)
        {
            _gizmoPosition.Clear();
            float length = rayCastLength;
            Ray ray = firstRay;
            Ray(ref length, ray);

            _linePositions = new Vector3[_gizmoPosition.Count + 1];
            for (int i = 0; i < _gizmoPosition.Count; i++)
            {
                _linePositions[i] = _gizmoPosition[i].Item1;
            }
            _linePositions[_gizmoPosition.Count] = _gizmoPosition[_gizmoPosition.Count - 1].Item2;
            DrawLine();
        }

        public void RayCast()
        {
            RayCast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));
        }

        private void DrawLine()
        {
            _lineRenderer.positionCount = _linePositions.Length;
            _lineRenderer.SetPositions(_linePositions);
        }

        private void Ray(ref float length, Ray ray)
        {
            if (length <= 0)
                return;
            if (Physics.Raycast(ray, out RaycastHit hit, length))
            {
                Vector3 hitPoint = hit.point;
                length -= hit.distance;
                _gizmoPosition.Add((ray.origin, hitPoint));
                Ray(ref length, new(hit.point, Vector3.Reflect(ray.direction, hit.normal)));
            }
            else
            {
                _gizmoPosition.Add((ray.origin, ray.origin + ray.direction * length));
                length = 0;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (var position in _gizmoPosition)
            {
                Gizmos.DrawLine(position.Item1, position.Item2);
            }
        }
    }
}
