using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBehaviour : MonoBehaviour
{
    public Utilities.GamePlayState State;
    public static GameBehaviour Instance;
    
    public TextMeshProUGUI _pauseMessage;
    //public TextMeshProUGUI _homeScreenMessage;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        State = Utilities.GamePlayState.Play;
            _pauseMessage.enabled = false;
        //_homeScreenMessage.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) 
        {    
            State = State == Utilities.GamePlayState.Play
                ? Utilities.GamePlayState.Pause
                : Utilities.GamePlayState.Play;
            if (_pauseMessage != null)
            {
                _pauseMessage.enabled = State == Utilities.GamePlayState.Pause;
            }
        }
    }
}
