using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {

	public float speed;
	public Player_Script player;
	public int damageToGive;
	private Rigidbody2D myRigidBody2D;
	public HealthBarSwapper healthBarSwapper;
	
	public bool shot;
	public Vector3 oldPlayer;
	public float multiplier;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player_Script> ();
		myRigidBody2D = GetComponent<Rigidbody2D> ();
		healthBarSwapper = FindObjectOfType<HealthBarSwapper> ();
		oldPlayer = player.transform.position - this.transform.position;
		oldPlayer.Normalize ();
		oldPlayer = oldPlayer * multiplier;
		oldPlayer = oldPlayer + this.transform.position;
		shot = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (shot == true) {
			this.transform.position = Vector2.MoveTowards(this.transform.position, oldPlayer, speed);
		}
		if (this.transform.position == oldPlayer) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "Player_Bird") {
			Debug.Log ("dafuq");
			healthBarSwapper.HurtPlayer (damageToGive);
			Destroy(gameObject);
		} 
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.transform.name != "Player_Bird") {
			Destroy(gameObject);
		} 
	} 
}
