using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class Rocket_Input : MonoBehaviour {

	public float ThrustForce = 20f;
	public float RotationSpeed = 5f;
	public bool isThrusting = false;

	private Rigidbody rBody;
	private AudioSource aSource;
	private float startVolume;


	// Use this for initialization
	void Start () {
		rBody = GetComponent<Rigidbody>();
		aSource = GetComponent<AudioSource>();
		startVolume = aSource.volume;
	}
	
	// Update is called once per frame
	void Update () {
		Thrust();
		RotateShip();
	}

	void RotateShip(){

		rBody.freezeRotation = true; // take manual control of rotation

		if(Input.GetKey(KeyCode.A)){
			 
			transform.Rotate(new Vector3 (0,0,RotationSpeed * Time.deltaTime));
			//transform.Rotate(Vector3.forward);

		} else if (Input.GetKey(KeyCode.D)){

			transform.Rotate(new Vector3 (0,0,-RotationSpeed * Time.deltaTime));
			//transform.Rotate(-Vector3.forward);

		}

		rBody.freezeRotation = false; // resume physics control of rotation
	}

	void Thrust(){
		
		if (Input.GetKey(KeyCode.Space)) {
			
			rBody.AddRelativeForce(new Vector3(0, ThrustForce, 0));
			//rBody.AddRelativeForce(Vector3.up);
			isThrusting = true;
			if(!aSource.isPlaying){
				aSource.volume = startVolume;
				aSource.Play();
			}
		}

		else {
			//TODO: use a coroutine or invoke to fade the sound out when thrust stops
			isThrusting = false;
			if(aSource.isPlaying){
				aSource.Stop();
			}
		}
	}
		
}
