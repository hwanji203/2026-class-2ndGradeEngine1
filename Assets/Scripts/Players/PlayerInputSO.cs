using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Players
{
    [CreateAssetMenu(fileName = "Player Input", menuName = "SO/Player Input", order = 0)]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        public event Action<Vector2> OnMovementChanged;
        public event Action OnAttackKeyPressed;
        public event Action OnSlideKeyPressed;

        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
                
            }
            _controls.Player.Enable();
        }

        private void OnDisable()
        {
            if(_controls != null)
                _controls.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 movementKey = context.ReadValue<Vector2>();
            OnMovementChanged?.Invoke(movementKey);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnAttackKeyPressed?.Invoke();
        }

        public void OnSlide(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnSlideKeyPressed?.Invoke();

        }
    }
}