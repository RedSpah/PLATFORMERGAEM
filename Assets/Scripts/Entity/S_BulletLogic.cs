using UnityEngine;
using System.Collections;

public class S_BulletLogic : MonoBehaviour {

	public LayerMask ColLayer;
	public LayerMask EndLevel;
	public static int framelife = 240;

	private int laif;
	public CircleCollider2D SelfCol;
	void Start() {
		SelfCol = gameObject.GetComponent<CircleCollider2D> ();
		laif = framelife;
	}


	void FixedUpdate () {
		if (SelfCol.IsTouchingLayers (ColLayer) || SelfCol.IsTouchingLayers (EndLevel)) {
			Destroy(gameObject);
		}
		laif--;
		if (laif == 0) {
			Destroy(gameObject);
		}
	}
}
