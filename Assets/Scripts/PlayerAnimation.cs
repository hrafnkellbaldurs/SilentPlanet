using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	// Variables
	public Animator anim; // Refrerence to the animator
	//private float fallSpeed; // The speed the character falls
	//private float verticalMovement; // The amount of vertical movement
	//private bool onGround; // Flag to check whether the character is on the ground
	private Player_Script player;
	public bool turningRight;
	public bool isDead;
	public bool running;
	public bool wallHugging;
	public bool jumping;
	public bool inAir;
	public bool grappledHanging;
	public bool walking;
	public bool wallJump;
	public bool idle;
	public float rotationTime;
	public bool mouseOnRightSide;
	public bool gamepadRAnalogOnRightSide;
	public bool lookAtMouseGrappled = true;
	public float jumpTime;
	private float jumpTimeActual;
	public float wallJumpTime;
	private float wallJumpTimeActual;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player_Bird").GetComponent<Player_Script> ();
	}
	
	// Update is called once per frame
	void Update () {

		//tells if the mouse is on the right of the player
		mouseOnRightSide = (GameObject.Find ("Player_Bird").transform.position.x < Camera.main.ScreenToWorldPoint (Input.mousePosition).x);
		gamepadRAnalogOnRightSide = (player.GPAD_RANALOG_VALUE_X > 0f);

		//Debug.Log (gamepadRAnalogOnRightSide? "right side" : "left side");

		
		wallHugging = (player.huggingLeftWall || player.huggingRightWall);

		wallJump = player.wallJump;

		jumping = (!player.grounded && !player.isGrappled && !wallJump);

		if (jumpTimeActual <= 0) {
			jumping = false;
			if(player.grounded){
				jumpTimeActual = jumpTime;
			}
		}

		if (wallJumpTimeActual <= 0) {
			wallJump = false;
			if(wallHugging || player.grounded){
				wallJumpTimeActual = wallJumpTime;
			}

		}

		inAir = !player.grounded;
		grappledHanging = (player.isGrappled && (Mathf.Abs(player.move) == 0f));

		if (player.move != 0) {
			if(Mathf.Abs(player.move) > 0.5){
				running = true;
				walking = false;
			}
			else{
				running = false;
				walking = true;
			}
			//if player is moving left
			if (player.move < 0.0f && player.move >= -1.0f) {
				turningRight =  false;
			} 
			else {
				turningRight = true;
			}
		} else {
			//if player is still
			running = false; 
			walking = false;
		}

		isDead = player.isDead;

		if (turningRight) {
			if(player.huggingRightWall && !player.grounded){
				//make player look away from wall if he's in the air and hugging wall
				transform.rotation = Quaternion.AngleAxis (180f, Vector3.up);
				turningRight = false;
			}
			else if(player.isGrappled){

				if(lookAtMouseGrappled){
					//makes player look at direction of mouse when grappled
					if(mouseOnRightSide || gamepadRAnalogOnRightSide){ 
						transform.rotation = Quaternion.AngleAxis (0f, Vector3.up);
					} else{
						transform.rotation = Quaternion.AngleAxis (180f, Vector3.up); 
						turningRight = false;
					}
				}
			}
			else{
				//turn player normally
				//Quaternion angleaxis = Quaternion.AngleAxis(0f, Vector3.up);
				//transform.rotation = Quaternion.Slerp(transform.rotation, angleaxis, rotationTime);

				transform.rotation = Quaternion.AngleAxis (0f, Vector3.up);
			}

		} else {
			if(player.huggingLeftWall && !player.grounded){
				//make player look away from wall if he's in the air and hugging wall
				transform.rotation = Quaternion.AngleAxis (0f, Vector3.up);
				turningRight = true;
			} else if(player.isGrappled){

				if(lookAtMouseGrappled){
					//makes player look at direction of mouse when grappled
					if(mouseOnRightSide || gamepadRAnalogOnRightSide){ 
						transform.rotation = Quaternion.AngleAxis (0f, Vector3.up);
						turningRight = true;
					} else{
						transform.rotation = Quaternion.AngleAxis (180f, Vector3.up); 
					}
				}
			} 
			else{
				//turn player normally
				//Quaternion angleaxis2 = Quaternion.AngleAxis(180f, Vector3.up);
				//transform.rotation = Quaternion.Slerp(transform.rotation, angleaxis2, rotationTime);

				transform.rotation = Quaternion.AngleAxis (180f, Vector3.up);
			}
				
		}

		idle = (player.move == 0f && player.grounded && !player.shootingHook);

		//if player is not touching ground and is not grappled, play jump animation
		anim.SetBool ("Jumping", jumping);
		anim.SetBool ("Dead", isDead);
		anim.SetBool ("Running", running);
		anim.SetBool ("Walking", walking);
		anim.SetBool ("Falling", player.falling);
		anim.SetBool ("Inair", inAir);
		anim.SetBool ("Walljumping", wallJump);
		anim.SetBool ("Wallsliding", (wallHugging && inAir));
		anim.SetBool ("Grappled", player.isGrappled);
		anim.SetBool ("GrappledHanging", grappledHanging);
		anim.SetBool ("ShootGrapple", player.shootingHook );
		anim.SetBool ("Idle", idle);

		if (jumping) {
			jumpTimeActual -= Time.deltaTime;
		}

		if (wallJump) {
			wallJumpTimeActual -= Time.deltaTime;
		}
	}
}
