using System.Collections.Generic;
using UnityEngine;
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
        public static IEnumerable<Vector3Int> GetActiveTiles(this Tilemap tilemap)
        {
            var activeTiles = new List<Vector3Int>();

            foreach (var tilePosition in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(tilePosition)) 
                    activeTiles.Add(tilePosition);
            }

            return activeTiles;
        }

        public static List<List<Vector3Int>> GetTileAreas(this Tilemap tilemap)
        {
            var visited = new List<Vector3Int>();
            var directions = new List<Vector3Int>{Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right};
            var groups = new List<List<Vector3Int>>();

            foreach (var tile in tilemap.cellBounds.allPositionsWithin)
            {
                var group = new List<Vector3Int>();
                var visit = new List<Vector3Int>();
                
                if (!visited.Contains(tile))
                {
                    visit.Add(tile);
                    for (var z = 0; z < visit.Count; z++)
                    {
                        var currentTile = visit[z];
                        if (!visited.Contains(currentTile) && tilemap.HasTile(currentTile))
                        {
                            visited.Add(currentTile);
                            group.Add(currentTile);
                            
                            foreach (var direction in directions)
                            {
                                if (!visited.Contains(currentTile + direction))
                                {
                                    visit.Add(currentTile + direction);
                                }
                            }
                        }
                        else if (!tilemap.HasTile(currentTile))
                        {
                            visit.Remove(currentTile);
                        }
                    }
                }
                
                if (group.Count > 0)
                {
                    groups.Add(group);
                }
            }
            
            visited.Clear();
            
            return groups;
        }
    }
}