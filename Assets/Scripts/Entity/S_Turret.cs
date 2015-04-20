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

	public bool GunActive = false;
	public int GunInitWait = 20;
	public int GunCooldown = 10;
	public bool BulletBoostable = false;
	public float BulletSpeed = 200;

	public float RotationSpeed = 0.5f;


	public bool LaserActive = false;
	public bool ConstLaser = false;
	public bool TurnWhileLaserIsActive = false;
	public int LaserDuration = 5;
	public int LaserInitWait = 20;
	public int LaserCooldown = 10;




	public bool AimThroughWalls = false;
	public bool FireAtWalls = false;
	public float LaserOffset = 0;

	private Quaternion InitAngle;

	private int BulletTimer = 0;
	private int LaserTimer = 0;

	public bool turretActive = false;
	private int TimeSeen;
	private int CooldownTimer;
	private bool CanSee;
	public LineRenderer AimLaser;
	public LineRenderer KillLaser;


	void Start () {

		//TurretTip = GameObject.Find ("O_BarrelTip").GetComponent<Transform>();
		InitAngle = gameObject.transform.rotation;
		//AimLaser = GameObject.Find ("TO_AimBeamRenderer").GetComponent<LineRenderer> ();
		//KillLaser = GameObject.Find ("TO_KillBeamRenderer").GetComponent<LineRenderer> ();

		AimLaser.SetColors (Color.red, Color.red);
		AimLaser.SetWidth (0.1f, 0.1f);
		AimLaser.enabled = true;
		AimLaser.useWorldSpace = true;

		KillLaser.SetColors (Color.red, Color.red);
		KillLaser.SetWidth (0.1f, 0.1f);
		KillLaser.enabled = false;
		KillLaser.useWorldSpace = true;

		CollisionLayer += EndLevel;
		PlayerLayer += CollisionLayer;
	}
	

	void FixedUpdate () {
		if (turretActive) {

			float len = Vector2.Distance (gameObject.transform.position, PlayerRef.transform.position);
			RaycastHit2D SeeBeam = Physics2D.Raycast (gameObject.transform.position, PlayerRef.transform.position - gameObject.transform.position, len, CollisionLayer);   
			CanSee = (SeeBeam.collider == null); 

			gameObject.transform.rotation =  Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z + LaserOffset);
			RaycastHit2D AimBeam = Physics2D.Raycast (TurretTip.position, transform.right, Mathf.Infinity, CollisionLayer);
			gameObject.transform.rotation =  Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z - LaserOffset);
			LaserHit.position = AimBeam.point;
			AimLaser.SetPosition(0, TurretTip.position);
			AimLaser.SetPosition(1, LaserHit.position);
			KillLaser.SetPosition(0, TurretTip.position);
			KillLaser.SetPosition(1, LaserHit.position);













			if (CanSee || AimThroughWalls || !(!TurnWhileLaserIsActive && LaserTimer > LaserCooldown)) {
				Vector3 lookDirection = PlayerRef.transform.position - gameObject.transform.position;
				float angle = Mathf.Atan2 (lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
				Quaternion targetRotation = Quaternion.AngleAxis (angle, Vector3.forward);
				transform.rotation = Quaternion.Slerp (gameObject.transform.rotation, targetRotation, RotationSpeed);
			}

			if (CanSee || FireAtWalls) 
			{
				TimeSeen++;
				LaserTimer++;
				BulletTimer++;
			}
			else {
				TimeSeen = 0;
				LaserTimer = 0;
				BulletTimer = 0;
			}

			if (TimeSeen > GunInitWait && BulletTimer > GunCooldown && GunActive) {
				GameObject tempBul = (GameObject)Instantiate ((BulletBoostable) ? BulletBoost : BulletNoBoost, gameObject.transform.position, Quaternion.Euler (0, 0, transform.eulerAngles.z));
				Vector3 vel = tempBul.transform.rotation * Vector3.right;
				tempBul.GetComponent<Rigidbody2D> ().AddForce (vel * BulletSpeed);
				BulletTimer = 0;
			}

			if (TimeSeen > LaserInitWait && LaserTimer > LaserCooldown && LaserActive)
			{
				RaycastHit2D KillBeam;
				KillLaser.enabled = true;
				if (LaserOffset != 0)
				{
					gameObject.transform.rotation =  Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z + LaserOffset);
					KillBeam = Physics2D.Raycast (TurretTip.position, transform.right, Mathf.Infinity, PlayerLayer);
					gameObject.transform.rotation =  Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z - LaserOffset);
				}
				else
				{
					KillBeam = Physics2D.Raycast (TurretTip.position, transform.right, Mathf.Infinity, PlayerLayer);
				}

				if (KillBeam.collider != null && KillBeam.collider.gameObject.tag.Equals ("Player"))
				{
					KillBeam.collider.gameObject.GetComponent<S_PlayerMovement>().death();
				}
			}
			else
			{
				KillLaser.enabled = false;
			}

			if (LaserTimer >= LaserCooldown + LaserDuration && !ConstLaser)
			{
				LaserTimer = 0;
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











