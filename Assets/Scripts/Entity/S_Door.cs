using UnityEngine;
using System.Collections;

public class S_Door : MonoBehaviour {
	public int NormalLevelIndex;
	public int HardLevelIndex;
	public int ExpertLevelIndex;
	public int InsaneLevelIndex;

	public string NormalLevelName;
	public string HardLevelName;
	public string ExpertLevelName;
	public string InsaneLevelName;

	public int NormalLevelTime;
	public int HardLevelTime;
	public int ExpertLevelTime;
	public int InsaneLevelTime;

	public int NormalLevelRank; 
	public int HardLevelRank;
	public int ExpertLevelRank;
	public int InsaneLevelRank;

	public string StageName; 
	private int pick = 0;
	private bool uplock = false, downlock = false;
	private bool showup = false;
	private float size;
	private TextMesh txt;
	void Start() 
	{
		txt = gameObject.GetComponentInChildren<TextMesh> ();
		gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 8;
		txt.text = GetUpText ();
		GameObject.Find("BG").GetComponent<RectTransform>().localScale = new Vector3(12+size ,5.1f,0.0f);
		GameObject.Find("BG").GetComponent<SpriteRenderer>().enabled = false;
		gameObject.GetComponentInChildren<MeshRenderer> ().enabled = false;
	}
	 
	string GetUpText()
	{
		int k = Mathf.Max(NormalLevelName.Length, HardLevelName.Length, ExpertLevelName.Length, InsaneLevelName.Length);
		size = k;
		string output = "";
		output += "<color=#eeaa00ff>" + StageName + "</color> \r\n <color=#8888ffff>Select Level: </color>\r\n";
		//output += "Diff. Lev. | Name | Best Time | Rank \r\n</color>";

		if (pick == 0)
		{
			output += "<color=#eeaa00ff>[</color> ";
		}
		output += "<color=#ffffffff>Normal: " + NormalLevelName;
		for(int i = NormalLevelName.Length; i <= k; i++)
		{
			output += " ";
		}
		output +="| " + RenderTime (NormalLevelTime) + " | </color>" + RenderRank (NormalLevelRank);
		if (pick == 0)
		{
			output += " <color=#eeaa00ff>]</color>";
		}

		output += "\r\n";

		if (pick == 1)
		{
			output += "<color=#eeaa00ff>[</color> ";
		}
		output += "<color=#ffaaaaff>Hard:   " + HardLevelName;
		for(int i = HardLevelName.Length; i <= k; i++)
		{
			output += " ";
		}
		output += "| " + RenderTime (HardLevelTime) + " | </color>" + RenderRank (HardLevelRank) ;
		if (pick == 1)
		{
			output += " <color=#eeaa00ff>]</color>";
		}

		output += "\r\n";

		if (pick == 2)
		{
			output += "<color=#eeaa00ff>[</color> ";
		}
		output += "<color=#ff5555ff>Expert: " + ExpertLevelName;
		for(int i = ExpertLevelName.Length; i <= k; i++)
		{
			output += " ";
		}
		output +="| " + RenderTime (ExpertLevelTime) + " | </color>" + RenderRank (ExpertLevelRank);
		if (pick == 2)
		{
			output += " <color=#eeaa00ff>]</color>";
		}
		output += "\r\n";


		if (pick == 3)
		{
			output += "<color=#eeaa00ff>[</color> ";
		}
		output += "<color=#dd0000ff>Insane: " + InsaneLevelName;
		for(int i = InsaneLevelName.Length; i <= k; i++)
		{
			output += " ";
		}
		output +="| " + RenderTime (InsaneLevelTime) + " | </color>" + RenderRank (InsaneLevelRank) ;
		if (pick == 3)
		{
			output += " <color=#eeaa00ff>]</color>";
		}
		

		output += "\r";
		return output;

	}



	string RenderTime(int time)
	{
		if (time == -1)
		{
			return "99:59.999";
		}
		int m = Mathf.FloorToInt (time / 3600);
		int s = Mathf.FloorToInt (time / 60) % 60;
		float ms = Mathf.FloorToInt((time % 60) * 1000/60); 
		string y = "";
		if (m < 10)
		{
			y += "0";
		}
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


    string RenderRank (int r)
	{
		switch (r)
		{
		case 0:
			return "<color=#555555ff>NONE</color>";
		case 1:
			return "<color=#777777ff>D   </color>";
		case 2:
			return "<color=#999999ff>D+  </color>";
		case 3:
			return "<color=#bbbb00ff>C   </color>";
		case 4:
			return "<color=#eeee00ff>C+  </color>";			
		case 5:
			return "<color=#0088ddff>B   </color>";

		case 6:
			return "<color=#33bbffff>B+  </color>";

		case 7:
			return "<color=#ff5500ff>A-  <color>";

		case 8:
			return "<color=#ff8800ff>A   </color>";

		case 9:
			return "<color=#ffbb00ff>A+  </color>";

		case 10:
			return "<color=#55ff11ff>S-  </color>";

		case 11:
			return "<color=#aaff66ff>S   </color>";
 
		case 12:
			return "<color=#ddffaaff>SS  </color>";

		default:
			return "redspah was drunk while coding again";

		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(Input.GetKey(KeyCode.UpArrow) && !uplock)
		{
			if (showup){
			pick = pick-1;
			}
			showup = true;
		};
		if(Input.GetKey(KeyCode.DownArrow) && !downlock)
		{
			pick = pick+1;
		}
		pick = (pick<0) ? 3 : (pick > 3) ? 0 : pick;
		if (showup)
			{
			GameObject.Find("BG").GetComponent<SpriteRenderer>().enabled = true;
			gameObject.GetComponentInChildren<MeshRenderer> ().enabled = true;
			//if (Input.GetKey (KeyCode.UpArrow)) {
			//	Application.LoadLevel (NormalLevelIndex);
			//}
		}

		uplock = Input.GetKey(KeyCode.UpArrow);
		downlock = Input.GetKey(KeyCode.DownArrow);
	}

	void FixedUpdate()
	{
		txt.text = GetUpText ();
	}

	public void GetTimesRef(ref int[] h)
	{
		try{
		NormalLevelTime = h[NormalLevelIndex];
		HardLevelTime = h[HardLevelIndex];
		ExpertLevelTime = h[ExpertLevelIndex];
		InsaneLevelTime = h[InsaneLevelIndex];
		}
		catch(System.IndexOutOfRangeException)
		{
			Debug.LogError ("you dun gooofed again");
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		showup = false;
		GameObject.Find("BG").GetComponent<SpriteRenderer>().enabled = false;
		gameObject.GetComponentInChildren<MeshRenderer> ().enabled = false;
	}
}
