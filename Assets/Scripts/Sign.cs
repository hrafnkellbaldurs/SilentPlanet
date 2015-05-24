using UnityEngine;
using System.Collections;

public class Sign : MonoBehaviour {

	public ToolTipController render;
	public string theMessage;

	// Use this for initialization
	void Start () {
//		render = FindObjectOfType<ToolTipController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.name == "Player_Bird"){
			render.Entering(theMessage);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.name == "Player_Bird"){
			render.Exiting();
		}
	}
}
