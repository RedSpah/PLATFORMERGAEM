using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class S_UISystem : MonoBehaviour {

	public int deaths = 0;
	public int time = 0;
	public bool running = false;
	private Text UIText;
	public bool endlevel = false;
	private bool replaying = false;

	private int min = 0, sec = 0; 
	private float msec = 0;

	// Use this for initialization
	void Start () {
		UIText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (running) {
			min = Mathf.FloorToInt (time / 3600);
			sec = Mathf.FloorToInt (time / 60) % 60;
			msec = Mathf.FloorToInt((time % 60) * 1000/60); 
			bool a = false, b = false, c = false;
			if (sec < 10) {a = true;}
			if (msec < 100) {b = true;}
			if (msec < 10) {c = true;}
			UIText.text = " \r\n   Time: " + min + ":"; 
			if (a) {UIText.text += "0";}
			UIText.text += sec + ".";
			if (b) {UIText.text += "0";}
			if (c) {UIText.text += "0";}
			UIText.text += msec + "\r\n   Deaths: " + deaths; 
			//UIText.text = "\r\n \r\n    " + time.ToString ("F0") + "\r\n \r\n    " + "Deaths: " + deaths; 
			if (replaying && !endlevel)
			{
				UIText.text += " \r\n  \r\n  \r\n     REPLAYING";
			}
		if (!running)
			{
				UIText.text = " ";
			}

		}
		if (endlevel) {
			UIText.fontSize = 48;
			UIText.color = new Color(255,0,0);
			bool a = false, b = false, c = false;
			if (sec < 10) {a = true;}
			if (msec < 100) {b = true;}
			if (msec < 10) {c = true;}
			UIText.text = " \r\n   Time: " + min + ":"; 
			if (a) {UIText.text += "0";}
			UIText.text += sec + ".";
			if (b) {UIText.text += "0";}
			if (c) {UIText.text += "0";}
			UIText.text += msec + "\r\n   Deaths: " + deaths; 
			UIText.text += " \r\n \r\n \r\n PRESS [A] TO REPLAY \r\n PRESS [S] TO RETRY";
			endlevel = false;
		}
	}



	public void setupReplay()
	{
		replaying = true;
	}
	
	public void resetlevel()
	{
		deaths = 0;
		time = 0;
		UIText.fontSize = 14;
		UIText.color = new Color(255,255,255);
		UIText.text = " ";
		replaying = false;
		endlevel = false;
	}
}
