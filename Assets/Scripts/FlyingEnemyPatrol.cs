using UnityEngine;
using System.Collections;

public class FlyingEnemyPatrol : MonoBehaviour {

	public float moveSpeed;
	public bool moveRight;

	public Transform wallCheck;
	public float wallCheckRadius;
	public LayerMask WhatIsWall;
	public bool hittingWall;
	
	public bool stunned;

	public float howLongStunned;
	public float stunnedFor;

	public float turnAroundTime;
	public float countdown;

	public float howMuchScale;

	public Animator enemyAnimator;

	public int number;


	// Use this for initialization
	void Start () {
		stunned = false;
		countdown = turnAroundTime;
		enemyAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
//		if (moveRight && !stunned) {//FlyingRight
//			enemyAnimator.SetInteger ("State", 0);
//			number = 0;
//		} 
//		if (stunned) {//IdleRight
//			enemyAnimator.SetInteger ("State", 1);
//			number = 1;
//		}
//		else if (!moveRight && !stunned) {//FlyingLeft
//			enemyAnimator.SetInteger ("State", 2);
//			number = 2;
//		}
//		else {//IdleLeft
//			enemyAnimator.SetInteger ("State", 3);
//			number = 3;
//		}
		
		if (stunnedFor >= 0) {
			stunnedFor -= Time.deltaTime;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0f, 0f);
			return;
		} 
		else {
			stunned = false;
			enemyAnimator.SetInteger ("State", 0);
		}
		hittingWall = Physics2D.OverlapCircle (wallCheck.position, wallCheckRadius, WhatIsWall);
		if (countdown <= 0 || hittingWall) {
			moveRight = !moveRight;
			countdown = turnAroundTime;
		}
		if (moveRight) {
			//			Debug.Log ("moving right");
			//			Debug.Log ("speed of: " + gameObject.name + ": " + GetComponent<Rigidbody2D>().velocity.x);
//			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			transform.localScale = new Vector3(-howMuchScale, howMuchScale, howMuchScale);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		} 
		else {
			//			Debug.Log ("moving left");
			//			Debug.Log ("speed of: " + gameObject.name + ": " + GetComponent<Rigidbody2D>().velocity.x);
//			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
			transform.localScale = new Vector3(howMuchScale, howMuchScale, howMuchScale);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}
		countdown -= Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "TrapsForEnemys") {
			stunned = true;
			stunnedFor = howLongStunned;
			enemyAnimator.SetInteger ("State", 1);
			number = 1;
		}
	}
}
