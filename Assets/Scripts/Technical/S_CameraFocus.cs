using UnityEngine;
using System.Collections;

public class S_CameraFocus : MonoBehaviour {
	public int CamNum;
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
	}
}