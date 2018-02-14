using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Required unity components, missing components will be added to the gameobject when this scipt is attached.
using UnityEngine.Timeline;


[RequireComponent (typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]

public class Rocket_Input : MonoBehaviour {
	[Header("Physics Forces")]
	[SerializeField] float mainThrust = 50f;	// The amount of thrust applied to the ship.
	[SerializeField] float rcsThrust = 200f;	// The amount of force applied to the rotations.
	public bool isThrusting = false;

	[Header("Destroy Settings")]
	//[SerializeField] float breakupForce = 20f;
	//[SerializeField] float breakupForceRadius = 5f;
	[SerializeField] GameObject explosionVFX;
	[SerializeField] AudioClip explosionSFX;
	[SerializeField] float explosionVolume = 1f;

	private Rigidbody rBody;
	private AudioSource aSource;
	private float startVolume;


	// Use this for initialization
	void Start () {
		rBody = GetComponent<Rigidbody>();	// Reference to ships RigidBody.
		aSource = GetComponent<AudioSource>(); // Reference to the ships AudioSource.
		startVolume = aSource.volume;	// added to allow fading thruster sound
	}
	
	// Update is called once per frame
	void Update () {
		Thrust();
		RotateShip();
	}

	#region Ship Input
	void RotateShip(){

		rBody.freezeRotation = true; // Take manual control of rotation.
		float rotationThisFrame = rcsThrust * Time.deltaTime; // Calculates the frame independant rotations.

		if(Input.GetKey(KeyCode.A)){
			transform.Rotate(Vector3.forward * rotationThisFrame); // Rotate the object counter clockwise.
		} else if (Input.GetKey(KeyCode.D)){
			transform.Rotate(-Vector3.forward * rotationThisFrame); // Rotate the object clockwise.
		}

		rBody.freezeRotation = false; // Resume physics control of rotation.
	}

	void Thrust(){

		if (Input.GetKey(KeyCode.Space)) {
			rBody.AddRelativeForce(Vector3.up * mainThrust); // Moves the rocket on the y axis relative to its local position.
			isThrusting = true;
			if(!aSource.isPlaying){
				aSource.volume = startVolume; // added to allow fading thruster sound
				aSource.Play();
			}
		}

		else {
			//TODO: Try to use a coroutine or invoke to fade the thruster sound when thrust stops.
			isThrusting = false;
			if(aSource.isPlaying){
				aSource.Stop();
			}
		}
	}
	#endregion	

	#region Collision Control
	void OnCollisionEnter(Collision collision){
		
		switch (collision.gameObject.tag){
			case "Friendly":
				print("Friendly");
				//TODO: do nothing remove later
				break;
			default:
				DestroyPlayer();
				break;
		}
	}

	void OnTriggerEnter(Collider collider){
		// TODO: These can be broken up and added to a pickup script
		// and added to the item's prefab instead of in the input script.
		switch (collider.gameObject.tag){
			case "PowerUp":
				print("Powerup Collected");
				// TODO: increase shield
				break;
			case "Fuel":
				print("Fuel Added");
				//TODO: increase fuel
				break;
			case "Coin":
				print("Coin Collected");
				//TODO: increase coin total
				break;
			default:
				print("No Tag Set on gameObject");
				break;
		}

		Destroy(collider.gameObject);
	}
	#endregion

	#region Destroy Player
	void DestroyPlayer(){
		print("DestroyPlayer Invoked");
		// TODO: Make ship break apart
		Vector3 currentPos = transform.position;
		// Play explosion vfx
		if(explosionVFX != null){
			Instantiate(explosionVFX, currentPos, Quaternion.identity);
		}
		// Play Explosion sound
		if(explosionSFX != null){
			aSource.clip = explosionSFX;
			aSource.volume = explosionVolume;
			aSource.Play();
		}
		// Destroy player
		Destroy(gameObject);
	}
	#endregion

}
