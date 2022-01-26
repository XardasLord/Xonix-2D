using UnityEngine.Tilemaps;

namespace Common.Extensions
{
    public static class TilemapExtensions
    {
        public static int CountActiveTiles(this Tilemap tilemap)
        {
            var count = 0;

            foreach (var tilePosition in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(tilePosition)) 
                    count++;
            }

            return count;
        }
    }
}