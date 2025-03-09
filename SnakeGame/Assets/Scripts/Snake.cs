using System;
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

    private float moveTimer = 0f;
    private float moveDelay;

    public BoxCollider2D gridArea;  

    [SerializeField] private AudioClip _collisionSound;
    [SerializeField] private AudioClip _foodSound;
    private AudioSource audioSource;

    private bool gameStarted = false;

    private void Start()
    {
        moveDelay = 1f / _speed;
        ResetState();

        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

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
}
