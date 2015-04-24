using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class S_Key : MonoBehaviour {

	public int KeyID;

	// Use this for initialization
	void OnTriggerEnter2D (Collider2D other) 
	{
		if (other.gameObject.tag.Equals ("Player")) {
			GameObject[] h = GameObject.FindGameObjectsWithTag("Keyblock");
			foreach (GameObject j in h)
			{
				if(j.GetComponent<S_Keyblock>().KeyLockID == KeyID){
					Debug.Log("Found matching keylock");
					j.GetComponent<S_Keyblock>().Unlock();
				}
			}
			gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		}
	}

	public void ResetLevel()
	{
		gameObject.GetComponent<SpriteRenderer> ().enabled = true;
	}
}
