using UnityEngine;

namespace Players
{
    public interface IControlMovement
    {
        bool CanManualMove { get; set; }
        void SetMovementDirection(Vector2 inputDirection);
        void SetMovementVelocity(Vector3 velocity);
    }
}