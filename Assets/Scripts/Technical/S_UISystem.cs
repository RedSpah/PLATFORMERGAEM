using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class S_UISystem : MonoBehaviour {

	public int deaths = 0;
	public int time = 0;
	public bool running = false;
	[SerializeField] private Text UIText;
	public bool endlevel = false;
	private bool replaying = false;

	private int min = 0, sec = 0; 
	private float msec = 0;
	private bool showdeaths;

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

			UIText.text = " \r\n   Time: " + GetTimeString(min, sec, msec); 
			if (showdeaths)
			{
				UIText.text += "\r\n   Deaths: " + deaths; 
			}
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
			UIText.text = " \r\n   Time: " + GetTimeString(min, sec, msec);  
			if (showdeaths)
			{
				UIText.text +=  "\r\n   Deaths: " + deaths; 
			}
			UIText.text += " \r\n \r\n \r\n PRESS [A] TO REPLAY \r\n PRESS [S] TO RETRY \r\n PRESS [D] TO RETURN TO HUB";
			endlevel = false;
		}
	}



	public void setupReplay()
	{
		replaying = true;
	}
	
	public void ResetLevel(bool y)
	{
		deaths = 0;
		showdeaths = y;
		time = 0;
		UIText.fontSize = 14;
		UIText.color = new Color(255,255,255);
		UIText.text = " ";
		replaying = false;
		endlevel = false;
		running = false;
	}

	public void BeginLevel()
	{
		deaths = 0;
	}

	public string GetTimeString(int m, int s, float ms)
	{
		string y = "";
		bool a = false, b = false, c = false;
		if (s < 10) {a = true;}
		if (ms < 100) {b = true;}
		if (ms < 10) {c = true;}
		y += m;
		y += ":";
		if (a) {y += "0";}
		y += s + ".";
		if (b) {y += "0";}
		if (c) {y += "0";}
		y += ms;
		return y;
	}
}
