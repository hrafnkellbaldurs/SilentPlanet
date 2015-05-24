using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

	public LevelManager levelManager;
	public bool activated;

	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager> ();
		activated = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "Player_Bird" && !activated) {
			levelManager.currentCheckpoint = gameObject;
			activated = true;
//			Debug.Log("Activated Checkpoint! " + transform.position);
		}
	}
}
