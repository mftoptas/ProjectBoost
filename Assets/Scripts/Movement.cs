using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationThrust = 100f;

    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;
    Rigidbody rb;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // getting rigidbody component
        audioSource = GetComponent<AudioSource>(); // getting audio component
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space)) //GetKey will report the status of the named key. Returns true while the user HOLDS down the key identified by name.
        {
            StartThrusting();
        }
        else // if not pushing the space
        {
            StopThrusting();
        }
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime); //rb.AddRelativeForce(Vector3.up); will do the same thing.
                                                                       //AddRelativeForce if i have a roteted object i still want to add the force relative to this particular object's coordinate system
        if (!audioSource.isPlaying) //Only play audio when i am not already playing the game.
        {
            audioSource.PlayOneShot(mainEngine); // makes mainEngine audio play
        }
        if (!mainEngineParticles.isPlaying) // if particles are not playing
        {
            mainEngineParticles.Play(); // makes play particles
        }
    }

    void StopThrusting()
    {
        audioSource.Stop(); // makes audio stop
        mainEngineParticles.Stop(); // makes stop particles
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A)) //GetKey will report the status of the named key. Returns true while the user HOLDS down the key identified by name.
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D)) //GetKey will report the status of the named key. Returns true while the user HOLDS down the key identified by name.
        {
            RotatingRight();
        }
        else
        {
            StopRotating();
        }
    }

    void RotateLeft()
    {
        ApplyRotation(rotationThrust);
        if (!rightThrusterParticles.isPlaying) // if particles are not playing
        {
            rightThrusterParticles.Play(); // makes play particles
        }
    }

     void RotatingRight()
    {
        ApplyRotation(-rotationThrust);
        if (!leftThrusterParticles.isPlaying) // if particles are not playing
        {
            leftThrusterParticles.Play(); // makes play particles
        }
    }
    
    void StopRotating()
    {
        rightThrusterParticles.Stop(); // makes stop particles
        leftThrusterParticles.Stop(); // makes stop particles
    }

    void ApplyRotation(float rotationThisFrame)
    {
        // freezeRotation Controls whether physics will change the rotation of the object.
        rb.freezeRotation = true; // freezing rotation so i can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing rotation so the physics system can take over        
    }
}
