using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour
{
    public TextMeshProUGUI _homeScreenMessage;

    void Start()
    {
        _homeScreenMessage.enabled = true;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            SceneManager.LoadScene("SnakeGame");
        }

    }
}
