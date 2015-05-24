using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {

	public float damping;
	public Color crosshairColor;
	public Color grappledCrosshairColor;
	//private Vector3 oldPos;

	// Use this for initialization
	void Start () {
		crosshairColor = new Color (1f, 1f, 1f, 1f);
		//oldPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newMouse = Input.mousePosition;
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (newMouse);
		Vector3 newPos = new Vector3();
		newPos.x = mousePosition.x;
		newPos.y = mousePosition.y;
		newPos.z = 0f;
		//oldPos = transform.position;
		//transform.position = Vector3.MoveTowards(oldPos, newPos, damping);
		//transform.position = Vector3.Lerp(oldPos, newPos, damping);
		transform.position = newPos;

	}
}
