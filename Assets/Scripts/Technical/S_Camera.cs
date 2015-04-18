using UnityEngine;
using System.Collections;

public class S_Camera : MonoBehaviour {

	public Transform focus;
	public Camera cam;

	public float Smoothness = 1.0f;

	public float Zoom = 12.0f;

	public bool FollowX = true;			// If camera will follow  focus in x dimension
	public bool FollowY = true;			// ditto but with y dimension
	public float BufferX = 0.0f;		// how far the focus has to move to start following it
	public float BufferY = 0.0f;		// ditto

	private bool start = false;
	private Vector2 NLoc;

	void FixedUpdate ()
	{
		if (focus != null) { // if there even is a focus
			if (FollowX && Mathf.Abs (transform.position.x - focus.position.x) > BufferX) {
				NLoc.x = (Mathf.Sign (transform.position.x - focus.position.x) * BufferX) + focus.position.x; // if it's outside of buffer range, move in its direction
			}

			//ditto
			if (FollowY && Mathf.Abs (transform.position.y - focus.position.y) > BufferY) {
				NLoc.y = (Mathf.Sign (transform.position.y - focus.position.y) * BufferY) + focus.position.y;
			}

			cam.orthographicSize = Zoom; // set zoom                \/ \/ \/ move camera from its current location to focus' location with given smoothness
			transform.position = new Vector3 ((1.0f - Smoothness) * transform.position.x + Smoothness * NLoc.x, (1.0f - Smoothness) * transform.position.y + Smoothness  * NLoc.y, -5);
		}
	}

	// set up everything
	public void NextFocus(Transform v, bool fh, bool fv, float bh, float bv, float smo, float zom)
	{
		focus = v;
		FollowX = fh;
		FollowY = fv;
		BufferX = bh;
		BufferY = bv;
		Smoothness = smo;
		if (zom != 0) {
			Zoom = zom;
		}
	}

	public void ResetCamera(Vector3 loc)
	{
		gameObject.transform.position = new Vector3 (loc.x, loc.y, loc.z);
		FollowX = false;
		FollowY = false;
		BufferX = 0;
		BufferY = 0;
		Smoothness = 0;
		Zoom = 5;
	}

	void OnPostRender()
	{
		if (!start) {
			GameObject.FindGameObjectWithTag ("StartLevel").GetComponent<S_LevelStart> ().BeginFinal = true;
			start = true;
		}
	}

}















/* CODE THAT iS UNUSED BUT i DON'T WAT TO REMOVE IT IMMEDIATELY
 * //public float fast = 13.0f;
	//public float spdcmp;
 * 	//public float tolerance = 8.0f;
 * //public float camlev = 0.18f;
 * public bool fixedmove = true;
	public bool fixedzoom = true;
 * 	spdcmp = (rigid.velocity.x + rigid.velocity.y) / fast;
				if (spdcmp < 1.0f) {
					spdcmp = 1.0f;
				}
			}
			if (!fixedmove) {
				dist = Mathf.Sqrt (Mathf.Pow (transform.position.y - focus.position.y, 2) + Mathf.Pow (transform.position.x - focus.position.x, 2));
				lev = Mathf.Log (dist, tolerance); 
			}
			if (!fixedzoom) {
				cam.orthographicSize = (prec - camlev) / prec * (Mathf.Sqrt (spdcmp) * zoom) + camlev / prec * cam.orthographicSize;
			} else {
				cam.orthographicSize = zoom;
			}
 * 	public Rigidbody2D rigid;
 * limh = cam.rect.width/2;
			limv = cam.rect.height/2;
			disth = Mathf.Abs(focus.transform.position.x - PlayerLoc.transform.position.x);
			distv = Mathf.Abs (focus.transform.position.y - PlayerLoc.transform.position.y);
			if ((limh < disth) || (limv < distv))
			{
				GameObject.Find ("O_Player").GetComponent<S_PlayerMovement>().death();
			}
 * 
 * private float disth;
	private float distv;

	private float limh;
	private float limv;
 * 
 * 
 * 
 * 
	 */

