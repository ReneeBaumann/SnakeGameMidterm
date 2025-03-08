using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public GameObject specialFoodPrefab; // Reference to the Special Food prefab
    public float specialFoodChance = 0.1f; // Chance to spawn special food each time food is eaten
    private int foodEatenCount = 0;

    private void Start()
    {
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;
        
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        
        transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foodEatenCount++;

            // Check if it's time to spawn special food
            if (Random.value <= specialFoodChance)  // Random chance to spawn special food
            {
                SpawnSpecialFood();
            }

            RandomizePosition(); // Respawn regular food at a new location
        }
    }

    private void SpawnSpecialFood()
    {
        if (specialFoodPrefab != null)
        {
            GameObject specialFood = Instantiate(specialFoodPrefab, transform.position, Quaternion.identity);
            specialFood.GetComponent<Food>().RandomizePosition();  // Randomize position directly
        }
        else
        {
            Debug.LogError("Special Food Prefab is not assigned!");
        }
    }
}