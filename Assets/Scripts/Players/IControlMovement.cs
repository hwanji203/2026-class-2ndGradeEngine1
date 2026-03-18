using UnityEngine;

namespace Players
{
    public interface IControlMovement
    {
        void SetMovementDirection(Vector2 inputDirection);
    }
}