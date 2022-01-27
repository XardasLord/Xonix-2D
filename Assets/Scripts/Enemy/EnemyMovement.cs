using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private Tilemap gameAreaTilemap;
        [Range(1f, 10f)] [SerializeField] private float moveSpeed = 5f;
        
        private const float HelperForceMultiplier = 100f;
        
        private Rigidbody2D _rigidBody;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            SetStartPosition();
            AddRandomMoveForce();
        }

        private void SetStartPosition()
        {
            var gameAreaBoundaries = gameAreaTilemap.cellBounds;
        
            var randX = Random.Range(gameAreaBoundaries.xMin, gameAreaBoundaries.xMax);
            var randY = Random.Range(gameAreaBoundaries.yMin, gameAreaBoundaries.yMax);

            transform.position = new Vector2(randX, randY);
        }

        private void AddRandomMoveForce()
        {
            var randX = Random.Range(0.5f, 0.9f);
            var randY = Random.Range(0.5f, 0.9f);
            
            _rigidBody.AddForce(new Vector2(randX, randY) * moveSpeed * HelperForceMultiplier);
        }
    }
}