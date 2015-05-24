using UnityEngine;
using System.Collections;

public class Alien_Head_Controller : MonoBehaviour {

	public Transform player;
	float angle;
	public float minAngle;
	public float maxAngle;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player_Bird").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		angle = Mathf.Atan2(player.transform.position.x - this.transform.position.x, player.transform.position.y - this.transform.position.y) * Mathf.Rad2Deg;

		Vector3 euler = new Vector3 (0, 0, -angle);

		//transform.rotation = Quaternion.Euler(euler);
		Quaternion mouseTarget = Quaternion.Euler (euler);
		transform.rotation = mouseTarget;// Quaternion.Slerp(transform.rotation, mouseTarget, 0.1f);
	}
}
