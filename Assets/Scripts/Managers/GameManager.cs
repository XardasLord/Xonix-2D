using System.Collections;
using System.Linq;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private IntEvent levelChangedEvent;

        [SerializeField] private TextMeshProUGUI nextLevelText;
        
        private static GameManager _instance;
        
        private static int _levelNumber = 0;
        private static int _nextLevelPercentageValue = 60;

        private const int MaxNextLevelPercentageValue = 90;

        private void Awake()
        {
            if (_instance == null) {
                // First run, set the instance
                _instance = this;
                DontDestroyOnLoad(gameObject);
 
            } else if (_instance != this) {
                // Instance is not the same as the one we have, destroy old one, and reset to newest one
                Destroy(_instance.gameObject);
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            
            _levelNumber++;
            levelChangedEvent.Raise(_levelNumber);

            _nextLevelPercentageValue += 2;
            if (_nextLevelPercentageValue > MaxNextLevelPercentageValue)
                _nextLevelPercentageValue = MaxNextLevelPercentageValue;
            
            nextLevelText ??= FindObjectsOfType<TextMeshProUGUI>().Single(x => x.name == "NextLevelText");
            nextLevelText.text = string.Empty;
        }

        public void ScoreChanged(int filledPercentage)
        {
            if (filledPercentage > _nextLevelPercentageValue)
            {
                nextLevelText.text = "Prepare for the next level...";
                StartCoroutine(LoadLevelAfterDelay(2f));
            }
        }
 
        private IEnumerator LoadLevelAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
