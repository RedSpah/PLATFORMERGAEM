using UnityEngine;
using System.Collections;

public class S_Door : MonoBehaviour {
	public int NormalLevelIndex;
	public int HardLevelIndex;
	public int ExpertLevelIndex;
	public string NormalLevelName;
	public string HardLevelName;
	public string ExpertLevelName;
	public string StageName; 

	private TextMesh txt;
	void Start()
	{
		txt = gameObject.GetComponentInChildren<TextMesh> ();

		txt.text = "<color=#ffffffff> Normal: </color>" + NormalLevelName + ":      \r\n" + "<color=#ffaaaaff> Hard: </color>" + HardLevelName + ":      \r\n" + "<color=#ff4444ff> Expert: </color>" + ExpertLevelName + ":      ";

		gameObject.GetComponentInChildren<MeshRenderer> ().enabled = false;
	}

	string GetUpText(string nn, string nh, string ne, int n, int h, int e, string sn)
	{
		int k = Mathf.Max(nn.Length, nh.Length, ne.Length);
		string output = new string();
		output += "<color=#eeaa00ff>" + sn + "</color> \r\n <color=#8888ffff>Select Level:</color> \r\n";
		output += "<color=#ffffffff> Normal: " + nn;
		for(int i = nn.Length; i <= k; i++)
		{

		}

	}






	void OnTriggerStay2D(Collider2D other)
	{
		gameObject.GetComponentInChildren<MeshRenderer> ().enabled = true;
		if (Input.GetKey (KeyCode.UpArrow)) {
			Application.LoadLevel (NormalLevelIndex);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		gameObject.GetComponentInChildren<MeshRenderer> ().enabled = false;
	}
}
