using UnityEngine;
using System.Collections;

public class Player_Script : MonoBehaviour {

	//GAMEPAD GLOBALS BEGIN
	[HideInInspector] public bool GPAD_JUMP;
	[HideInInspector] public bool GPAD_SHOOT_HOLD;
	[HideInInspector] public bool GPAD_SHOOT_DOWN;
	[HideInInspector] public bool GPAD_SHOOT_UP;
	[HideInInspector] public bool GPAD_L1;
	[HideInInspector] public bool GPAD_LANALOG_RIGHT;
	[HideInInspector] public bool GPAD_LANALOG_LEFT;
	[HideInInspector] public bool GPAD_LANALOG_UP;
	[HideInInspector] public bool GPAD_LANALOG_DOWN;
	[HideInInspector] public bool GPAD_R2_TRIGGER;
	[HideInInspector] public bool GPAD_L2_TRIGGER;
	[HideInInspector] public bool GPAD_START;
	[HideInInspector] public float GPAD_TRIGGER_VALUE;
	[HideInInspector] public float GPAD_RANALOG_VALUE_X;
	[HideInInspector] public float GPAD_RANALOG_VALUE_Y;
	//GAMEPAD GLOBALS END
	
	
	public bool grounded;
	public bool falling;
	public bool isDead;
	public bool wallJump;
	public float velocityX;
	public float velocityY;
	public float move;
	public float moveVertical;
	public bool hasGrapplingHook;
	public GameObject grapplingHook;
	
	public Transform shooter; // Transform coordinates of the Player
	public LayerMask layersDetectedByHook; // The Layer that you can grapple too
	public bool isGrappled = false;
	public Material mat; // Material of the grappling rope
	public float grapplingRopeWidth;
	public float grapplingHookShotTime;
	public GrapplingHookController grapplingHookController;
	
	public float maxRopeLength = 8.5f; // Maximum length of the rope
	//public float moveSpeed = 3; // the movespeed of the player
	public float ropeSwingSpeed = 2.35f; // speed of player movement when swinging from the rope
	public float ropeReelSpeed = 0.05f; // the speed of which the rope reels in or slackens in length
	//public float jumpHeight = 6;
	//public float afterBeingGrappledAirMovement = 0.35f;
	//public float maxMoveSpeed = 2;
	
	//BÆTT INN, AÐFERÐ 2 FYRIR GROUNDCHECK
	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	
	
	public bool wasGrappled = false;
	
	//	public Transform startPos; // for casting rays to see if we are grounded
	//	public Transform endPos;   // -------------------""---------------------
	//	public LayerMask groundLayer; // The ground layer
	
	private Vector3 mousePos;
	
	private LineRenderer rope; // The LineRender object that will draw the rope
	private SpringJoint2D grapple; // The SpringJoint that is between the player and the grappled point
	
	private Vector3 hitPosition;
	private GameObject hitObject; // The object that is grappled to
	
	private Rigidbody2D playerRigidBody;
	
	// BÆTT AF ATLA
	private Stack ropeCollisionPoints; // Stack for keeping the transform coordinates of the collision points
	private ArrayList ropeCollPoes;
	
	private float oldRopeTotalLength;
	private Stack oldRopeSegmentLengths;
	
	public LayerMask collidableLayersForRope; //nonCollidableForRope = collidableLayersForRope
	
	private Transform distractionTransforms;
	private bool hasDistraction = false;
	private float distractionDelay;
	
	// ----------------------------------------------------------
	
	
	//BÆTT INN FYRIR KNOCKBACK/WALLJUMP !!!!
	public float knockBack;
	public float knockBackLength = 0.5f;
	public float knockBackCount;
	
	//BÆTT INN FYRIR WALLJUMP!!!
	//public Transform wallCheck;
	//public float wallCheckRadius;
	public LayerMask WhatIsWall;
	
	public Transform groundCheckPoint1;
	public Transform groundCheckPoint2;
	
