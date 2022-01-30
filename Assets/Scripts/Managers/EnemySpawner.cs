using UnityEngine;
using UnityEngine.Tilemaps;

namespace Managers
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Tilemap gameAreaTilemap;

        public void SpawnEnemies(int level)
        {
            for (var i = 0; i < level; i++)
            {
                var position = GetRandomStartPosition();

                Instantiate(enemyPrefab, position, Quaternion.identity);
            }
        }
        
        private Vector3 GetRandomStartPosition()
        {
            var gameAreaBoundaries = gameAreaTilemap.cellBounds;
        
            var randX = Random.Range(gameAreaBoundaries.xMin, gameAreaBoundaries.xMax);
            var randY = Random.Range(gameAreaBoundaries.yMin, gameAreaBoundaries.yMax);

            return new Vector2(randX, randY);
        }
    }
}