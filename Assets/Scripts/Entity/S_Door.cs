using UnityEngine;
using System.Collections;

public class S_Door : MonoBehaviour {
	public int NormalLevelIndex;
	public int HardLevelIndex;
	public int ExpertLevelIndex;
	public string NormalLevelName;
	public string HardLevelName;
	public string ExpertLevelName;

	private TextMesh txt;
	void Start()
	{
		txt = gameObject.GetComponentInChildren<TextMesh> ();

		txt.text = "<color=#ffffffff> Normal: </color>" + NormalLevelName + ":      \r\n" + "<color=#ffaaaaff> Hard: </color>" + HardLevelName + ":      \r\n" + "<color=#ff4444ff> Expert: </color>" + ExpertLevelName + ":      ";

		gameObject.GetComponentInChildren<MeshRenderer> ().enabled = false;
	}
	// Use this for initialization
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
