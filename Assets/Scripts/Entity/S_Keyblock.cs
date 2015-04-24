using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_Keyblock : MonoBehaviour {

	public int KeyLockID;
	private List<GameObject> NextKeyblock = new List<GameObject> ();
	public bool First = false;
	public int UnlockTime = 0; 

	public Sprite break0;
	public Sprite break1;
	public Sprite break2;
	public Sprite break3;
	private int timer = 0;
	private bool unlocking = false;
	public bool done = false;

	void Start()
	{
		if (First) {
			FindNextLock ();
		}
	}

	public void FindNextLock()
	{
		List<GameObject> colsum = new List<GameObject> ();
		Collider2D[] k = Physics2D.OverlapAreaAll (new Vector2 (transform.position.x + 1, transform.position.y+0.1f), new Vector2 (transform.position.x - 1, transform.position.y-0.1f));
		Collider2D[] j = Physics2D.OverlapAreaAll (new Vector2 (transform.position.x-0.1f, transform.position.y+1), new Vector2 (transform.position.x+0.1f , transform.position.y-1));
		foreach (Collider2D u in k) {
			colsum.Add (u.gameObject); 
		}
		foreach (Collider2D u in j) {
			colsum.Add (u.gameObject); 
		}

		if (colsum != null) {
			foreach (GameObject c in colsum) {
				if (c.gameObject.tag.Equals ("Keyblock")) {
					NextKeyblock.Add (c.gameObject);
					done = true;
					if (c.gameObject.GetComponent<S_Keyblock>() == null)
					{
						c.gameObject.AddComponent<S_Keyblock>();
					}


					if (c.gameObject.GetComponent<S_Keyblock> ().done == false)
					{
						c.gameObject.GetComponent<S_Keyblock> ().UnlockTime = UnlockTime;
						c.gameObject.GetComponent<S_Keyblock> ().break0 = break0;
						c.gameObject.GetComponent<S_Keyblock> ().break1 = break1;
						c.gameObject.GetComponent<S_Keyblock> ().break2 = break2;
						c.gameObject.GetComponent<S_Keyblock> ().break3 = break3;

						c.gameObject.GetComponent<S_Keyblock> ().FindNextLock ();

					}


				}
			}
		}
	}

	public void Unlock()
	{
		timer = UnlockTime;
		unlocking = true;
	}

	void UnlockPrivate()
	{
		foreach (GameObject g in NextKeyblock) {
			if (g != null)
			{
				g.GetComponent<S_Keyblock>().Unlock();
			}
		}
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		gameObject.GetComponent<Collider2D> ().enabled = false;
		unlocking = false;
	}

	void FixedUpdate()
	{
		if (unlocking) {

			if (timer == 0)
			{
				UnlockPrivate();
			}
			timer--;

			if (timer < UnlockTime*0.75)
			{
				gameObject.GetComponent<SpriteRenderer>().sprite = break1;
			}
			if (timer < UnlockTime*0.50)
			{
				gameObject.GetComponent<SpriteRenderer>().sprite = break2;
			}
			if (timer < UnlockTime*0.25)
			{
				gameObject.GetComponent<SpriteRenderer>().sprite = break3;
			}
		}

	}

	public void ResetLevel()
	{
		gameObject.GetComponent<SpriteRenderer> ().enabled = true;
		gameObject.GetComponent<Collider2D> ().enabled = true;
		gameObject.GetComponent<SpriteRenderer>().sprite = break0;
		unlocking = false;
		timer = -1;
	}

}
