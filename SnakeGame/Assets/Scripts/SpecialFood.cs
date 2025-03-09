using System.Collections;
using UnityEngine;

public class SpecialFood : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Snake>().StartSpeedBoostCoroutine(); // Trigger the speed boost from Snake
            gameObject.SetActive(false); // Deactivate special food object after consumption
        }
    }
}