using UnityEngine;
using System.Collections;

public class S_Turret : MonoBehaviour {

	public Transform PlayerRef;
	public Transform TurretTip;
	public Transform LaserHit;

	public GameObject BulletNoBoost;
	public GameObject BulletBoost;

	public LayerMask CollisionLayer;
	public LayerMask PlayerLayer;
	public LayerMask EndLevel;


	public int InitWait = 20;
	public int Cooldown = 10;
	public bool BulletBoostable = false;
	public float BulletSpeed = 200;
	public float RotationSpeed = 0.5f;
	public bool LaserActive = false;
	public bool GunActive = false;
	public bool AimThroughWalls = false;


	private Quaternion InitAngle;


	public bool turretActive = false;
	private int TimeSeen;
	private int CooldownTimer;
	private bool CanSee;
	private LineRenderer Laser;


	void Start () {

		//TurretTip = GameObject.Find ("O_BarrelTip").GetComponent<Transform>();
		InitAngle = gameObject.transform.rotation;
		Laser = gameObject.GetComponentInChildren<LineRenderer> ();
		Laser.SetColors (Color.red, Color.red);
		Laser.SetWidth (0.1f, 0.1f);

		//LaserHit = GameObject.Find("TO_Hit").GetComponent<Transform>();
		Laser.enabled = true;
		Laser.useWorldSpace = true;

		CollisionLayer += EndLevel;
		PlayerLayer += CollisionLayer;
	}
	

	void FixedUpdate () {
		if (turretActive) {
			float len = Vector2.Distance (gameObject.transform.position, PlayerRef.transform.position);

			RaycastHit2D v;
			if (LaserActive)
			{
				v = Physics2D.Raycast (TurretTip.position, transform.right, Mathf.Infinity, PlayerLayer);
			}
			else
			{
				v = Physics2D.Raycast (TurretTip.position, transform.right, Mathf.Infinity, CollisionLayer);
			}

			RaycastHit2D k = Physics2D.Raycast (gameObject.transform.position, PlayerRef.transform.position - gameObject.transform.position, len, CollisionLayer);   
			CanSee = (k.collider == null); 


			if (v.collider != null && v.collider.gameObject.tag.Equals ("Player"))
			{
				v.collider.gameObject.GetComponent<S_PlayerMovement>().death();
			}

			LaserHit.position = v.point;
			Laser.SetPosition(0, TurretTip.position);
			Laser.SetPosition(1, LaserHit.position);


			if (CanSee || AimThroughWalls) {
				Vector3 lookDirection = PlayerRef.transform.position - gameObject.transform.position;
				float angle = Mathf.Atan2 (lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
				Quaternion targetRotation = Quaternion.AngleAxis (angle, Vector3.forward);
				transform.rotation = Quaternion.Slerp (gameObject.transform.rotation, targetRotation, RotationSpeed);
			}

			if (CanSee) 
			{
				TimeSeen++;
			}
			else {
				TimeSeen = 0;
			}

			if (TimeSeen > InitWait && (TimeSeen % Cooldown) == 0 && GunActive) {
				GameObject tempBul = (GameObject)Instantiate ((BulletBoostable) ? BulletBoost : BulletNoBoost, gameObject.transform.position, Quaternion.Euler (0, 0, transform.eulerAngles.z));
				Vector3 vel = tempBul.transform.rotation * Vector3.right;
				tempBul.GetComponent<Rigidbody2D> ().AddForce (vel * BulletSpeed);
			}
		}
		// remember to credit /u/kreaol n /u/Zwejhajfa in credits and shit
	
	}

	public void ResetLevel()
	{
		gameObject.transform.rotation = Quaternion.Slerp (gameObject.transform.rotation, InitAngle, 1);
		TimeSeen = 0;
	}

	public void ResetLevelInit()
	{
		turretActive = false;
	}

	void OnDrawGizmos()
	{
		//Gizmos.color = Color.blue;
		//Gizmos.DrawSphere (LaserHit.position, 0.5f);
	}
}











