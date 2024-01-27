using BlockSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Input
{
    public class InputHandler
    {
        private GridStateController _gridStateController;

        [Inject]
        private void Construct(GridStateController gridStateController)
        {
            _gridStateController = gridStateController;
        }

        public void OnMove(InputAction.CallbackContext obj)
        {
            var direction = obj.ReadValue<Vector2>();
            _gridStateController.MoveElements(direction);
        }
    }
}