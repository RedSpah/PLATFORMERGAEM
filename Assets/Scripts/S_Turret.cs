using UnityEngine;
using System.Collections;

public class S_Turret : MonoBehaviour {

	public Transform PlayerRef;
	private bool clear;
	public LayerMask CollisionLayer;
	public float Range = 0; 
	public Transform TurretTip;

	public bool turretActive = false;
	public int InitWait = 20;
	public int Cooldown = 10;
	public bool BulletBoostable = false;
	public float BulletSpeed = 200;

	private int TimeSeen;
	private int CooldownTimer;
	public GameObject BulletNoBoost;
	public GameObject BulletBoost;
	private Vector3 LastSeen;
	private bool CanSee;
	void Start () {
		if (Range == 0) {
			Range = Mathf.Infinity;
		}
		TurretTip = GameObject.Find ("O_BarrelTip").GetComponent<Transform>();
	}
	

	void FixedUpdate () {
		if (turretActive) {
			float len = Vector2.Distance (gameObject.transform.position, PlayerRef.transform.position);

			RaycastHit2D k = Physics2D.Raycast (gameObject.transform.position, PlayerRef.transform.position - gameObject.transform.position, Mathf.Min (Range, len), CollisionLayer, -Mathf.Infinity, Mathf.Infinity);   
			CanSee = (k.collider == null); 

			Debug.DrawRay (gameObject.transform.position, PlayerRef.transform.position - gameObject.transform.position, Color.red, 0);


			if (CanSee) {
				Vector3 lookDirection = PlayerRef.transform.position - gameObject.transform.position;
				float angle = Mathf.Atan2 (lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
				Quaternion targetRotation = Quaternion.AngleAxis (angle, Vector3.forward);
				transform.rotation = Quaternion.Slerp (gameObject.transform.rotation, targetRotation, 1);
				TimeSeen++;
			} else {
				TimeSeen = 0;
			}

			if (TimeSeen > InitWait && (TimeSeen % Cooldown) == 0) {
				GameObject tempBul = (GameObject)Instantiate ((BulletBoostable) ? BulletBoost : BulletNoBoost, gameObject.transform.position, Quaternion.Euler (0, 0, transform.eulerAngles.z));
				Vector3 vel = tempBul.transform.rotation * Vector3.right;
				tempBul.GetComponent<Rigidbody2D> ().AddForce (vel * BulletSpeed);
			}
		}
		// remember to credit /u/kreaol n /u/Zwejhajfa in credits and shit
	
	}

	/*void OnDrawGizmos()
	{
		if (!CanSee) {
			Gizmos.color = Color.blue;
			Gizmos.DrawCube (ColLoc, new Vector3 (0.5f, 0.5f, 0.5f));
		}
		if (CanSee)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y,0), new Vector3 (PlayerRef.transform.position.x, PlayerRef.transform.position.y,0));
		}
	}*/
}











