using Enemy;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private VoidEvent playerDeadEvent;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.GetComponent<EnemyMovement>() is null)
                return;

            playerDeadEvent.Raise();
            gameObject.SetActive(false);
        }
    }
}
