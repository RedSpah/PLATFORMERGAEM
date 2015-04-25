using UnityEngine;
using System.Collections;

public class S_Spike : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ("Entered Killzone.");
		if (other.gameObject.tag.Equals ("Player")) {
			Debug.Log ("PlayerKonfirmed.");
			other.gameObject.GetComponent<S_PlayerMovement>().death();
		}
	}
}
