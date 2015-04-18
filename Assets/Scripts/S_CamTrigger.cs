using UnityEngine;
using System.Collections;

public class S_CamTrigger : MonoBehaviour {

	public GameObject CamRef;
	public int CamNum;
	private GameObject FocusRef;

	public float BufX, BufY, Smoothness, Zoom;
	public bool FolX, FolY;
	public bool isFocPlayer;
	public bool playerFound = false;


	// Use this for initialization
	void Start () {
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		CamRef = GameObject.Find ("O_Camera");

		if (!isFocPlayer) {
			GameObject[] allfoc = GameObject.FindGameObjectsWithTag ("CameraFocus");
			foreach (GameObject k in allfoc) {
				if (k.GetComponent<S_CameraFocus> ().CamNum == CamNum) {
					FocusRef = k;
				}
			}
			if (FocusRef == null) {
				Debug.LogError ("ERROR: CANNOT FIND A CAMERA FOCUS WITH GIVEN ID.");
			}
		}

	}

	void FixedUpdate()
	{
		if (!playerFound && isFocPlayer) {
			FocusRef = GameObject.FindWithTag("Player");
			if (FocusRef != null)
			{
				playerFound = true;
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag.Equals ("Player")) {
			CamRef.GetComponent<S_Camera>().NextFocus(FocusRef.transform, FolX, FolY, BufX, BufY, Smoothness, Zoom);
		}
	}

}