	public Transform leftWallCheckPoint1;
	public Transform leftWallCheckPoint2;
	
	public Transform rightWallCheckPoint1;
	public Transform rightWallCheckPoint2;
	
	public bool huggingLeftWall = false;
	public bool huggingRightWall = false;
	
	[HideInInspector] public bool shootingHook = false;
	private RaycastHit2D hookCollisionHit;
	private bool creatRopeNow = false;
	
	public GameObject theActualHook;
	private bool didWeHitGrapplabe = false;
	private bool retractingTheHook = false;
	public Transform hitPositionTempObj;
	private float jumpTimer = 0;

	//SOUND
	private LevelManager sound;
	
	//private float move; // The horizontal movement input by the player's controls

	public Vector3 mouseDirection;
	public Transform gamepadDirectionTransform;
	
	// Use this for initialization
	void Start () {

		Cursor.visible = false;
		
		playerRigidBody = this.GetComponent<Rigidbody2D>(); // Get RigidBody component of the player
		
		grapple = GetComponent<SpringJoint2D> (); // Get component of SpringJoint2D 
		grapple.enabled = false;                  // Disable the SpringJoint between the player and the hooable object
		rope = GetComponent<LineRenderer> ();     // Access the linerenderer for the rope
		hitObject = GameObject.FindGameObjectWithTag("GrappleHit");  // Get the object that points to the grapple point
		
		// The code for the rendering of the line
		rope.SetWidth(grapplingRopeWidth, grapplingRopeWidth);         // Width of the rope
		rope.SetVertexCount(2);              // Number of rope elements
		rope.material.color = Color.black;   // Rope is black
		rope.enabled = false;                // Make rope not render by default
		
		ropeCollisionPoints = new Stack();
		ropeCollPoes = new ArrayList();
		
		oldRopeTotalLength = 0;
		oldRopeSegmentLengths = new Stack();
		
		theActualHook.SetActive(false);

		sound = GameObject.Find ("Level Manager").GetComponent<LevelManager>();

		//for accessing the gamepad values to aim with controller
		grapplingHookController = GameObject.Find ("GamepadCrosshair").GetComponent<GrapplingHookController> ();
		gamepadDirectionTransform = GameObject.Find ("GamepadDirection").GetComponent<Transform> ();

	}
	
	
	//BÆTT INN!!!!!
	void FixedUpdate() {
		grounded = Physics2D.OverlapArea (groundCheckPoint1.position, groundCheckPoint2.position, whatIsGround);
		
		if ((grounded == true) && !isGrappled && (jumpTimer <= 0)) {
			playerRigidBody.drag = 10.5f;
		} else {
			playerRigidBody.drag = 0.25f;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("Update is Run");
		
		// For Laser sights
		
		//Vector3 mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		//mouseDirection.Normalize();
		//Debug.DrawRay(transform.position, mouseDirection*4, Color.red);
		// --------------------------------
		
		move = Input.GetAxisRaw ("Horizontal");
		moveVertical = Input.GetAxisRaw ("Vertical");
		
		velocityX = playerRigidBody.velocity.x;
		velocityY = playerRigidBody.velocity.y;

		//GAMEPAD SETTINGS BEGIN
		GPAD_L1 = Input.GetButton ("L1");
		GPAD_JUMP = Input.GetButtonDown ("X") || GPAD_L1; //jump with X or L1
		GPAD_SHOOT_HOLD = Input.GetButton ("R1");
		GPAD_SHOOT_DOWN = Input.GetButtonDown ("R1");
		GPAD_SHOOT_UP = Input.GetButtonUp ("R1");
		GPAD_START = Input.GetButton ("Start");
		GPAD_TRIGGER_VALUE = Input.GetAxis ("Triggers");
		GPAD_RANALOG_VALUE_X = Input.GetAxis ("R_analog_horz");
		GPAD_RANALOG_VALUE_Y = Input.GetAxis ("R_analog_vert") * -1;

		//if player is moving left analog stick right
		if (move > 0.1f) {
			GPAD_LANALOG_RIGHT = true;
		} else {
			GPAD_LANALOG_RIGHT = false;
		}

		//if player is moving left analog stick left
		if (move < -0.1f) {
			GPAD_LANALOG_LEFT = true;
		} else {
			GPAD_LANALOG_LEFT = false;
		}

		//if player is moving left analog stick up
		if (moveVertical > 0.9f) {
			GPAD_LANALOG_UP = true;
		} else {
			GPAD_LANALOG_UP = false;
		}
		
		//if player is moving left analog stick down
		if (moveVertical < -0.9f) {
			GPAD_LANALOG_DOWN = true;
		} else {
			GPAD_LANALOG_DOWN = false;
		}

		//if player is holding down L2 trigger
		if (GPAD_TRIGGER_VALUE > 0.9f) {
			GPAD_L2_TRIGGER = true;
		} else {
			GPAD_L2_TRIGGER = false;
		}

		//if player is holding down R2 trigger
		if (GPAD_TRIGGER_VALUE < -0.1f) {
			GPAD_R2_TRIGGER = true;
		} else {
			GPAD_R2_TRIGGER = false;
		}

		//GAMEPAD SETTINGS END

		
		//checks if player is falling
		if (playerRigidBody.velocity.y < 0 && !isGrappled) {
			falling = true;
		} else {
			falling = false;
		}
		
		if (!hasGrapplingHook) {
			grapplingHook.SetActive (false);
		} else {
			grapplingHook.SetActive (true);
		}
		
		// -----------------------------------------
		// Creates the rope and activates the spring joint if the hook has reached the grapplable surface
		if (creatRopeNow == true) {
			creatRopeNow = false;
			shootingHook = false;
			activateTheRope ();
		}
		
		// called if the left-mousebutton is pressed
		if (((Input.GetMouseButtonDown (0) || GPAD_SHOOT_DOWN) && !shootingHook) && !hasDistraction && hasGrapplingHook) {
			shootGrapplingHook ();
		}
		
		// If you are grappling a surface and you are holding down the left mouse button
		else if ((((Input.GetMouseButton (0) || GPAD_SHOOT_HOLD) && !shootingHook) && isGrappled) && !hasDistraction && hasGrapplingHook) {
			movementWhileGrappled ();
		}
		
		// To let go of the grappling hook
		else if ((Input.GetMouseButtonUp (0) || GPAD_SHOOT_UP) && !shootingHook && !hasDistraction && hasGrapplingHook && isGrappled) {
			letGo ();
			theActualHook.SetActive (false);
		}
		
		//--This is the standard moveset--
		else {
			//--While hook is travelling to the grapplable surface and the player is holdin the left mausebutton---
			if ((Input.GetMouseButton (0) || GPAD_SHOOT_HOLD) && !retractingTheHook && shootingHook && !hasDistraction && hasGrapplingHook && !isGrappled) {
				grapplingHookIsTravelling ();
			} else if (retractingTheHook) {
				retractingGrappelingHook ();
			}
			// If player releases the left mousebutton while the hook is still traveling to the grapplable surface then it is stopped
			else if ((Input.GetMouseButtonUp (0) || GPAD_SHOOT_UP) && shootingHook && !hasDistraction && hasGrapplingHook && !isGrappled) {
				shootingHook = false;
				rope.enabled = false;
				theActualHook.SetActive (false);
			}
			
			//-------------------------------------------
			/*
			grounded = Physics2D.OverlapArea (groundCheckPoint1.position, groundCheckPoint2.position, whatIsGround);
			
			if ((grounded == true) && !isGrappled && (jumpTimer <= 0)) {
				playerRigidBody.drag = 10.5f;
			} else {
				playerRigidBody.drag = 0.25f;
			}
			*/
			
			huggingLeftWall = Physics2D.OverlapArea (leftWallCheckPoint1.position, leftWallCheckPoint2.position, WhatIsWall);
			huggingRightWall = Physics2D.OverlapArea (rightWallCheckPoint1.position, rightWallCheckPoint2.position, WhatIsWall);
			
			if (hasDistraction) {
				if (distractionDelay > 0 && distractionTransforms != null) {
					distractionDelay -= Time.deltaTime;
					hitObject.transform.position = distractionTransforms.position;
					hitPosition = hitObject.transform.position;
					rope.enabled = true;
					redrawRope ();
				} else {
					hasDistraction = false;
					rope.enabled = false;
				}
			}
			
			//--Main ground movement--
			if (grounded) {
				normalPlayerMovement ();
			}
			
			//--movement after detaching the grapple hook--
			/*else if(wasGrappled) {
				afterBeingGrappledMovement();
			}*/
			
			//--Main air movement--
			else {
				jumpPlayerMovement ();
			}
			
			// Walljump on left wall
			if ((Input.GetKeyDown (KeyCode.Space) || GPAD_JUMP) && huggingLeftWall && !grounded) {
				knockPlayer (true);
				knockBackCount = knockBackLength;
				wallJump = true;
			}
			// Walljump on right wall
			else if ((Input.GetKeyDown (KeyCode.Space) || GPAD_JUMP) && huggingRightWall && !grounded) {
				knockPlayer (false);
				knockBackCount = knockBackLength;
				wallJump = true;
			}
			//--Main jump code--
			else if (((Input.GetKeyDown (KeyCode.Space) || GPAD_JUMP) && (jumpTimer <= 0)) && grounded) {
				//playerRigidBody.drag = 0.25f;
				wallJump = false;
				jumpTimer = 0.3f;
				mainJump ();
			}
			          
			jumpTimer -= Time.deltaTime;
		}
	}


	public void shootGrapplingHook(){
			//Vector3 mouseDirection;
			Vector3 gamepadVector = gamepadDirectionTransform.position - shooter.position;

			if(grapplingHookController.useGamepad){

			mouseDirection = gamepadVector;
			//Debug.Log ("gamepadDirectionTransform.position: " + gamepadDirectionTransform.position);
			//Debug.DrawLine(gamepadDirectionTransform.position, shooter.position, Color.red);


			//mouseDirection = new Vector3( GPAD_RANALOG_VALUE_X, GPAD_RANALOG_VALUE_Y, 0) - shooter.position;

			//float gamepadAimAngleSin = Mathf.Sin (Mathf.Deg2Rad * grapplingHookController.aim_angle);
			//float gamepadAimAngleCos = Mathf.Cos(Mathf.Deg2Rad * grapplingHookController.aim_angle);

			//mouseDirection = new Vector3(gamepadAimAngleSin, gamepadAimAngleCos, 0);
				//mouseDirection = Quaternion.AngleAxis(grapplingHookController.aim_angle + 90f, Vector3.forward) * Vector3.left;
			}
			else{
				mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - shooter.position;
				
			}

			///////////////-------------------------------------------------------------------------------------

			sound.playGrappleShoot ();
		
			// Gets the direction of the mouse relative to the player(the direction that he tried to shoot the grappling hook)
			mouseDirection.Normalize(); // A little Linear Algebra to get the direction vector
			//   Vector3 mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - shooter.position;
			
			
			// Casts a ray to see if the player shot at a grapplable surface
			RaycastHit2D hit = Physics2D.Raycast(shooter.position, mouseDirection, maxRopeLength, layersDetectedByHook);
			
			if (hit.collider != null) {
				if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Grapplabe")) { // true when we hit a grapplable surface

					if(hit.collider.gameObject.tag == "Vine"){
						sound.playGrappleHitVine();
					}

					shootingHook = true;
			
					hitObject.transform.position = shooter.position;
					hitPositionTempObj.position = hit.point;
					hookCollisionHit = hit;
					didWeHitGrapplabe = true;
				} else if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("TrapsForEnemys")) {
					Rigidbody2D distractionObject = hit.collider.GetComponent<Rigidbody2D> ();
					distractionObject.isKinematic = false;
					distractionTransforms = hit.collider.GetComponent<Transform> ();
					hasDistraction = true;
					distractionDelay = 0.5f;
				} else {
					didWeHitGrapplabe = false;
					shootingHook = true;
					hitPositionTempObj.position = hit.point;
					hitObject.transform.position = shooter.position;
				}
			}
			else {
				// Setting up the parameters for the failed grappling animation
				didWeHitGrapplabe = false;
				shootingHook = true;
				//hitPositionTempObj.position = new Vector3(transform.position.x + (mouseDirection.x * maxRopeLength), transform.position.y + (mouseDirection.y * maxRopeLength), 0);
				hitPositionTempObj.position = shooter.position;

				if(grapplingHookController.useGamepad){
					hitPositionTempObj.LookAt(new Vector3(mouseDirection.x + shooter.position.x, mouseDirection.y + shooter.position.y, 0));
				}
				else{
					hitPositionTempObj.LookAt(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0));
				}

