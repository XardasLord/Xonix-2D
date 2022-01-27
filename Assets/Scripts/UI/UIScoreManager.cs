using Common.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UI
{
    public class UIScoreManager : MonoBehaviour
    {
        [SerializeField] private Tilemap openGameAreaTileMap;
        [SerializeField] private TextMeshProUGUI percentageValue;
        
        private int _numberOfOpenTilesAtStart;

        private void Start()
        {
            _numberOfOpenTilesAtStart = openGameAreaTileMap.CountActiveTiles();
            percentageValue.text = string.Empty;
        }

        public void SetScore()
        {
            var value = CalculatePercentageOfClosedTiles();
            percentageValue.text = $"{value}%";
        }
        
        private int CalculatePercentageOfClosedTiles()
        {
            var currentOpenTilesCount = openGameAreaTileMap.CountActiveTiles();

            return 100 - currentOpenTilesCount * 100 / _numberOfOpenTilesAtStart;
        }
    }
}
