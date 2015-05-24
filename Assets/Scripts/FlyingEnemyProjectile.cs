using UnityEngine;
using System.Collections;

public class FlyingEnemyProjectile : MonoBehaviour {
	
	public float speed;
	public Player_Script player;
	public int damageToGive;
	public HealthBarSwapper healthBarSwapper;
	
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player_Script> ();
		healthBarSwapper = FindObjectOfType<HealthBarSwapper> ();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnTriggerEnter2D(Collider2D other){	
		if (other.name == "Player_Bird") {
			healthBarSwapper.HurtPlayer(damageToGive);
		}
	}
	
	void OnCollisionEnter2D(){
		Destroy (gameObject);
	}
}
