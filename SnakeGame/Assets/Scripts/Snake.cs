using System;
using UnityEngine;
using System.Collections.Generic;

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

    private void Start()
    {
        moveDelay = 1f / _speed;
        ResetState();
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        // Only accept input for movement if the game is not paused
        if (GameBehaviour.Instance.State == Utilities.GamePlayState.Play)
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
    }

    private void FixedUpdate()
    {
        // Only move if the game is not paused
        if (GameBehaviour.Instance.State == Utilities.GamePlayState.Play)
        {
            moveTimer += Time.fixedDeltaTime;
            if (moveTimer >= moveDelay)
            {
                moveTimer = 0f;

                // Move the snake body
                for (int i = _segments.Count - 1; i > 0; i--)
                {
                    _segments[i].position = _segments[i - 1].position;
                }

                transform.position = new Vector3(
                    Mathf.Round(transform.position.x + _direction.x),
                    Mathf.Round(transform.position.y + _direction.y),
                    0.0f
                );

                // Check if the snake has hit the wall
                CheckWallCollision();
            }
        }
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
        if (other.CompareTag("Food"))
        {
            Grow();
            ScoreManager.instance.AddPoint();
            
            //eat sound
            if (_foodSound != null && audioSource != null)
                audioSource.PlayOneShot(_foodSound);
        }
        else if (other.CompareTag("Obstacle"))
        {
            ResetState();
        }
    }

    private void CheckWallCollision()
    {
        // Check if the snake has collided with the wall
        if (!gridArea.bounds.Contains(transform.position))
        {  
            //play collision is new
            PlayCollisionSound();
            
            ResetState();
        }
    }
    
    private void PlayCollisionSound()
    {
        if (_collisionSound != null && audioSource != null)
            audioSource.PlayOneShot(_collisionSound);
    }
}