				hitPositionTempObj.Translate(0, 0, maxRopeLength, Space.Self);
				
				hitObject.transform.position = shooter.position;
			}
		}
			
			public void grapplingHookIsTravelling()
			{
				float step = grapplingHookShotTime * Time.deltaTime;
				hitObject.transform.position = Vector3.MoveTowards(hitObject.transform.position, hitPositionTempObj.position, step);
				
				theActualHook.SetActive(true);
				theActualHook.transform.position = hitObject.transform.position;
				
				redrawRope();
				rope.enabled = true;
				if((Mathf.Abs(hitObject.transform.position.x - hitPositionTempObj.position.x) < 0.01f) && (Mathf.Abs(hitObject.transform.position.y - hitPositionTempObj.position.y) < 0.01f)) {
					if(didWeHitGrapplabe) {
						creatRopeNow = true;
					}
					else {
						retractingTheHook = true;
					}
					
				}
			}
			
			public void retractingGrappelingHook()
			{
				float step = 1.5f * grapplingHookShotTime * Time.deltaTime;
				hitObject.transform.position = Vector3.MoveTowards(hitObject.transform.position, shooter.position, step);
				
				theActualHook.SetActive(true);
				theActualHook.transform.position = hitObject.transform.position;
				
				redrawRope();
				rope.enabled = true;
				if((Mathf.Abs(hitObject.transform.position.x - shooter.position.x) < 0.01f) && (Mathf.Abs(hitObject.transform.position.y - shooter.position.y) < 0.01f)) {
					retractingTheHook = false;
					rope.enabled = false;
					theActualHook.SetActive(false);
					shootingHook = false;
				}
			}
			
			
			public void activateTheRope()
			{
				playerRigidBody.gravityScale = 0.75f;
				playerRigidBody.drag = 0.25f;
				createOriginalRope (hookCollisionHit);
				
				rope.enabled = true; // lets the rendered rope be visible
				redrawRope (); // Actually the first drawing :/*/
			}
			
			public void movementWhileGrappled()
			{
				redrawRope ();
				
				Vector3 oldRopeAngle = hitPosition - shooter.position;
				oldRopeAngle.Normalize ();
				Vector3 rayFromRope = new Vector3 (hitPosition.x - (oldRopeAngle.x * 0.05f), hitPosition.y - (oldRopeAngle.y * 0.05f), 0);
				
				// Casts a ray to see if the player shot at a grapplable surface
				RaycastHit2D newRopeHit = Physics2D.Linecast(shooter.position, rayFromRope, collidableLayersForRope);
				
				// Checks if the rope collided with something
				if (newRopeHit.collider!= null) { // true when the rope hits a surface
					createNewRopeJoint(newRopeHit); // Creates a new rope joint at the collided surface
				}
				
				//--remove a rope joint if it is no longer needed--
				if (ropeCollisionPoints.Count > 0) {
					removeRopeJoint();
				}
				
				//--Swing right on rope (When D key is pressed)--
				if (Input.GetKey (KeyCode.D) || GPAD_LANALOG_RIGHT) {
					//Debug.Log("Grappling Movement Run");
					playerRigidBody.AddForce(new Vector2(move * ropeSwingSpeed * 72 * Time.deltaTime, 0));
				}
				
				//--Swing left on rope (When A key is pressed)--
				if (Input.GetKey (KeyCode.A) || GPAD_LANALOG_LEFT) {
					//Debug.Log("Grappling Movement Run");
					playerRigidBody.AddForce(new Vector2(move * ropeSwingSpeed * 72 * Time.deltaTime, 0));
				}
				
				//--Reel in the rope--
				if(Input.GetKey(KeyCode.W) || Input.GetMouseButton (1) || GPAD_R2_TRIGGER || GPAD_LANALOG_UP) {
					grapple.distance = grapple.distance - (ropeReelSpeed * 68 * Time.deltaTime);
				}
				
				//--Loosen the rope--
				if(Input.GetKey(KeyCode.S) || GPAD_L2_TRIGGER || GPAD_LANALOG_DOWN) {
					// Make shure we don't extend the rope beyond the maximum length of the rope
					if((grapple.distance + oldRopeTotalLength) <= maxRopeLength){
						grapple.distance = grapple.distance + (ropeReelSpeed * 68 * Time.deltaTime);
					}
				}
				//Debug.Log(grapple.distance + oldRopeTotalLength);
			}
			
			
			// When you let go of the left mouse button on letgo of the grappling hook
			public void letGo()
			{
				isGrappled = false;
				rope.enabled = false; // hide the rendered rope
				grapple.enabled = false; // deactivate the spring joint
				wasGrappled = true; // bool for the movement right after you detach the grappling hook
				oldRopeTotalLength = 0;
				oldRopeSegmentLengths.Clear ();
				ropeCollisionPoints.Clear();
				ropeCollPoes.Clear ();
				playerRigidBody.gravityScale = 0.87f;
			}
			
			public void normalPlayerMovement()
			{
				wallJump = false;
				
				if ((knockBackCount <= 0)) 
				{ 
					playerRigidBody.AddForce(new Vector2(move * 33 * 65, 0) * Time.deltaTime,ForceMode2D.Force);  
				}
				else {
					knockBackCount -= Time.deltaTime;
				}
			}
			
			
			public void jumpPlayerMovement()
			{
				if ((knockBackCount <= 0) ) { 
					if (Mathf.Abs (move) <= 0.001f) {
						
					} else {
						// Impulse Version
						if((playerRigidBody.velocity.x < -0.08) && (move > 0.01f))
						{
							playerRigidBody.AddForce (new Vector2 (move * 0.85f, 0), ForceMode2D.Impulse); 
						}
						else if((playerRigidBody.velocity.x > 0.08) && (move < -0.01))
						{
							playerRigidBody.AddForce (new Vector2 (move * 0.85f, 0), ForceMode2D.Impulse); 
						}
						else if((Mathf.Abs (playerRigidBody.velocity.x) <= 3.5f)) {
							playerRigidBody.AddForce (new Vector2 (move * 6.4f * 55 * Time.deltaTime, 0), ForceMode2D.Force); 
						}
					}
				}
				else {
					knockBackCount -= Time.deltaTime;
				}
			}
			
			public void mainJump()
			{
				playerRigidBody.AddForce(new Vector2(0, 6),ForceMode2D.Impulse); 
			}
			
			
			public void createOriginalRope(RaycastHit2D hit)
			{
				ropeCollisionPoints = new Stack();
				ropeCollPoes = new ArrayList();
				Vector3 normallinn = hit.normal;
				
				hitObject.transform.position = new Vector3(hit.point.x + (normallinn.x * 0.01f), hit.point.y + (normallinn.y * 0.01f), 0);
				hitPosition = hitObject.transform.position;
				
				// Calculates the length of the rope
				grapple.distance = Vector2.Distance(hitPosition, shooter.position);
				grapple.distance = grapple.distance - (grapple.distance / 9); // Shortens the rope a little so its pulls you just slightly
				
				grapple.enabled = true; // activate the spring joint for grappling hanging
				isGrappled = true;
			}
			
			public void redrawRope()
			{
				int counter = ropeCollPoes.Count;
				rope.SetVertexCount (counter + 2);
				for(int i = 0; i < counter; i++) 
				{
					rope.SetPosition(i, (Vector3)ropeCollPoes[i]);
				}
				rope.SetPosition (counter, hitObject.transform.position);
				rope.SetPosition (counter+1, shooter.position);
			}
			
			public void createNewRopeJoint(RaycastHit2D newRopeHit)
			{
				float oldTempDist = Vector3.Distance(hitPosition,shooter.position);
				
				// Length of the previus egment of the rope(the one before the current collision)
				//float oldRopeSegmentLength = Vector3.Distance(newRopeHit.point, hitPosition);
				
				// Keeping tabs on the current length of the parts of the rope that are not active at this time
				//oldRopeTotalLength += oldRopeSegmentLength;
				//oldRopeSegmentLengths.Push(oldRopeSegmentLength);
				
				// Adding the old collision point to the collection of rope collision points
				ropeCollisionPoints.Push(hitObject.transform.position);
				ropeCollPoes.Add(hitObject.transform.position);
				
				// Calculactes the normal of the surface the rope collided with
				Vector3 normallinn = newRopeHit.normal;
				
				// The new collision point is always located a small distance from the normal of the collider surface
				Vector3 newJointLocation = new Vector3(newRopeHit.point.x + (normallinn.x * 0.02f), newRopeHit.point.y + (normallinn.y * 0.02f), 0);
				
				// Length of the previus egment of the rope(the one before the current collision)
				float oldRopeSegmentLength = Vector3.Distance(newJointLocation, hitPosition);
				// Keeping tabs on the current length of the parts of the rope that are not active at this time
				oldRopeTotalLength += oldRopeSegmentLength;
				oldRopeSegmentLengths.Push(oldRopeSegmentLength);
				
				hitObject.transform.position = newJointLocation;
				hitPosition = hitObject.transform.position;
				
				// The new distance of the current used rope is updated
				//grapple.distance = Vector2.Distance(hitPosition, transform.position);
				grapple.distance = oldTempDist - oldRopeSegmentLength - 0.2f;
			}
			
			public void removeRopeJoint()
			{
				RaycastHit2D isOldHit = Physics2D.Linecast((Vector3)ropeCollisionPoints.Peek(), shooter.position, collidableLayersForRope);
				if(isOldHit.collider == null)
				{
					hitObject.transform.position = (Vector3)ropeCollisionPoints.Pop (); // Move the rope swing joint position to the old position
					hitPosition = hitObject.transform.position;
					//grapple.distance = Vector2.Distance(hitPosition, transform.position);
					grapple.distance = (grapple.distance + (float)oldRopeSegmentLengths.Peek()); // Making the active rope the correct length 
					oldRopeTotalLength -= (float)oldRopeSegmentLengths.Pop(); // 
					ropeCollPoes.RemoveAt(ropeCollPoes.Count-1);
				}
			}
		
			public void knockPlayer(bool left){
				if (left) {
					playerRigidBody.velocity = new Vector2 (knockBack, knockBack);
					
				} 
				else {
					playerRigidBody.velocity = new Vector2 (-knockBack, knockBack);
				}
			}
}
