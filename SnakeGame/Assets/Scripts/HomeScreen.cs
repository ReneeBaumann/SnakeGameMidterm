using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class HomeScreen : MonoBehaviour
{
    public TextMeshProUGUI _homeScreenMessage;
    public float flickerSpeed = 0.3f;
    private bool isFlickering = true;

    void Start()
    {
        _homeScreenMessage.enabled = true;
        StartCoroutine(FlickerText());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            SceneManager.LoadScene("SnakeGame");
        }

    }
    
    IEnumerator FlickerText()
    {
        while (isFlickering)
        {
            _homeScreenMessage.alpha = 1f;
            yield return new WaitForSeconds(flickerSpeed);
            _homeScreenMessage.alpha = 0f;
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
    
}
