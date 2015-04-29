using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RedHelp;

public class S_UISystem : MonoBehaviour {

	public int deaths = 0;
	public int time = 0;
	public bool running = false;
	[SerializeField] private Text UIText;
	public bool endlevel = false;
	private bool replaying = false;
	
	private bool showdeaths;

	// Use this for initialization
	void Start () {
		UIText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (running) {

			UIText.text = " \r\n   Time: " + Helper.RenderTime(time); 
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
			UIText.fontSize = 26;
			UIText.color = new Color(255,0,0);
			UIText.text = " \r\n   Time: " + Helper.RenderTime(time); 
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

}
