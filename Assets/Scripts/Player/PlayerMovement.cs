using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Range(1f, 20f)]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private LayerMask layerStopsMovement;

        private bool _isMoving;
        private Vector3 _targetPosition;

        private void Start()
        {
            _targetPosition = transform.position;
        }

        private void Update()
        {
            if (_isMoving)
                MoveTowardsToTargetPosition();
            else
                SetNewTargetPositionFromInput();
        }
        
        private void MoveTowardsToTargetPosition()
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, moveSpeed * Time.deltaTime);

            if (transform.position == _targetPosition)
                _isMoving = false;
        }

        private void SetNewTargetPositionFromInput()
        {
            var inputX = Input.GetAxisRaw("Horizontal");
            var inputY = Input.GetAxisRaw("Vertical");

            if (inputX != 0f)
            {
                var direction = new Vector3(inputX, 0f);

                if (CanMove(direction))
                {
                    _targetPosition += direction;
                    _isMoving = true;
                }
            } 
            else if (inputY != 0f)
            {
                var direction = new Vector3(0f, inputY);

                if (CanMove(direction))
                {
                    _targetPosition += direction;
                    _isMoving = true;
                }
            }
        }

        private bool CanMove(Vector3 direction) 
            => !Physics2D.OverlapCircle(transform.position + direction, .2f, layerStopsMovement);
    }
}