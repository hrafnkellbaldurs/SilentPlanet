using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public int playerLives;

	public string startLevel;

	public void Start(){
		Cursor.visible = true;
	}

	public void Gamepad(){
//		if (paused.isPaused) {
//			paused.isPaused = !paused.isPaused;
//		}
		PlayerPrefs.SetInt ("Controls", 0);
		Cursor.visible = false;
		NewGame ();
	}

	public void Mouse(){
		//		if (paused.isPaused) {
		//			paused.isPaused = !paused.isPaused;
		//		}
		PlayerPrefs.SetInt ("Controls", 1);
		Cursor.visible = false;
		NewGame ();
	}

	public void NewGame(){
		//		if (paused.isPaused) {
		//			paused.isPaused = !paused.isPaused;
		//		}
		PlayerPrefs.SetInt ("PlayerCurrentLives", playerLives);
		Application.LoadLevel (startLevel);
	}

	public void QuitGame(){
		Application.Quit();
	}

}