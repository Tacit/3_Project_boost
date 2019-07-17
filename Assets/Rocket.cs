using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    float loadLevelDelay = 1f;
	[SerializeField] float rscThrust = 100f;
	[SerializeField] float thrust = 100f;
    [SerializeField] int currentLevel = 0;
    [SerializeField] AudioClip mainThrust;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip win;
    [SerializeField] ParticleSystem mainThrustParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem winParticles;
	Rigidbody rigidBody;
	AudioSource audioSource;

    enum State 
    {
        Alive,
        Dying,
        Transcending
    }

    State state;
    private bool disableCollisions;

    // Use this for initialization
    void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
        state = State.Alive;
	}
	
    private void OnCollisionEnter(Collision other) 
    {
        if(state != State.Alive || disableCollisions)
        {
            return;
        }

        switch(other.gameObject.tag)
        {
            case "Friendly":
                //Do nothing
                break;
            case "Finish":
                StartWinSequence();
                break;
            default:
                StartDieSequence();
                break;
        }
    }

    private void StartDieSequence()
    {
        audioSource.Stop();
        state = State.Dying;
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", loadLevelDelay);
    }

    private void StartWinSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        transform.rotation = Quaternion.identity;
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        audioSource.PlayOneShot(win);
        winParticles.Play();
        Invoke("LoadNextLevel", loadLevelDelay);
    }

    private void LoadNextLevel()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex + 1;
        currentLevel %= SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(currentLevel);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update ()
    {
        if(state != State.Alive)
        {
            return;
        }

		RespondToThrustInput();
        RespondToRotateInput();
        ResponseToDebugInput();
    }

    private void ResponseToDebugInput()
    {
        if(!Debug.isDebugBuild)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            disableCollisions = !disableCollisions;
        }
    }

    private void RespondToThrustInput()
    {
		var thrustSpeed = Time.deltaTime * thrust;

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust(thrustSpeed);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
             audioSource.Stop();
             mainThrustParticles.Stop();
        }
    }

    private void ApplyThrust(float thrustSpeed)
    {
        rigidBody.AddRelativeForce(Vector3.up * thrustSpeed);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainThrust);
        }
        mainThrustParticles.Play();
    }

    private void RespondToRotateInput()
    {
		var rotateSpeed = Time.deltaTime * rscThrust;

		if (Input.GetKey(KeyCode.A))
        {
            RotatePlayer(rotateSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotatePlayer(-rotateSpeed);
        }
    }

    private void RotatePlayer(float rotateSpeed)
    {
        rigidBody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotateSpeed);
        rigidBody.freezeRotation = false;
    }
}
