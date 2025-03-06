using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI _gameOverText;
    public float flickerSpeed = 0.3f;
    private bool isFlickering = true;
    void Start()
    {
        if (_gameOverText != null)
        {     
            _gameOverText.enabled = true;
            StartCoroutine(FlickerText());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            SceneManager.LoadScene("SnakeGame");
        }
        
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Utilities.QuitGame();
        }

    }
    
    IEnumerator FlickerText()
    {
        while (isFlickering)
        {
            _gameOverText.alpha = 1f;
            yield return new WaitForSeconds(flickerSpeed);
            _gameOverText.alpha = 0f;
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}
