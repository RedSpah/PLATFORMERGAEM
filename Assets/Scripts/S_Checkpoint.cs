using UnityEngine;
using System.Collections;

public class S_Checkpoint : MonoBehaviour {
	

	public bool checkpoint = false; 
	public bool levelstart = false;
	private GameObject UIRef;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag.Equals("Player") && !checkpoint) {
				/*GameObject[] allCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
				foreach(GameObject gm in allCheckpoints)
				{
					gm.GetComponent<S_Checkpoint>().checkpoint = false;
				}
				checkpoint = true; */
				Debug.Log("Checkpoint Set."); 
				other.GetComponent<S_PlayerMovement>().CheckpointReference = gameObject.transform;
				other.GetComponent<S_PlayerMovement>().LevelStart = levelstart;
		}
	}

	void FixedUpdate()
	{


	}

	// Update is called once per frame
	void Update () {
	
	}
}
