using UnityEngine;
using System.Collections;
using RedHelp;

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
	private bool showup = false, goshow = false;
	private float size;
	private TextMesh txt;

	private const int NUM_OF_TIERS = 4;

	private int[] times = new int[NUM_OF_TIERS];
	private string[] names = new string[NUM_OF_TIERS];
	private int[] indexes = new int[NUM_OF_TIERS];
	private int[] ranks = new int[NUM_OF_TIERS];
	private string[] tiernames = new string[NUM_OF_TIERS] {"Normal: ", "Hard:   ", "Expert: ", "Insane: "};
	private Color[] tiercolours = new Color[NUM_OF_TIERS] {new Color(1,1,1), new Color(1, 0.6f, 0.6f), new Color(1, 0.2f, 0.2f), new Color(0.8f, 0, 0)};


	void Start() 
	{
		txt = gameObject.GetComponentInChildren<TextMesh> ();
		gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 8;
		UpdateDisplayData ();
		UpdateText ();
		GameObject.Find("BG").GetComponent<RectTransform>().localScale = new Vector3(12+size ,5.1f,0.0f);
		GameObject.Find("BG").GetComponent<SpriteRenderer>().enabled = false;
		gameObject.GetComponentInChildren<MeshRenderer> ().enabled = false;
	}
	 
	void UpdateText()
	{
		int k = Mathf.Max(NormalLevelName.Length, HardLevelName.Length, ExpertLevelName.Length, InsaneLevelName.Length);
		size = k;
		string output = "";
		output += Helper.ColorString(StageName,230,230,0) + Helper.LineBreak; 
		output += Helper.ColorString("Select Level", 90,90,255) + Helper.LineBreak;
		for(int i = 0; i < NUM_OF_TIERS; i++)
		{
			output += GetDifficultyChunk(i, k);
		}
		txt.text = output;
	}
	
	string GetDifficultyChunk(int tier, int k)
	{
		string o = "";
		if (pick == tier)
		{
			o += Helper.ColorString("[",220,170,0);
		}
		string h = "";
		h += tiernames[tier] + names[tier];
		for(int i = names[tier].Length; i <= k; i++)
		{
			h += " ";
		}
		h += "| " + Helper.RenderTime (times[tier]) + " | ";
		o += Helper.ColorString(h,tiercolours[tier]);
		o += Helper.RenderRank (ranks[tier]);
		if (pick == tier)
		{
			o += Helper.ColorString("]",220,170,0);
		}
		o += "\r\n";
		return o;
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(Input.GetKey(KeyCode.UpArrow) && !uplock)
		{
			if (showup){
			pick = pick-1;
			}
			showup = true;
			pick = (pick<0) ? 3 : (pick > 3) ? 0 : pick;
			UpdateText();
		};

		if(Input.GetKey(KeyCode.DownArrow) && !downlock)
		{
			pick = pick+1;
			pick = (pick<0) ? 3 : (pick > 3) ? 0 : pick;
			UpdateText();
		}

		if (showup && !goshow)
		{
			GameObject.Find("BG").GetComponent<SpriteRenderer>().enabled = true;
			gameObject.GetComponentInChildren<MeshRenderer> ().enabled = true;
			goshow = true;
		}

		if (showup)
			{
			if (Input.GetKey (KeyCode.C))
			{
				switch (pick)
				{
				case 0:
					Application.LoadLevel(NormalLevelIndex);
					break;
				case 1:
					Application.LoadLevel(HardLevelIndex);
					break;
				case 2:
					Application.LoadLevel(ExpertLevelIndex);
					break;
				case 3:
					Application.LoadLevel(InsaneLevelIndex);
					break;
				}
			}
		}

		uplock = Input.GetKey(KeyCode.UpArrow);
		downlock = Input.GetKey(KeyCode.DownArrow);
	}
	

	public void GetTimesRef(ref int[] h)
	{
		try
		{
			NormalLevelTime = h[NormalLevelIndex];
			HardLevelTime = h[HardLevelIndex];
			ExpertLevelTime = h[ExpertLevelIndex];
			InsaneLevelTime = h[InsaneLevelIndex];
		}
		catch(System.IndexOutOfRangeException)
		{
			Debug.LogError ("you dun gooofed again");
		}
		UpdateDisplayData ();
	}

	void UpdateDisplayData()
	{
		times[0] = NormalLevelTime;
		times[1] = HardLevelTime;
		times[2] = ExpertLevelTime;
		times[3] = InsaneLevelTime;
		
		names[0] = NormalLevelName;
		names[1] = HardLevelName;
		names[2] = ExpertLevelName;
		names[3] = InsaneLevelName;
		
		ranks[0] = NormalLevelRank;
		ranks[1] = HardLevelRank;
		ranks[2] = ExpertLevelRank;
		ranks[3] = InsaneLevelRank;
		
		indexes[0] = NormalLevelIndex;
		indexes[1] = HardLevelIndex;
		indexes[2] = ExpertLevelIndex;
		indexes[3] = InsaneLevelIndex;
	}

	void OnTriggerExit2D(Collider2D other)
	{
		goshow = false;
		showup = false;
		GameObject.Find("BG").GetComponent<SpriteRenderer>().enabled = false;
		gameObject.GetComponentInChildren<MeshRenderer> ().enabled = false;
	}
}
