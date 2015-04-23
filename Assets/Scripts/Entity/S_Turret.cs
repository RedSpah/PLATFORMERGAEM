using UnityEngine;
using System.Collections;

public class S_Turret : MonoBehaviour {

    /*------------CONSTS------------*/
   
    [SerializeField]
    private int GunInitWait = 20;
    [SerializeField]
    private int GunCooldown = 10;
    [SerializeField]
    private bool BulletBoostable = false;
    [SerializeField]
    private float BulletSpeed = 200;
    [SerializeField]
    private float GunOffset = 0;
    
    [SerializeField]
    private bool ConstLaser = false;
    [SerializeField]
    private bool TurnWhileLaserIsActive = false;
    [SerializeField]
    private int LaserDuration = 5;
    [SerializeField]
    private int LaserInitWait = 20;
    [SerializeField]
    private int LaserCooldown = 10;
    [SerializeField]
    private float LaserOffset = 0;

    [SerializeField]
    private bool AimThroughWalls = false;
    [SerializeField]
    private bool FireAtWalls = false;
    [SerializeField]
    private float TurnSpeed = 0.5f;
    [SerializeField]
    private float RotationSpeed = 0f;
    
    [SerializeField]
    private Color AimColorBegin = Color.red, AimColorEnd = Color.red, KillColorBegin = Color.red, KillColorEnd = Color.red;
    [SerializeField]
    private float AimWidthBegin = 0.1f, AimWidthEnd = 0.1f, KillWidthBegin = 0.1f, KillWidthEnd = 0.1f;
    public Material AimMaterial, KillMaterial;

    [SerializeField]
    private bool AimLaserPresent = true;
   
    /*------------MODIFIABLE-CONSTS------------*/
    
    [SerializeField]
    private bool TurretActive = false;      // If anything even works
    [SerializeField]
    private bool GunActive = false;         // If Gun works
    [SerializeField]
    private bool LaserActive = false;       // If Laser works
    [SerializeField]
    private bool Tracking = true;           // If the turret tracks the position of the target

    /*------------VARS------------*/

	private Vector3 InitAngle;
	private int BulletTimer = 0;
	private int LaserTimer = 0;
	private int TimeSeen;
	private int CooldownTimer;
	private bool CanSee;

    /*------------OBJECTS------------*/

	[SerializeField] 
    private LineRenderer Laser;
    [SerializeField]
    private Transform Target;
    [SerializeField]
    private Transform TurretTip;
    [SerializeField]
    private Transform LaserHit;
    [SerializeField]
    private GameObject BulletNoBoost;
    [SerializeField]
    private GameObject BulletBoost;
    [SerializeField]
    private LayerMask CollisionLayer;
    [SerializeField]
    private LayerMask PlayerLayer;
    [SerializeField]
    private LayerMask EndLevel;

	void Start () {
		
        Laser.SetColors(AimColorBegin, AimColorEnd);
        Laser.SetWidth(AimWidthBegin, AimWidthEnd);
        Laser.material = AimMaterial;
        Laser.enabled = true;
        Laser.useWorldSpace = true;

		CollisionLayer += EndLevel;
		PlayerLayer += CollisionLayer;
	}
	

	void FixedUpdate () {
        if (TurretActive)
        {

            // If the target can be seen
            float len = Vector2.Distance(gameObject.transform.position, Target.transform.position);
            RaycastHit2D SeeBeam = Physics2D.Raycast(gameObject.transform.position, Target.transform.position - gameObject.transform.position, len, CollisionLayer);
            CanSee = (SeeBeam.collider == null);

            // Aim beam and targeting
            gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z);
            RaycastHit2D AimBeam = Physics2D.Raycast(TurretTip.position, transform.right, Mathf.Infinity, CollisionLayer);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z);
            if (AimBeam.collider != null)
            {
                LaserHit.position = AimBeam.point;
            }
            else
            {
                LaserHit.position = new Vector2(Mathf.Cos(Mathf.Deg2Rad * gameObject.transform.rotation.eulerAngles.z) * 200 + gameObject.transform.position.x, Mathf.Sin(Mathf.Deg2Rad * gameObject.transform.rotation.eulerAngles.z) * 200 + gameObject.transform.position.y);
            }
            Laser.SetPosition(0, TurretTip.position);
            Laser.SetPosition(1, LaserHit.position);
            Laser.enabled = AimLaserPresent;


