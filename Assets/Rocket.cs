using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	[SerializeField] float rscThrust = 100f;
	[SerializeField] float thrust = 100f;
    [SerializeField] int currentLevel = 0;
    [SerializeField] int levelsCount = 3;
    [SerializeField] AudioClip mainThrust;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip win;
	Rigidbody rigidBody;
	AudioSource audioSource;

    enum State 
    {
        Alive,
        Dying,
        Transcending
    }

    State state;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
        state = State.Alive;
	}
	
    private void OnCollisionEnter(Collision other) 
    {
        if(state != State.Alive)
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
        Invoke("LoadFirstLevel", 1f);
    }

    private void StartWinSequence()
    {
        audioSource.Stop();
        state = State.Transcending;
        audioSource.PlayOneShot(win);
        Invoke("LoadNextLevel", 1f);
    }

    private void LoadNextLevel()
    {
        currentLevel++;
        currentLevel %= levelsCount;
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
    }

    private void RespondToThrustInput()
    {
		var thrustSpeed = Time.deltaTime * thrust;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustSpeed);
            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainThrust);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
             audioSource.Stop();
        }
    }
    private void RespondToRotateInput()
    {
		rigidBody.freezeRotation = true;
        
		var rotateSpeed = Time.deltaTime * rscThrust;

		if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotateSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotateSpeed);
        }
		
		rigidBody.freezeRotation = false;
    }    
}
