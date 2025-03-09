using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segments = new List<Transform>();

    public Transform segmentPrefab;
    public int initialSize = 4;
    public float _speed = 5f;

    private float normalSpeed;
    private float moveTimer = 0f;
    private float moveDelay;

    public BoxCollider2D gridArea;

    [SerializeField] private AudioClip _collisionSound;
    [SerializeField] private AudioClip _foodSound;

    private AudioSource audioSource;

    private bool gameStarted = false;

    // Special food variables
    public GameObject specialFood; // Reference to the special food GameObject
    public float spawnInterval = 15f;
    private float spawnTimer = 0f;

    private void Start()
    {
        normalSpeed = _speed;
        moveDelay = 1f / _speed;
        ResetState();

        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        // Initializing the special food (make sure it's deactivated initially)
        if (specialFood != null)
        {
            specialFood.SetActive(false); // Make sure special food is inactive at start
        }

        Invoke(nameof(StartGame), 0.5f);
    }

    private void StartGame()
    {
        gameStarted = true;
    }

    private void Update()
    {
        if (GameBehaviour.Instance.State != Utilities.GamePlayState.Play) return;

        HandleInput();

        moveTimer += Time.deltaTime;
        if (moveTimer >= moveDelay)
        {
            moveTimer = 0f;
            MoveSnake();
            CheckWallCollision();
        }

        // Handle spawning of special food
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnSpecialFood();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && _direction != Vector2.down)
            _direction = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow) && _direction != Vector2.up)
            _direction = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && _direction != Vector2.right)
            _direction = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow) && _direction != Vector2.left)
            _direction = Vector2.right;
    }

    private void MoveSnake()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        transform.position = new Vector3(
            Mathf.Round(transform.position.x + _direction.x),
            Mathf.Round(transform.position.y + _direction.y),
            0.0f
        );
    }

    private void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }

    private void ResetState()
    {
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        _segments.Clear();
        _segments.Add(this.transform);

        for (int i = 1; i < this.initialSize; i++)
        {
            Transform segment = Instantiate(segmentPrefab);
            _segments.Add(segment);
        }

        transform.position = Vector3.zero;
        _direction = Vector2.right;

        ScoreManager.instance.ResetScore();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameStarted) return;

        if (other.CompareTag("Food"))
        {
            Grow();
            ScoreManager.instance.AddPoint();
            audioSource?.PlayOneShot(_foodSound);
        }
        else if (other.CompareTag("Special Food")) // Check for special food
        {
            StartSpeedBoostCoroutine(); // Apply speed boost
            other.gameObject.SetActive(false); // Deactivate the special food
            ScoreManager.instance.AddPoint(); // Increase score
            audioSource?.PlayOneShot(_foodSound); // Play sound
        }
        else if (other.CompareTag("Obstacle"))
        {
            GameOver();
        }
    }

    private void CheckWallCollision()
    {
        if (!gameStarted) return;

        if (!gridArea.bounds.Contains(transform.position))
        {
            PlayCollisionSound();
            GameOver();
        }
    }

    private void PlayCollisionSound()
    {
        audioSource?.PlayOneShot(_collisionSound);
    }

    private void GameOver()
    {
        ScoreManager.instance.ResetScore();  // Ensure score is saved before scene change
        SceneManager.LoadScene("GameOver");
    }

    public void StartSpeedBoostCoroutine()
    {
        StartCoroutine(ApplySpeedBoost()); // Call the speed boost coroutine
    }

    private IEnumerator ApplySpeedBoost()
    {
        IncreaseSpeed(1.2f); 
        yield return new WaitForSeconds(4f); // Wait for the duration of the speed boost (3 seconds)
        ResetSpeed(); // Reset speed back to normal
    }

    public void IncreaseSpeed(float speedMultiplier)
    {
        _speed *= speedMultiplier;  // Increase the speed
        moveDelay = 1f / _speed;    // Adjust the move delay based on the new speed
    }

    public void ResetSpeed()
    {
        _speed = normalSpeed;  // Reset speed to the normal value
        moveDelay = 1f / _speed;  // Reset the move delay
    }

    // Function to spawn the special food
    private void SpawnSpecialFood()
    {
        if (specialFood != null && !specialFood.activeInHierarchy) 
        {
            // Set special food to a random position within grid boundaries
            Vector3 spawnPosition = new Vector3(
                Mathf.Round(Random.Range(-10, 10)), // X position (adjust based on your grid)
                Mathf.Round(Random.Range(-5, 5)), // Y position (adjust based on your grid)
                0f
            );

            specialFood.transform.position = spawnPosition; // Set the position
            specialFood.SetActive(true); // Activate the special food
        }
    }
}
