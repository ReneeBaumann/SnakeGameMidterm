using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] private AudioClip _themesong;
    private AudioSource audioSource;
    
    void Start()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = _themesong;
        audioSource.loop = true; // Set to true too loop
        audioSource.Play();
    }
}

