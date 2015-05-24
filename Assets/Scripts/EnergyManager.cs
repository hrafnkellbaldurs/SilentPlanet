using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour {
	public static int energy;
	public HealthBarSwapper healthBarSwapper;

	Text text;

	void Start(){
		//text = GetComponent<Text> ();
		energy = 0;
		healthBarSwapper = FindObjectOfType<HealthBarSwapper> ();
	}

	void Update(){
		//if (energy < 0) {
		//	energy = 0;
		//}
		if (energy == 5) {
			healthBarSwapper.IncreaseMaxHealth ();
			Reset();
		}
		//text.text = "" + energy + "/5";
	}

	public static void AddEnergy (int energyToAdd){
		energy += energyToAdd;
	}

	public static void Reset(){
		energy = 0;
	}
	
}