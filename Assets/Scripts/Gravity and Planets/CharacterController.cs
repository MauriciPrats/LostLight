﻿using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	public float speed;
	

	private bool isLookingRight = true;
	private bool isMoving = false;
	private bool isJumping = false;
	private bool isGoingUp = false;

	Vector3 moveAmount;

	void FixedUpdate(){
		Vector3 movement = transform.TransformDirection (moveAmount) * Time.fixedDeltaTime;
		this.transform.position = new Vector3(this.transform.position.x + movement.x,this.transform.position.y + movement.y,this.transform.position.z);

		if (isJumping) {
			isGoingUp = Vector3.Angle (transform.up, GetComponent<Rigidbody> ().velocity.normalized) < 90f;
		}
	}

	void Update(){
		//Check if the gravity body is touching the ground or not
		if(isJumping && GetComponent<GravityBody>().getIsTouchingPlanet() && !isGoingUp){
			isJumping = false;
		}
	}

	/*
	 * It moves the character a determinate amount where -1 is left and +1 is right
	 */
	public void Move(float amount) {
		isMoving = true;
		moveAmount = (speed * amount) * -this.transform.right;
		LookLeftOrRight (amount);
	}
	/*
	 * It makes the character change the orientation depending on the moveAmount (-1 left, 1 right)
	 */
	public void LookLeftOrRight(float amount){
		if (amount > 0f) {
			if(!isLookingRight){
				transform.Rotate(0f,180f,0f);
				isLookingRight = true;
			}
		}else if(amount<0f){
			if(isLookingRight){
				transform.Rotate(0f,180f,0f);
				isLookingRight = false;
			}
		}
	}

	public void StopMoving(){
		moveAmount = new Vector3 (0f, 0f, 0f);
	}

	public void Jump(float jumpSpeed){
		GetComponent<Rigidbody> ().velocity = transform.up * jumpSpeed;
		isJumping = true;
		isGoingUp = true;
	}

	public bool getIsLookingRight(){
		return isLookingRight;
	}

	public Vector3 getMoveAmout(){
		return moveAmount;
	}

	public void setMoveAmount(Vector3 newMoveAmount){
		moveAmount = newMoveAmount;
	}

	public void stopJumping(){
		isJumping = false;
		isGoingUp = false;
	}

	public bool getIsGoingUp(){
		return isGoingUp;
	}

	public bool getIsJumping(){
		return isJumping;
	}
}