using Common.Extensions;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UI
{
    public class UIScoreManager : MonoBehaviour
    {
        [SerializeField] private Tilemap openGameAreaTileMap;
        
        [Header("UI Texts")] 
        [SerializeField] private TextMeshProUGUI percentageTextValue;
        [SerializeField] private TextMeshProUGUI levelTextValue;
        
        [Header("Events")] 
        [SerializeField] private IntEvent percentageChangedEvent;
        
        private int _numberOfOpenTilesAtStart;

        private void Start()
        {
            _numberOfOpenTilesAtStart = openGameAreaTileMap.CountActiveTiles();
            percentageTextValue.text = "0%";
        }

        public void SetScore()
        {
            var value = CalculatePercentageOfClosedTiles();
            percentageTextValue.text = $"{value}%";

            percentageChangedEvent.Raise(value);
        }

        public void SetLevel(int level)
        {
            levelTextValue.text = level.ToString();
        }
        
        private int CalculatePercentageOfClosedTiles()
        {
            var currentOpenTilesCount = openGameAreaTileMap.CountActiveTiles();

            return 100 - currentOpenTilesCount * 100 / _numberOfOpenTilesAtStart;
        }
    }
}
