using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnergyBarSwapper : MonoBehaviour {
	
	public Sprite [] energyBars;
	public Image image;
	
	// Use this for initialization
	void Start () {
		image = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		image.sprite = energyBars[EnergyManager.energy];
	}
}
