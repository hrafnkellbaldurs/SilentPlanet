using UnityEngine;
using System.Collections;

public class EnergyPicker : MonoBehaviour {

	public int energyToAdd;

	public AudioSource energyAudio;

//	void Start(){
//
//	}

	void OnTriggerEnter2D (Collider2D other){
		if(other.GetComponent<Player_Script>() == null){
			return;
		}
		EnergyManager.AddEnergy (energyToAdd);
		energyAudio.Play ();
		Destroy (gameObject);
	}
}
