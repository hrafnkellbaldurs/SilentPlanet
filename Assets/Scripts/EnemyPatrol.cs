using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour {
	public float xScaleFactor;
	public float yScaleFactor;
	public float moveSpeed;
	public bool moveRight;

	public Transform wallCheck;
	public float wallCheckRadius;
	public LayerMask WhatIsWall;
	public bool hittingWall;

	public bool notAtEdge;
	public Transform edgeCheck;

	public bool stunned;

	public float howLongStunned;
	private float stunnedFor;

	// Use this for initialization
	void Start () {
		stunned = false;
	}
	// Update is called once per frame

	//
	void Update () {
		if (stunnedFor > 0) {
			stunnedFor -= Time.deltaTime;
			return;
		} 
		else {
			stunned = false;
		}
		hittingWall = Physics2D.OverlapCircle (wallCheck.position, wallCheckRadius, WhatIsWall);
		notAtEdge = Physics2D.OverlapCircle (edgeCheck.position, wallCheckRadius, WhatIsWall);

		if (hittingWall || !notAtEdge || (!hittingWall && notAtEdge &&  GetComponent<Rigidbody2D>().velocity.x == 0)) {
			moveRight = !moveRight;
		}
//		if (!hittingWall && notAtEdge &&  GetComponent<Rigidbody2D>().velocity.x == 0) {
//			moveRight = !moveRight;
//		}

		if (moveRight) {
//			Debug.Log ("moving right");
//			Debug.Log ("speed of: " + gameObject.name + ": " + GetComponent<Rigidbody2D>().velocity.x);
			transform.localScale = new Vector3(-xScaleFactor, yScaleFactor, 1f);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		} 
		else {
//			Debug.Log ("moving left");
//			Debug.Log ("speed of: " + gameObject.name + ": " + GetComponent<Rigidbody2D>().velocity.x);
			transform.localScale = new Vector3(xScaleFactor, yScaleFactor, 1f);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "TrapsForEnemys") {
			stunned = true;
			stunnedFor = howLongStunned;
			Destroy(other.gameObject);
		}
	}
}
