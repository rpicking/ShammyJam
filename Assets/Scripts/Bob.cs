﻿using UnityEngine;
using System.Collections;

public class Bob : MonoBehaviour {

	public string Horizontal = "L_XAxis_2";
	public string Vertical = "L_YAxis_2";
	public string B_button = "B_2";
	public string X_button = "X_2";
	public float leap_force = 30f;
	public float run_speed = 15f;
	public float jump_time = 0.1f;
	Vector3 ramp_vec = new Vector3 (0, 0, 0);
	public bool jumping = false;
	//bool ramp = false;
	float jump_clock = 0;
	Vector3 tempjump = new Vector3(0,0,0);
	public GameObject Steve;
	bool has_key = false;
	public bool swap = false;
	Vector3 bobpos;
	Vector3 stevepos;


	// Use this for initialization
	void Start () {
		Steve = GameObject.Find ("Steve");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton (Horizontal)) {
			Debug.Log (Input.GetButton (Horizontal));
		}
		if (jumping) {
			GetComponent<Rigidbody> ().velocity = (tempjump * leap_force) + ramp_vec;
			jump_clock += Time.deltaTime;
			if (jump_clock > jump_time) {
				jumping = false;
				Vector3 pos = transform.position - new Vector3 (0, 2, 0);
				transform.position = pos;
			}

		} else if (!Steve.GetComponent<Steve>().lantern && !swap){
			GetComponent<Rigidbody>().velocity =(new Vector3 (Input.GetAxis(Horizontal) - Input.GetAxis(Vertical), 0, -(Input.GetAxis(Horizontal) + Input.GetAxis(Vertical)))* run_speed) + ramp_vec;
		}

		if (!jumping) {
			if (transform.position.y != -2.4f) {
				Vector3 temp = transform.position;
				temp.y = -2.4f;
				transform.position = temp;
			}
		}

		if (Input.GetButtonDown (B_button) && !jumping && (Input.GetAxis(Horizontal) != 0 || Input.GetAxis(Vertical) != 0)) {
			tempjump = new Vector3 (Input.GetAxis(Horizontal) - Input.GetAxis(Vertical), 0, -(Input.GetAxis(Horizontal) + Input.GetAxis(Vertical)));
			tempjump.Normalize();
			jumping = true;
			jump_clock = 0;
			Vector3 pos = transform.position + new Vector3 (0, 2, 0);
			transform.position = pos;
		}
		if (Input.GetButtonDown (X_button)&& !jumping && !Steve.GetComponent<Steve>().lantern) {
			swap = true;
			bobpos = transform.position;
			stevepos = Steve.transform.position;
			//Vector3 temppos = transform.position;
			//transform.position = Steve.transform.position;
			//Steve.transform.position = temppos;

		}
		if (swap) {
			float step = run_speed * 5 * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, stevepos, step);
			Steve.transform.position = Vector3.MoveTowards(Steve.transform.position, bobpos, step);
			if (transform.position == stevepos && Steve.transform.position == bobpos) {
				swap = false;
			}
		}

	}

	void OnTriggerEnter(Collider collision) {
		//Debug.Log ("hit");
		if (collision.gameObject.tag == "Ramp") {
			//Debug.Log ("hit player");
			//ramp_object = collision.gameObject;
			ramp_vec = collision.GetComponent<Push_block>().push * collision.GetComponent<Push_block>().push_force;
			//ramp = true;
		}
		if (collision.gameObject.tag == "Key"  && !has_key) {
			has_key = true;
			Destroy(collision.gameObject);
		}
		
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "locked_door" && has_key) {
			has_key = false;
			Destroy(collision.gameObject);
		}
		
	}

	void OnTriggerExit(Collider collision) {
		//Debug.Log ("hit");
		if (collision.gameObject.tag == "Ramp") {
			//Debug.Log ("hit player");
			ramp_vec = new Vector3 (0, 0, 0);
			//ramp = false;
		}
		
	}


}
