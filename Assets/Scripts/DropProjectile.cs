using UnityEngine;
using System.Collections;

public class DropProjectile : MonoBehaviour {

	public GameObject enemyProjectile;
	public Player_Script player;
	public Transform launchPoint;
	public FlyingEnemyPatrol checkEnemy;
	
	public float waitBetweenShots;
	private float shotCounter;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player_Script> ();
		shotCounter = waitBetweenShots;
	}
	
	// Update is called once per frame
	void Update () {
		shotCounter -= Time.deltaTime;
		if (checkEnemy.stunned) {
			return;
		}
		if (shotCounter < 0) {
			Instantiate(enemyProjectile, launchPoint.position, launchPoint.rotation);
			shotCounter = waitBetweenShots;
		}
	}
}
