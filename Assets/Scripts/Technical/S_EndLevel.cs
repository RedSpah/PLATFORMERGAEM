using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_EndLevel : MonoBehaviour {

	public List<byte> InpList;
	public bool Done = false;
	private bool ALock = true;
	private bool SLock = true;
	private bool DLock = true;
	public S_UISystem UIScript;

	void Start () {
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
	}


	// Update is called once per frame
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals ("Player")) {
			InpList = new List<byte>(other.GetComponent<S_PlayerMovement>().InputList);
			other.GetComponent<S_PlayerMovement>().GetInput = false;
			other.GetComponent<S_PlayerMovement>().ReplayMode = false;
			S_Camera CamScrRef = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<S_Camera>();
			CamScrRef.ResetCamera (gameObject.transform.position);
			UIScript.endlevel = true;
			UIScript.running = false;
			Done = true;
			ALock = Input.GetKey(KeyCode.A);
			SLock = Input.GetKey(KeyCode.S);
			DLock = Input.GetKey(KeyCode.D);
			int time;
			time = UIScript.time;
			if (PlayerPrefs.GetInt("LevelTime" + Application.loadedLevel) == -1 || PlayerPrefs.GetInt("LevelTime" + Application.loadedLevel) > time)
			{
				PlayerPrefs.SetInt("LevelTime" + Application.loadedLevel, time);
			}
		}
	}

	void FixedUpdate()
	{
		UIScript.time++;
		if (Done && Input.GetKey(KeyCode.A) && !ALock) {
			GameObject.FindGameObjectWithTag ("StartLevel").GetComponent<S_LevelStart>().ResetLevel(true, InpList); 
			Done = false;
		}
		if (Done && Input.GetKey (KeyCode.S) && !SLock) {
			Done = false;
			GameObject.FindGameObjectWithTag ("StartLevel").GetComponent<S_LevelStart>().ResetLevel(true);
		}
		if (Done && Input.GetKey (KeyCode.D) && !DLock) {
			Application.LoadLevel(0);
		}
		ALock = Input.GetKey(KeyCode.A);
		SLock = Input.GetKey(KeyCode.S);
	}

}
