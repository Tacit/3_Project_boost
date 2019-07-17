using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour {

	[SerializeField] Vector3 movementVector;
	[Range(0,1)]
	[SerializeField]
	float movementFactor;
	[SerializeField]
	float period = 2f;
	private Vector3 startPosition;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(period <= Mathf.Epsilon)
		{
			return;
		}
		
		float cycles = Time.time / period;
		const float tau = Mathf.PI * 2;
		float rawSin = Mathf.Sin(cycles * tau);
		movementFactor = rawSin / 2f + 0.5f;

		var offset = movementFactor * movementVector;
		transform.position = startPosition + offset;
	}
}
