using UnityEngine;
using System.Collections;

public class S_Killzone : MonoBehaviour {


	public bool isstatic = false;

	void Start () {
		if (isstatic) {
			gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		}
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ("Entered Killzone.");
		if (other.gameObject.tag.Equals ("Player")) {
			Debug.Log ("PlayerKonfirmed.");
			other.gameObject.GetComponent<S_PlayerMovement>().death();
		}
	}

}
