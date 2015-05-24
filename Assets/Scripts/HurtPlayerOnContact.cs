using UnityEngine;
using System.Collections;

public class HurtPlayerOnContact : MonoBehaviour {

	public int damageToGive;
	public HealthBarSwapper healthBarSwapper;

	// Use this for initialization
	void Start () {
		healthBarSwapper = FindObjectOfType<HealthBarSwapper> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log ("trigger enter");
		if (other.name == "Player_Bird" && healthBarSwapper.playerHealth > 0) {
			healthBarSwapper.HurtPlayer(damageToGive);
//			other.GetComponent<AudioSource>().Play();
			var player = other.GetComponent<Player_Script>();
			player.knockBackCount = player.knockBackLength;
			if(other.transform.position.x < transform.position.x){
				player.knockPlayer(false);
			}
			else{
				player.knockPlayer(true);
			}
		}
	}
}
