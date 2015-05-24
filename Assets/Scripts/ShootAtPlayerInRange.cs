using UnityEngine;
using System.Collections;

public class ShootAtPlayerInRange : MonoBehaviour {
	public GameObject enemyProjectile;
	public Player_Script player;

	public float waitBetweenShots;
	private float shotCounter;

	public float stunnedFor;
	public float stunCounter;
	public bool stunned;

	public bool playerInRange;
	public Transform oldPlayerPosition;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player_Script> ();
		shotCounter = waitBetweenShots;
		playerInRange = false;
		stunned = false;
	}
	
	// Update is called once per frame
	void Update () {
		shotCounter -= Time.deltaTime;
		if (stunCounter > 0) {
			return;
		}
		if (playerInRange && shotCounter < 0) {
			shotCounter = waitBetweenShots;
			GameObject yolo = Instantiate(enemyProjectile, transform.position, player.transform.rotation) as GameObject;
			Debug.Log ("Launching missile");
//			yolo.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(player.transform.position.x * speed, player.transform.position.y * speed, 1));
//			currentProjectile = Instantiate(enemyProjectile);
//			currentProjectile.init(this.transform.position.x, this.transform.position.y);
		}

//		if (transform.localScale.x < 0 && player.transform.position.x > transform.position.x && player.transform.position.x < transform.position.x + playerRange && shotCounter < 0) {
//			Instantiate(enemyProjectile, launchPoint.position, launchPoint.rotation);
//			shotCounter = waitBetweenShots;
//		}
//		if (transform.localScale.x > 0 && player.transform.position.x < transform.position.x && player.transform.position.x > transform.position.x - playerRange && shotCounter < 0) {
//			Instantiate(enemyProjectile, launchPoint.position, launchPoint.rotation);
//			shotCounter = waitBetweenShots;
//		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.transform.name == "Player_Bird") {
			playerInRange = true;
			Debug.Log ("In range");
		}
		if(other.transform.tag == "TrapsForEnemys"){
			stunCounter = stunnedFor;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.transform.name == "Player_Bird") {
			shotCounter = waitBetweenShots;
			playerInRange = false;
		}
	}

}
