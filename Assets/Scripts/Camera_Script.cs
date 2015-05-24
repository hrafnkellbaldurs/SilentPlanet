using UnityEngine;
using System.Collections;

public class Camera_Script : MonoBehaviour {

	public Player_Script the_player;

	public bool isFollowing;

	public float xOffset;
	public float yOffset;
	public float lerpSpeed;
	private Vector3 from;
	private PlayerAnimation playerAnimation;


	// Use this for initialization
	void Start () {
//		the_player = FindObjectOfType<Player_Script> ();
		the_player = GameObject.Find ("Player_Bird").GetComponent<Player_Script> ();
		playerAnimation = GameObject.Find ("PlayerAnimation").GetComponent<PlayerAnimation> ();
		isFollowing = true;
		lerpSpeed = 0.1f;
	}

	void Awake (){
		//Cursor.visible = false; // turn off cursor visibility
	}

	void FixedUpdate(){
		if (isFollowing) {
			Vector3 to = new Vector3();

			//makes camera move to an offset according to player movement
			if(playerAnimation.turningRight){
				to.x = the_player.transform.position.x + xOffset;
				to.y = the_player.transform.position.y + yOffset;
				to.z = transform.position.z;
			}
			else{
				to.x = the_player.transform.position.x - xOffset;
				to.y = the_player.transform.position.y + yOffset;
				to.z = transform.position.z;
			}

			from = transform.position;
			//transform.position = Vector3.MoveTowards(oldPos, newPos, damping);
			//transform.position = Vector3.Lerp(oldPos, newPos, damping);
			//transform.position = to;
			transform.position = Vector3.Lerp (from, to, lerpSpeed);
		}
	}
}
