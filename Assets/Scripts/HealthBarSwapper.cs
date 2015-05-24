using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBarSwapper : MonoBehaviour {

	public Sprite [] healthBars;
	public int playerHealth;
	public int maxPlayerHealth;
	private LevelManager levelManager;
	public bool isDead;
	public Image image;
	private Player_Script playerScript;

	// Use this for initialization
	void Start () {
		maxPlayerHealth = PlayerPrefs.GetInt ("PlayerCurrentLives");
		playerScript = GameObject.Find ("Player_Bird").GetComponent<Player_Script>();
		image = GetComponent<Image> ();
		playerHealth = maxPlayerHealth;
		levelManager = FindObjectOfType<LevelManager> ();
		isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerHealth <= 0 && !isDead) {
			playerHealth = 0;
			levelManager.RespawnPlayer ();
			isDead = true;
			playerScript.isDead = true;
		} 
		updateHud ();
	}

	public void HurtPlayer(int damageToGive){
		playerHealth -= damageToGive;
		gameObject.GetComponent<AudioSource>().Play();
		updateHud ();
	}

	public void FullHealth(){
		playerHealth = maxPlayerHealth;
		updateHud ();
	}

	public void IncreaseMaxHealth(){
//		maxPlayerHealth++;
		PlayerPrefs.SetInt ("PlayerCurrentLives", (maxPlayerHealth + 1));
		maxPlayerHealth = maxPlayerHealth + 1;
		playerHealth = maxPlayerHealth;
		updateHud ();
	}

	private void threeBars(){
		if (playerHealth == 0) {
			image.sprite = healthBars [0];
		} 
		else if (playerHealth == 1) {
			image.sprite = healthBars [1];
		}
		else if (playerHealth == 2) {
			image.sprite = healthBars [2];
		}
		else {
			image.sprite = healthBars [3];
		}
	}

	private void fourBars(){
		if (playerHealth == 0) {
			image.sprite = healthBars [4];
		} 
		else if (playerHealth == 1) {
			image.sprite = healthBars [5];
		}
		else if (playerHealth == 2) {
			image.sprite = healthBars [6];
		}
		else if(playerHealth == 3) {
			image.sprite = healthBars [7];
		}
		else {
			image.sprite = healthBars [8];
		}
	}

	private void fiveBars(){
		if (playerHealth == 0) {
			image.sprite = healthBars [9];
		} 
		else if (playerHealth == 1) {
			image.sprite = healthBars [10];
		}
		else if (playerHealth == 2) {
			image.sprite = healthBars [11];
		}
		else if(playerHealth == 3) {
			image.sprite = healthBars [12];
		}
		else if(playerHealth == 4) {
			image.sprite = healthBars [13];
		}
		else {
			image.sprite = healthBars [14];
		}
	}

	private void sixBars(){
		if (playerHealth == 0) {
			image.sprite = healthBars [15];
		} 
		else if (playerHealth == 1) {
			image.sprite = healthBars [16];
		}
		else if (playerHealth == 2) {
			image.sprite = healthBars [17];
		}
		else if(playerHealth == 3) {
			image.sprite = healthBars [18];
		}
		else if(playerHealth == 4) {
			image.sprite = healthBars [19];
		}
		else if(playerHealth == 5) {
			image.sprite = healthBars [20];
		}
		else {
			image.sprite = healthBars [21];
		}
	}

	private void updateHud(){
		if (maxPlayerHealth == 3) {
			threeBars();
		}
		else if (maxPlayerHealth == 4) {
			fourBars();
		}
		else if (maxPlayerHealth == 5) {
			fiveBars();
		}
		else {
			sixBars();
		}
	}
}
