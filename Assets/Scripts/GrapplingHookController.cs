using UnityEngine;
using System.Collections;

public class GrapplingHookController : MonoBehaviour {
	
	//public GameObject toRotate;
	public float cameraDistance; //The distance between the camera and object
	public bool useGamepad;
	public Player_Script pScript;
	public float crosshairFadeTime;
	public Color crosshairColor;
	private Color transparent;
	private Color showGrapplingHand;
	private Color hideGrapplingHand;
	private Color grappledCrosshairColor;
	public Transform grapplingHandTransform;
	public float hand_angle;
	public float aim_angle;
	public Transform hookHitTransform;


	[HideInInspector]  public float rAnalogX; 
	[HideInInspector]  public float rAnalogY; 
	
	//float angle;
	//PlayerAnimation pAnim;
	GameObject crosshair;

	SpriteRenderer crosshairRenderer;
	SpriteRenderer gamepadCrosshairRenderer;
	SpriteRenderer grapplingHandRenderer;

	Vector3 mousePos;
	Vector3 grapplingHandPos;
	public float mouseGrapplingHandAngle;
	
	
	void Start(){
		if (PlayerPrefs.GetInt ("Controls") == 0) {
			useGamepad = true;
		} 
		else {
			useGamepad = false;
		}
		
		//GRAPPLING HAND COLOR MANIPULATION
		grapplingHandRenderer = GameObject.Find ("GrapplingHand").GetComponent<SpriteRenderer> ();
		showGrapplingHand = grapplingHandRenderer.color;
		hideGrapplingHand = showGrapplingHand;
		hideGrapplingHand.a = 0f;
		grapplingHandRenderer.color = hideGrapplingHand;
		//GRAPPLING HAND COLOR MANIPULATION

		//target = GameObject.Find (toRotate.name).transform;
		pScript = GameObject.Find ("Player_Bird").GetComponent<Player_Script>();
		//pAnim = GameObject.Find ("PlayerAnimation").GetComponent<PlayerAnimation> ();
		crosshair = GameObject.Find ("Crosshair");
		crosshairColor = crosshair.GetComponent<FollowMouse> ().crosshairColor;
		crosshairRenderer = crosshair.GetComponent<SpriteRenderer> ();
		gamepadCrosshairRenderer = GameObject.Find ("GamepadCrosshair").GetComponent<SpriteRenderer> ();
		crosshairFadeTime = 0.2f;
		transparent = crosshairColor;
		transparent.a = 0f;
		grappledCrosshairColor = crosshair.GetComponent<FollowMouse> ().grappledCrosshairColor;

		cameraDistance = 4f;

		grapplingHandTransform = GameObject.Find ("GrapplingHand").GetComponent<Transform> ();

		//We start out with an invisible crosshair
		crosshairRenderer.color = transparent;
		gamepadCrosshairRenderer.color = transparent;

		hookHitTransform = GameObject.Find ("GrapppleHitPoint").GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {

		rAnalogX = pScript.GPAD_RANALOG_VALUE_X;
		rAnalogY = pScript.GPAD_RANALOG_VALUE_Y;

		if (useGamepad) {
			// ROTATE A GUN OBJECT AROUND THE Z-AXIS
			// BASED ON THE ANGLE OF THE RIGHT ANALOG STICK

			aim_angle = 0.0f;
			
			// CALCULATE ANGLE AND ROTATE
			if (rAnalogX != 0.0f || rAnalogY != 0.0f) {

				if(pScript.hasGrapplingHook){
					if(pScript.isGrappled){

						rotateGrapplingHandToGrapple(); 

						Color lerpedColor = gamepadCrosshairRenderer.color; 
						gamepadCrosshairRenderer.color = Color.Lerp(lerpedColor, grappledCrosshairColor, crosshairFadeTime);
					}
					else{
						Color lerpedColor = gamepadCrosshairRenderer.color; 
						gamepadCrosshairRenderer.color = Color.Lerp(lerpedColor, crosshairColor, crosshairFadeTime);
						//gamepadCrosshairRenderer.color = crosshairColor;
					}

				}
				
				aim_angle = Mathf.Atan2(rAnalogY, rAnalogX) * Mathf.Rad2Deg - 90f;
				hand_angle = Mathf.Atan2 (rAnalogY, rAnalogX) * Mathf.Rad2Deg;

				// ANGLE LASER
				transform.rotation = Quaternion.AngleAxis(aim_angle, Vector3.forward);
				
				//if player just shot grappling hook and does not hit grapplable he moves his hand towards it
				if (pScript.shootingHook) {
					grapplingHandTransform.rotation = Quaternion.AngleAxis(hand_angle, Vector3.forward);
					grapplingHandRenderer.color = showGrapplingHand;
				}
				//if player shot grappling hook and hit grapplable
				else if(pScript.isGrappled){
					//TODO make arm follow grappled area
					//grapplingHandTransform.rotation = Quaternion.AngleAxis(hand_angle, Vector3.forward); //change the rotation to the point
					//grapplingHandRenderer.color = showGrapplingHand;
					rotateGrapplingHandToGrapple();
				}
				//hide grappling hand if player is not shooting grappling hook
				else{
					grapplingHandRenderer.color = hideGrapplingHand;
				}
			}
			else{ //if right analog stick is not being moved
				Color lerpedColor = gamepadCrosshairRenderer.color; 
				gamepadCrosshairRenderer.color = Color.Lerp(lerpedColor, transparent, crosshairFadeTime);

				if(pScript.isGrappled){
					grapplingHandRenderer.color = showGrapplingHand;
				}
			}
			
		} 
		else { //if player is using mouse

			if(pScript.hasGrapplingHook){

				if (pScript.shootingHook) {

					rotateGrapplingHandToMouse(); 

					grapplingHandRenderer.color = showGrapplingHand;

					//grapplingHandTransform.rotation = Quaternion.AngleAxis(hand_angle, Vector3.forward);

				}
				else if(pScript.isGrappled){

					rotateGrapplingHandToGrapple(); 

					grapplingHandRenderer.color = showGrapplingHand;

					Color lerpedColor = crosshairRenderer.color; 
					crosshairRenderer.color = Color.Lerp(lerpedColor, grappledCrosshairColor, crosshairFadeTime);
				}
				else{
					grapplingHandRenderer.color = hideGrapplingHand;

					Color lerpedColor = crosshairRenderer.color; 
					crosshairRenderer.color = Color.Lerp(lerpedColor, crosshairColor, crosshairFadeTime);

					//Color lerpedColor = crosshairRenderer.color; 
					//crosshairRenderer.color = Color.Lerp(lerpedColor, transparent, crosshairFadeTime);
				}
			}
			else{
				grapplingHandRenderer.color = hideGrapplingHand;
				//grapplingHandRenderer.color = hideGrapplingHand;
			}
		}
	}

	public void rotateGrapplingHandToMouse(){

		mousePos = Input.mousePosition;
		mousePos.z = cameraDistance;

		//Normalize mouseposition
		grapplingHandPos = Camera.main.WorldToScreenPoint (grapplingHandTransform.position);
		//Debug.Log ("grapplinghandpos: " + grapplingHandPos);
		
	//if (!grappled) {
		//fix mouse position for object
		mousePos.x = mousePos.x - grapplingHandPos.x;
		mousePos.y = mousePos.y - grapplingHandPos.y;
	//}
		
		//calculate the correct angle
		mouseGrapplingHandAngle = Mathf.Atan2 (mousePos.y, mousePos.x) * Mathf.Rad2Deg;
		//Debug.Log ("mouseangle: " + mouseGrapplingHandAngle);
		Vector3 euler = new Vector3 (0, 0, mouseGrapplingHandAngle);
		
		//apply the transformation
		grapplingHandTransform.rotation = Quaternion.Euler (euler);
		
	}

	public void rotateGrapplingHandToGrapple(){

		//if (useGamepad) {
		//} 
		//else {
		//}

		mousePos = hookHitTransform.position;
		mousePos.z = cameraDistance;
		
		Debug.Log ("mousepos: " + mousePos);
		
		//Normalize mouseposition
		//grapplingHandPos = Camera.main.WorldToScreenPoint (grapplingHandTransform.position);
		grapplingHandPos = grapplingHandTransform.position;
		//Debug.Log ("grapplinghandpos: " + grapplingHandPos);
		
		//if (!grappled) {
		//fix mouse position for object
		mousePos.x = mousePos.x - grapplingHandPos.x;
		mousePos.y = mousePos.y - grapplingHandPos.y;
		//}
		
		//calculate the correct angle
		mouseGrapplingHandAngle = Mathf.Atan2 (mousePos.y, mousePos.x) * Mathf.Rad2Deg;
		//Debug.Log ("mouseangle: " + mouseGrapplingHandAngle);
		Vector3 euler = new Vector3 (0, 0, mouseGrapplingHandAngle);
		
		//apply the transformation
		grapplingHandTransform.rotation = Quaternion.Euler (euler);
		
	}
}


