using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Enemy;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Player
{
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
        private EdgeCollider2D _edgeCollider;
        private List<Vector3> _moveLinePoints;
        private bool _isDrawing;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _edgeCollider = GetComponent<EdgeCollider2D>();
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

            if (openGameAreaTileMap.HasTile(Vector3Int.FloorToInt(playerPosition.position)))
            {
                _moveLinePoints.Add(playerPosition.position);
                DrawLineForPoints();
                SetLineCollider();
                
                _isDrawing = true;
            }
            else if (_isDrawing)
            {
                SwapOpenTilesToCloseTiles();
                
                gameAreaClosedEvent.Raise();

                _moveLinePoints.Clear();
                DrawLineForPoints();
                SetLineCollider();
                
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

        private void DrawLineForPoints()
        {
            _lineRenderer.positionCount = _moveLinePoints.Count;
            _lineRenderer.SetPositions(_moveLinePoints.ToArray());
        }

        private void SetLineCollider()
        {
            var playerPos = playerPosition.position;
            
            var vector2Points = _moveLinePoints
                .Select(point => new Vector2(point.x - playerPos.x, point.y - playerPos.y))
                .ToList();
            
            if (vector2Points.Any())
                _edgeCollider.SetPoints(vector2Points);
            else
                _edgeCollider.Reset();
        }
    }
}