using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Enemy;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Player
{
    [RequireComponent(typeof(LineRenderer))]
    public class PlayerLineMovementDrawer : MonoBehaviour
    {
        [SerializeField] private Transform playerPosition;

        [Header("Tiles")]
        [SerializeField] private Tilemap openGameAreaTileMap;
        [SerializeField] private Tilemap closeGameAreaTileMap;
        [SerializeField] private Tile closeTile;

        [Header("Events")] 
        [SerializeField] private VoidEvent gameAreaClosedEvent;
        
        private LineRenderer _lineRenderer;
        private List<Vector3> _moveLinePoints;
        private bool _isDrawing;

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

            if (openGameAreaTileMap.HasTile(Vector3Int.FloorToInt(transform.position)))
            {
                _moveLinePoints.Add(playerPosition.position);
                AssignDrawingPoints();
                
                _isDrawing = true;
            }
            else if (_isDrawing)
            {
                SwapOpenTilesToCloseTiles();
                
                gameAreaClosedEvent.Raise();

                _moveLinePoints.Clear();
                AssignDrawingPoints();
                
                _isDrawing = false;
            }
        }

        private void SwapOpenTilesToCloseTiles()
        {
            var playerMovementPositionsToSwap = _moveLinePoints
                .Select(Vector3Int.FloorToInt)
                .Distinct()
                .ToArray();

            foreach (var pos in playerMovementPositionsToSwap)
            {
                openGameAreaTileMap.SetTile(pos, null);
                closeGameAreaTileMap.SetTile(pos, closeTile);
            }

            var openTileAreas = openGameAreaTileMap.GetTileAreas();

            var smallestArea = new List<Vector3Int>();
            foreach (var openTileArea in openTileAreas)
            {
                if (!smallestArea.Any() || smallestArea.Count > openTileArea.Count)
                {
                    // If any enemy is in this area then continue
                    var enemies = FindObjectsOfType<EnemyMovement>();
                    var enemyExistInArea = enemies.Any(x =>
                    {
                        var currentEnemyPosition = Vector3Int.FloorToInt(x.transform.position);

                        return openTileArea.Any(tile => tile == currentEnemyPosition);
                    });
                    if (!enemyExistInArea)
                    {
                        smallestArea = openTileArea;
                    }
                }
            }

            if (smallestArea.Any())
            {
                openGameAreaTileMap.FloodFill(smallestArea.First(), null);
                closeGameAreaTileMap.FloodFill(smallestArea.First(), closeTile);
            }
            
            openGameAreaTileMap.CompressBounds();
        }

        private void AssignDrawingPoints()
        {
            _lineRenderer.positionCount = _moveLinePoints.Count;
            _lineRenderer.SetPositions(_moveLinePoints.ToArray());
        }
    }
}