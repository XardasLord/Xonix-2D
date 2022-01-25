using Common;
using UnityEngine;

namespace Player.Commands
{
    public class MoveCommand : ICommand
    {
        private readonly GameObject _gameObject;
        private readonly Vector3 _movementDirection;
        private Vector3 _previousLocation;

        public MoveCommand(GameObject gameObject, Vector3 movementDirection)
        {
            _gameObject = gameObject;
            _movementDirection = movementDirection;
        }
        
        public void Execute()
        {
            _previousLocation = _gameObject.transform.position;
            
            _gameObject.transform.Translate(_movementDirection);
        }

        public void Undo()
        {
            _gameObject.transform.Translate(_previousLocation);
        }
    }
}