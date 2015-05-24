using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

	public static int playerHealth;

	public int maxPlayerHealth;
	
	Text text;

	private LevelManager levelManager;

	public bool isDead;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		playerHealth = maxPlayerHealth;
		levelManager = FindObjectOfType<LevelManager> ();
		isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerHealth <= 0 && !isDead) {
			playerHealth = 0;
			isDead = true;
			levelManager.RespawnPlayer();
		}
		text.text = "" + playerHealth;
	}

	public static void HurtPlayer(int damageToGive){
		playerHealth -= damageToGive;
	}

	public void FullHealth(){
		playerHealth = maxPlayerHealth;
	}
}
