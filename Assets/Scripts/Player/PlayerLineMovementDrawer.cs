using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(LineRenderer))]
    public class PlayerLineMovementDrawer : MonoBehaviour
    {
        [SerializeField] private Transform playerPosition;
        [SerializeField] private LayerMask layerToDrawLine;
        
        private LineRenderer _lineRenderer;
        private List<Vector3> _moveLinePoints;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _moveLinePoints = new List<Vector3>();
        }

        private void Update()
        {
            DrawMovementLine();
        }

        private void DrawMovementLine()
        {
            if (_moveLinePoints.Any() && _moveLinePoints.Last() == playerPosition.position)
                return;

            if (Physics2D.OverlapCircle(playerPosition.position, .2f, layerToDrawLine))
            {
                _moveLinePoints.Add(playerPosition.position);
                AssignDrawingPoints();
            }
            else
            {
                // TODO: Fill area with CloseGameArea tiles
                
                _moveLinePoints.Clear();
                AssignDrawingPoints();
            }
        }

        private void AssignDrawingPoints()
        {
            _lineRenderer.positionCount = _moveLinePoints.Count;
            _lineRenderer.SetPositions(_moveLinePoints.ToArray());
        }
    }
}