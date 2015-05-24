using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToolTipController : MonoBehaviour {
//	public bool isActive;
	
	public GameObject toolTipCanvas;
	public Text text;

	void Start() {
//		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
//		if (isPaused) {
//			pauseMenuCanvas.SetActive(true);
//			Time.timeScale = 0f;
//		} 
//		else {
//			pauseMenuCanvas.SetActive(false);
//			Time.timeScale = 1f;
//		}
//		if (Input.GetKeyDown (KeyCode.Escape)) {
//			isPaused = !isPaused;
//		}
	}
	public void Entering(string theMessage){
		text.text = theMessage;
		toolTipCanvas.SetActive (true);
	}

	public void Exiting(){
		toolTipCanvas.SetActive (false);
	}
}
