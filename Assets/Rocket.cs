using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	const float rscThrust = 1f;
	Rigidbody rigidBody;
	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
		if(Input.GetKeyDown(KeyCode.Space))
		{
			audioSource.Play();
		}

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);
        }

		if(Input.GetKeyUp(KeyCode.Space))
		{
			audioSource.Stop();
		}

		if(Input.GetKey(KeyCode.A))
		{
			transform.Rotate(Vector3.forward );// * Time.deltaTime);
		} else if(Input.GetKey(KeyCode.D))
		{
			transform.Rotate(-Vector3.forward);//  * Time.deltaTime);
		}
    }
}
