using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource; // to cache AudioSource

    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L)) // GetKeyDown: Returns true during the frame the user starts pressing down the key identified by name.
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C)) // GetKeyDown: Returns true during the frame the user starts pressing down the key identified by name.
        {
            collisionDisabled = !collisionDisabled; // toggle collision
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisabled) // if isTransitioning is true or collisionDisabled is true return.
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            
            case "Friendly":
                Debug.Log("This thing is friendly.");
                break;
            case "Finish":
                StartSuccessSequence();               
                break;
            case "Fuel":
                Debug.Log("You picked up fuel.");
                break;            
            default:
                StartCrashSequence();                
                break;
        }
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop(); // Need to stop the sound after collision so we add that        
        audioSource.PlayOneShot(success); // makes play only one sound that assigned
        successParticles.Play(); // makes play particles
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay); // Add delay while going next level 1 second via Invoke().
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop(); // Need to stop the sound after collision so i add that        
        audioSource.PlayOneShot(crash); // makes play only one sound that assigned
        crashParticles.Play(); // makes play particles
        GetComponent<Movement>().enabled = false; // Disable the Movement.cs because i don't want to player still have the control after the collision.
        Invoke("ReloadLevel", levelLoadDelay); // And add delay while reloading level 1 second via Invoke().
    }


    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex +1; // Add +1 to go to next level.
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) // SceneManager.sceneCountInBuildSettings gets the number of the scenes.
        {
            nextSceneIndex = 0; // Go to first level which is index 0 (zero).
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
