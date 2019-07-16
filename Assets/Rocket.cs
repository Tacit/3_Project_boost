using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	[SerializeField] float rscThrust = 100f;
	[SerializeField] float thrust = 100f;

	Rigidbody rigidBody;
	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	
    private void OnCollisionEnter(Collision other) 
    {
        switch(other.gameObject.tag)
        {
            case "Friendly":
                print("ok");
                break;          
            case "Finish":
                print("Finish");   
                break;   
            default:
                print("Die");
                break;
        }
    }


	// Update is called once per frame
	void Update ()
    {
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