            // Turret rotation
            if ((CanSee || AimThroughWalls || !(!TurnWhileLaserIsActive && LaserTimer > LaserCooldown)) && Tracking)
            {
                Vector3 lookDirection = Target.transform.position - gameObject.transform.position;
                float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, TurnSpeed);
            }
            else if (!Tracking)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z + RotationSpeed);
            }

            // Increase subroutine timers
            if (CanSee || FireAtWalls)
            {
                TimeSeen++;
                LaserTimer++;
                BulletTimer++;
            }
            else
            {
                TimeSeen = 0;
                LaserTimer = 0;
                BulletTimer = 0;
            }

            // Gun subroutine
            if (TimeSeen > GunInitWait && BulletTimer > GunCooldown && GunActive)
            {
                GameObject tempBul = (GameObject)Instantiate((BulletBoostable) ? BulletBoost : BulletNoBoost, gameObject.transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z + GunOffset));
                Vector3 vel = tempBul.transform.rotation * Vector3.right;
                tempBul.GetComponent<Rigidbody2D>().AddForce(vel * BulletSpeed);
                BulletTimer = 0;
            }

            // Laser subroutine
            if (TimeSeen > LaserInitWait && LaserTimer > LaserCooldown && LaserActive)
            {
                RaycastHit2D KillBeam;

                Laser.SetColors(KillColorBegin, KillColorEnd);
                Laser.SetWidth(KillWidthBegin, KillWidthEnd);
                Laser.material = KillMaterial;
                Laser.enabled = true;

                if (LaserOffset != 0)
                {
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z + LaserOffset);
                    KillBeam = Physics2D.Raycast(TurretTip.position, transform.right, Mathf.Infinity, PlayerLayer);
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z - LaserOffset);
                }
                else
                {
                    KillBeam = Physics2D.Raycast(TurretTip.position, transform.right, Mathf.Infinity, PlayerLayer);
                }

                if (KillBeam.collider != null && KillBeam.collider.gameObject.tag.Equals("Player"))
                {
                    KillBeam.collider.gameObject.GetComponent<S_PlayerMovement>().death();
                }

                if (LaserTimer >= LaserCooldown + LaserDuration && !ConstLaser)
                {
                    LaserTimer = 0;
                }
            }
            else
            {
                Laser.SetColors(AimColorBegin, AimColorEnd);
                Laser.SetWidth(AimWidthBegin, AimWidthEnd);
                Laser.material = AimMaterial;
                Laser.enabled = AimLaserPresent;
            }

            

        }
        else
        {
            Laser.enabled = false;
        }

		// remember to credit /u/kreaol n /u/Zwejhajfa in credits and shit
	
	}

	public void ResetLevel()
	{
		gameObject.transform.rotation = Quaternion.Euler(InitAngle);
		TimeSeen = 0;
	}

    public void AngleAssure(Vector3 d)
    {
        InitAngle = d;
    }

	public void ResetLevelInit()
	{
        TurretActive = false;
	}

    public void ToggleTurret(bool state)
    {
        TurretActive = state;
    }

    public void ToggleTurretGun(bool state)
    {
        GunActive = state;
    }

    public void ToggleTurretLaser(bool state)
    {
        LaserActive = state;
    }

    public void ToggleTracking(bool state)
    {
        Tracking = state;
    }

    public void SetTarget(Transform f)
    {
        Target = f;
    }

}











