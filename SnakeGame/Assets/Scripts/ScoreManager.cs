using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    
    public TextMeshProUGUI scoreText;

    private int _score = 0;
    
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        scoreText.text = " Score: " + _score.ToString();
    }

    public void AddPoint()
    {
        _score += 5;
        scoreText.text = " Score: " + _score.ToString();
    }
    
    public void ResetScore()
    {
        _score = 0;
        scoreText.text = " Score: " + _score.ToString();
    }
}
