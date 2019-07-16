using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	[SerializeField] float rscThrust = 100f;
	[SerializeField] float thrust = 100f;
    [SerializeField] int currentLevel = 0;
    [SerializeField] int levelsCount = 3;

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
                print("ok");
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel",1f);
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstLevel",1f);
                break;
        }
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

		Thrust();
        Rotate();
    }

    private void Thrust()
    {
		var thrustSpeed = Time.deltaTime * thrust;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Play();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustSpeed);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Stop();
        }
    }
    private void Rotate()
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
