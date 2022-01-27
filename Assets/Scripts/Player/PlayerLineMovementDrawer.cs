using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Player
{
    [RequireComponent(typeof(LineRenderer))]
    public class PlayerLineMovementDrawer : MonoBehaviour
    {
        [SerializeField] private Transform playerPosition;
        [SerializeField] private LayerMask layerToDrawLine;
        
        [Header("Tiles")]
        [SerializeField] private Tilemap openGameAreaTileMap;
        [SerializeField] private Tilemap closeGameAreaTileMap;
        [SerializeField] private Tile closeTile;
        
        private LineRenderer _lineRenderer;
        private List<Vector3> _moveLinePoints;
        private bool _isDrawing;
        private int _numberOfOpenTilesAtStart;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _moveLinePoints = new List<Vector3>();
            _numberOfOpenTilesAtStart = openGameAreaTileMap.CountActiveTiles();
            Debug.Log(_numberOfOpenTilesAtStart);
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
                
                _isDrawing = true;
            }
            else if (_isDrawing)
            {
                SwapOpenTilesToCloseTiles();
                
                // TODO: We can here recalculate percentage of closed tiles, move it to separate dedicated class for UI
                Debug.Log(CalculatePercentageOfClosedTiles() + "%");

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
                    smallestArea = openTileArea;
            }

            openGameAreaTileMap.FloodFill(smallestArea.First(), null);
            closeGameAreaTileMap.FloodFill(smallestArea.First(), closeTile);
            
            openGameAreaTileMap.CompressBounds();
        }

        private void AssignDrawingPoints()
        {
            _lineRenderer.positionCount = _moveLinePoints.Count;
            _lineRenderer.SetPositions(_moveLinePoints.ToArray());
        }

        private int CalculatePercentageOfClosedTiles()
        {
            var currentOpenTilesCount = openGameAreaTileMap.CountActiveTiles();

            return 100 - currentOpenTilesCount * 100 / _numberOfOpenTilesAtStart;
        }
    }
}