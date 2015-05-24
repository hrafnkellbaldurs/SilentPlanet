using UnityEngine;
using System.Collections;

public class TeleportDoor : MonoBehaviour {
	
	private bool playerInZone;
	public GameObject exitDoor;
	public Player_Script player;
	
	// Use this for initialization
	void Start () {
		playerInZone = false;
		player = FindObjectOfType<Player_Script> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.W) && playerInZone) {
			//player.transform.position = currentCheckpoint.transform.position;
			player.transform.position = exitDoor.transform.position;
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "Player_Bird") {
			playerInZone = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D other){
		if (other.name == "Player_Bird") {
			playerInZone = false;;
		}
	}
}