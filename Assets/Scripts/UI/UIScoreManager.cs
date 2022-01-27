using Common.Extensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UI
{
    public class UIScoreManager : MonoBehaviour
    {
        [SerializeField] private Tilemap openGameAreaTileMap;
        
        private int _numberOfOpenTilesAtStart;

        private void Start()
        {
            _numberOfOpenTilesAtStart = openGameAreaTileMap.CountActiveTiles();
        }

        public void SetScore()
        {
            Debug.Log(CalculatePercentageOfClosedTiles() + "%");
        }
        
        private int CalculatePercentageOfClosedTiles()
        {
            var currentOpenTilesCount = openGameAreaTileMap.CountActiveTiles();

            return 100 - currentOpenTilesCount * 100 / _numberOfOpenTilesAtStart;
        }
    }
}
