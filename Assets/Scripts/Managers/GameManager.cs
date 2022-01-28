using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private IntEvent levelChangedEvent; 
        
        private static GameManager instance;
        
        private static int _levelNumber = 0;
        private static int _numberOfEnemies = 1;
        private static int _nextLevelPercentageValue = 60;
        
        private int _maxNextLevelPercentageValue = 90;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
            
            _levelNumber++;
            levelChangedEvent.Raise(_levelNumber);

            _nextLevelPercentageValue += 2;
            if (_nextLevelPercentageValue > _maxNextLevelPercentageValue)
                _nextLevelPercentageValue = _maxNextLevelPercentageValue;
            
            // TODO: Spawn enemies
            _numberOfEnemies++;
        }

        public void ScoreChanged(int filledPercentage)
        {
            if (filledPercentage > _nextLevelPercentageValue)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
