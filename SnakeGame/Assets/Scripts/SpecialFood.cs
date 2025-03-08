using UnityEngine;

public class SpecialFood : MonoBehaviour
{
    public float speedBoostDuration = 5f;  // Duration of the speed boost
    private Snake snake;  // Reference to the Snake script

    private void Start()
    {
        snake = GameObject.Find("Snake").GetComponent<Snake>();  // Get the Snake component
        if (snake == null)
        {
            Debug.LogError("Snake component not found!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Apply the speed boost to the snake
            snake.ApplySpeedBoost(2f, speedBoostDuration);  // 2x speed boost for the specified duration
            Destroy(gameObject);  // Destroy special food after it is eaten
        }
    }
}