using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    
    private int _score = 0;
    private int _highScore = 0;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        scoreText.text = " Score: " + _score.ToString();
        highScoreText.text = "High Score: " + _highScore.ToString();
    }

    public void AddPoint()
    {
        _score += 5;
        scoreText.text = " Score: " + _score.ToString();
    }
    
    public void ResetScore()
    {
        if (_score > _highScore)
        {
            _highScore = _score;
            PlayerPrefs.SetInt("HighScore", _score);
            PlayerPrefs.Save();
            highScoreText.text = "High Score: " + _highScore.ToString();
            
        }
        
        _score = 0;
        scoreText.text = " Score: " + _score.ToString();
    }
}
