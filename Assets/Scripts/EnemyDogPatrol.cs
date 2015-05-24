using UnityEngine;
using System.Collections;

public class EnemyDogPatrol : MonoBehaviour {
	private Animator animator;

	public float enemyScaling = 1;

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

	public Player_Script player;
	public float playerRange;
	public float moveSpeedIncrease;
	public float normalMoveSpeed;
	public Rigidbody2D enemyRigidbody;

	public bool notInRange;
	public float edgeDelay;
	public float edgeCount;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
		enemyRigidbody = this.GetComponent<Rigidbody2D> ();
		stunned = false;
		player = FindObjectOfType<Player_Script> ();
		notInRange = false;
	}
	// Update is called once per frame
	void Update () {
		if (stunnedFor > 0) {
			stunnedFor -= Time.deltaTime;
			return;
		} 
		else {
			stunned = false;
		}
		//(!hittingWall && notAtEdge &&  GetComponent<Rigidbody2D>().velocity.x == 0 && grounded) ||
		hittingWall = Physics2D.OverlapCircle (wallCheck.position, wallCheckRadius, WhatIsWall);
		notAtEdge = Physics2D.OverlapCircle (edgeCheck.position, wallCheckRadius, WhatIsWall);
		if (!notAtEdge) {
			edgeCount = edgeDelay;
		}
		if (!(player.transform.localPosition.y + player.transform.parent.gameObject.transform.localPosition.y + (player.transform.GetComponent<Renderer> ().bounds.size.y / 2) < transform.localPosition.y - (transform.GetComponent<Renderer> ().bounds.size.y / 2) ||
		    player.transform.localPosition.y + player.transform.parent.gameObject.transform.localPosition.y - (player.transform.GetComponent<Renderer> ().bounds.size.y / 2) > transform.localPosition.y + (transform.GetComponent<Renderer> ().bounds.size.y / 2) ||
		    player.transform.localPosition.x + player.transform.parent.gameObject.transform.localPosition.x < transform.localPosition.x - playerRange ||
		    player.transform.localPosition.x + player.transform.parent.gameObject.transform.localPosition.x > transform.localPosition.x + playerRange || player.gameObject.layer == 23) && edgeCount < 0){
			notInRange = false;
		}
		else{
			notInRange = true;
		}
		if (notInRange) {
			if (hittingWall || !notAtEdge) {
				Debug.Log ("Swapping");
				moveRight = !moveRight;
			}
			animator.SetBool("isWalking", true);
			moveSpeed = normalMoveSpeed;
		} 
		else {
			if(transform.localScale.x > 0 && player.transform.position.x > transform.position.x || transform.localScale.x < 0 && player.transform.position.x < transform.position.x){
				moveRight = !moveRight;
			}
			animator.SetBool("isWalking", false);
			moveSpeed = moveSpeedIncrease;
		}
		Debug.DrawLine (new Vector3 (transform.position.x - playerRange, transform.position.y, transform.position.z), new Vector3 (transform.position.x + playerRange, transform.position.y, transform.position.z));
		if (moveRight) {
			//			Debug.Log ("moving right");
			//			Debug.Log ("speed of: " + gameObject.name + ": " + GetComponent<Rigidbody2D>().velocity.x);
//			animator.SetBool("isWalking", false);
			transform.localScale = new Vector3(-enemyScaling, enemyScaling, enemyScaling);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		} 
		else {
			//			Debug.Log ("moving left");
			//			Debug.Log ("speed of: " + gameObject.name + ": " + GetComponent<Rigidbody2D>().velocity.x);
			transform.localScale = new Vector3(enemyScaling, enemyScaling, enemyScaling);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}
//		Debug.Log ("bounds size: " + player.transform.GetComponent<Renderer> ().bounds.size.y);
//		if ((transform.position.y + (transform.GetComponent<Renderer> ().bounds.size.y / 2)) < (player.transform.position.y - (player.transform.GetComponent<Renderer> ().bounds.size.y / 2)) &&
//		    (player.transform.position.x + player.transform.GetComponent<Renderer> ().bounds.size.x) > (transform.position.x - transform.GetComponent<Renderer> ().bounds.size.x) &&
//		    (player.transform.position.x - player.transform.GetComponent<Renderer> ().bounds.size.x) < (transform.position.x + transform.GetComponent<Renderer> ().bounds.size.x) &&
//		    grounded && 
//		    player.gameObject.layer != 23) {
//			Debug.Log ("DOG DOES JUMP");
////			enemyRigidbody.AddForce(new Vector2(0, 0.2f));
//			animator.SetBool("isJumping", true);
//			enemyRigidbody.velocity = new Vector2 (enemyRigidbody.velocity.x, 6.5f);
//		}
		edgeCount -= Time.deltaTime;
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "TrapsForEnemys") {
			stunned = true;
			stunnedFor = howLongStunned;
			Destroy(other.gameObject);
		}
	}
}