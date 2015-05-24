using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public GameObject currentCheckpoint;
	private Player_Script player;

	public GameObject deathParticle;
	public GameObject respawnParticle;
	
	public float respawnDelay;
	public Camera_Script mainCamera;
	//private PlayerAnimation playerStatus;
    //public GameObject mainCamera;

//	public HealthManager healthManager;
	public HealthBarSwapper healthBarSwapper;

	public AudioSource pickup;
	public AudioSource grappleShoot;
	public AudioSource grappleHitVine;
	public AudioSource playerRunning;


	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player_Script> ();
		mainCamera = GameObject.Find ("Main Camera").GetComponent<Camera_Script> ();
		healthBarSwapper = FindObjectOfType<HealthBarSwapper> ();
		//playerStatus = GameObject.Find ("PlayerAnimation").GetComponent<PlayerAnimation> ();

		grappleShoot = GameObject.Find ("GrapplingHookShoot").GetComponent<AudioSource> ();
		grappleHitVine = GameObject.Find ("GrapplingHookHitVine").GetComponent<AudioSource> ();
		playerRunning = GameObject.Find ("PlayerRunning").GetComponent<AudioSource> ();
		playerRunning.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if ((player.move > 0f || player.move < 0f) && !playerRunning.isPlaying && player.grounded) {
			Debug.Log ("play");
			//playerRunning.Play();
			playerRunning.UnPause();
		} else if(player.move == 0f && playerRunning.isPlaying || !player.grounded || player.isDead) {
			Debug.Log ("stop");
			//playerRunning.Stop();
			playerRunning.Pause();
		}
	}

	public void RespawnPlayer(){
		StartCoroutine ("RespawnPlayerCo");
	}

	public IEnumerator RespawnPlayerCo(){
		player.letGo ();
		Instantiate (deathParticle, player.transform.position, player.transform.rotation);
		player.enabled = false;
		player.GetComponent<Renderer> ().enabled = false;
		mainCamera.isFollowing = false;
//		Debug.Log ("Player respawn here!");
		yield return new WaitForSeconds (respawnDelay);
		player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0f, 0f);
		player.transform.position = currentCheckpoint.transform.position;
		player.enabled = true;
		player.GetComponent<Renderer> ().enabled = true;
		healthBarSwapper.FullHealth ();
		healthBarSwapper.isDead = false;
		mainCamera.isFollowing = true;
		player.isDead = false;
		Instantiate (respawnParticle, currentCheckpoint.transform.position, currentCheckpoint.transform.rotation);
	}

	public void playPickupSound(){
		pickup.Play ();
	}

	public void playGrappleShoot(){
		grappleShoot.Play ();
	}

	public void playGrappleHitVine(){
		grappleHitVine.Play ();
	}


}
