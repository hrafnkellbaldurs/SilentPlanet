using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	private bool playerInZone;
	public string levelToLoad;

	// Use this for initialization
	void Start () {
		playerInZone = false;
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetKeyDown (KeyCode.W) || Input.GetAxisRaw ("Vertical") > 0.9f) && playerInZone) {
			Application.LoadLevel(levelToLoad);
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "Player_Bird") {
			playerInZone = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.name == "Player_Bird") {
			playerInZone = false;
		}
	}
}
