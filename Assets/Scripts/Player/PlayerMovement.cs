using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private Transform movePoint;
        [SerializeField] private LayerMask whatStopsMovement;

        private void Start()
        {
            movePoint.parent = null;
        }

        private void Update()
        {
            MoveToMovementPosition();

            if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
            {
                SetMovementPosition();
            }
        }

        private void MoveToMovementPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        }

        private void SetMovementPosition()
        {
            var inputX = Input.GetAxisRaw("Horizontal");
            var inputY = Input.GetAxisRaw("Vertical");

            if (inputX != 0f)
            {
                var movePosition = movePoint.position + new Vector3(inputX, 0f);

                if (!Physics2D.OverlapCircle(movePosition, .2f, whatStopsMovement))
                {
                    movePoint.position = movePosition;
                }
            }
            else if (inputY != 0f)
            {
                var movePosition = movePoint.position + new Vector3(0f, inputY);

                if (!Physics2D.OverlapCircle(movePosition, .2f, whatStopsMovement))
                {
                    movePoint.position = movePosition;
                }
            }
        }
    }
}