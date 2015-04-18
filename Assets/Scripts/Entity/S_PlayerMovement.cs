using UnityEngine;
using System.Collections;
//using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;

public class S_PlayerMovement : MonoBehaviour {


	/*-------------CONSTS--------------*/

	[SerializeField] private  float MaximumRunningSpeed 			= 11f;		// Maximum Horizontal Fast allowed when not sprinting
	[SerializeField] private  float RunningForce					= 80f;		// Running/Sprinting Force
	[SerializeField] private  float DecelerationFactor			= 0.95f;	// By how much speed is multiplied when not running

	[SerializeField] private  float JumpingForce					= 1100f;		// Jumping Force
	[SerializeField] private  short MaxJumps						= 1;       	// Maximum In-Air Jumps
	
	[SerializeField] private  short WallGrabTime 					= 1;		// Time in frames needed to grab a wall
	[SerializeField] private  short WallLetGoTime 				= 10;		// Time in frames needed to let go of a wall
	[SerializeField] private  float WallgrabWalljumpMult 			= 1.20f;	// Multiplier to Walljump force when Wallgrabbing
	[SerializeField] private  float WallgrabWallkickMult			= 0.90f;	// Multiplier to Wallkick force when Wallgrabbing
	[SerializeField] private  float WallgrabGlueFactor			= 0.91f;	// Multiplier to vertical speed Wallgrabbing
	
	[SerializeField] private  float BoostForce 					= 2500f;	// Boosting Force
	[SerializeField] private  short BoostSloMoTime 				= 60;		// Maximum Time in frames allowed to choose direction when boosting
	[SerializeField] private  short BoostCooldown 				= 20;		// Boost cooldown
	[SerializeField] private  float BoostedDecelerationFactor 	= 0.95f;	// By how much speed is multiplied when above maximum allowed speed

	[SerializeField] private  float NodeGroundRadius				= 0.001f;	// Radius of detection of Ground nodes
	[SerializeField] private  float NodeElseRadius 				= 0.02f; 	// Radius of detection of other nodes

	[SerializeField] private  float WallkickHorizontalMult		= 1.35f; 	// Multiplier to horizontal walljump force when wallkicking
	[SerializeField] private  float WallkickVerticalMult			= 0.75f;	// Multiplier to vertical walljump force when wallkicking
	[SerializeField] private  float WallkickComebackMult			= 0.40f;	// Multiplier to horizontal force in the opposide direction to wallkick after wallkicking
	[SerializeField] private  short WallkickComebackTime			= 10;		// Length of that multiplier

	[SerializeField] private  bool WalljumpEnabled 				= true;	// Replace Wallkick with Walljump (can't decide which one's better)
	[SerializeField] private  bool JumpWallRegenEnabled	 		= false;	// Allow airjumps to regen on touching walls

	[SerializeField] private  float DashForce						= 400f;		// Dashing Force
	[SerializeField] private  short MaxDashes						= 1;		// Maximum In-Air Dashes
	[SerializeField] private  float DashAirPushForce				= 200f;		// Vertical Force when air-dashing

	[SerializeField] private  float GlideVerticalMult				= 0.8f;		// Vertical Speed Multiplier when gliding
	[SerializeField] private  float GlideHorizontalMult			= 0.93f;	// Horizontal Speed Multiplier when gliding

	[SerializeField] private  float MaximumSprintingSpeed 		= 17f;		// Maximium Horizontal Fast allowed when sprinting		
	[SerializeField] private  short SprintTime 					= 90;		// Time in frames running needed to start sprinting
	[SerializeField] private  float SprintJumpMult		 		= 1.15f;	// Jump height multiplier when jumping

	[SerializeField] private  float WallclimbForce				= 900f;		// Wallclimbing Force
	[SerializeField] private  short MaxWallclimbs					= 1;		// Maximum allowed wallclimbs
	[SerializeField] private  short WallclimbCooldown				= 12;		// Wallclimb cooldown

	[SerializeField] private  float PullupForce					= 750f;		// Pullup Force
	
	[SerializeField] private  LayerMask CollisionMask;  						// Collision Mask
	[SerializeField] private  Rigidbody2D Rigidbody;							// Reference to own rigidbody
	[SerializeField] private  GameObject BoostableBullet;		

	/*-------------VARS-----------------*/

	private bool 	OnGround = false;				// Standing on ground
	private bool 	TouchingWall = false;			// Touching wall
	private short 	WallSide = 0;					// Touching wall from which side
	private bool 	InAir = false;					// In Mid-air
	
	private bool 	GrabbingWall = false;			// Grabbing a wall
	private short 	WallGrabTimer = 0;				// Timer for grabbing a wall
	private short 	WallLetGoTimer = 0;				// Timer for letting go of a wall

	private int 	InputDirection = 0; 			// -1 - Left / 0 - None or Both / 1 - Right
	private int 	FacingDirection = 1; 			// Same as InputDirection, but doesn't change value when nothing or both keys are being pressed (currently unused) 

	private bool 	Boosted = false; 				// Allowed to go over maximum speed
	private bool 	Boosting = false;				// Currently in slomo
	private float 	BoostArrowDirection = 0;		// Direction of the boost direction arrow
	private short 	BoostTimer = 0;					// Timer for keeping time left in slomo
	private int 	BoostCooldownTimer = 0;			// Timer for boost cooldown

	private short 	JumpsLeft = 0;					// Air Jumps left
		
	private bool 	JumpKeylock = false;			// Lock limiting input from JumpKey('Z') to 1 frame
	private bool 	BoostKeylock = false;			// Lock limiting input from BoostKey('X') to 1 frame after boosting
	private bool 	DashKeylock = false;			// Lock limiting input from DashKey('C') to 1 frame	
	private bool 	UpKeylock = false;				// Lock limiting input from UpKey(up arrow) to 1 frame after wallgrabbing

	private short 	WallkickSide = 0;				// Keeps in memory which side user has wallkicked from 
	private short 	WallkickComebackTimer = 0;		// Timer for duration of slower movement to the side the user has wallkicked from

	private short 	DashesLeft = 0;					// Dashes left
	private int		CurrentMovementDirection = 1;	// Direction the user is currently moving in				
	private short	CurrentMovementLength = 0;		// Length of time in frames user has been moving in a direction
	private bool	Sprinting = false;				// Sprinting or not

	private bool 	GlideKeylockOverride = false;	// Overrides JumpKeyLock when gliding
	public bool 	GlideAllowed = false;

	private short 	WallclimbsLeft = 0;				// Wallclimbs Left
	private short 	WallclimbCooldownTimer = 0;		// Timer for wallclimb cooldown

	private bool 	LeftPullable = false;			// Possible to grab the ledge from left side
	private bool 	RightPullable = false;			// Possible to grab the ledge from right side

	public Vector3 CheckpointReference;
	private bool 	Dying = false;

	private ParticleSystem ParticleSys;

	public S_UISystem UIReference;
	public bool LevelStart = false;

	public List<byte> InputList ;
	public bool GetInput = true;
	public bool ReplayMode = false;
	public int FrameNum = 0;
	public bool Running = false;


	private bool Boostable = false;
	private Vector3 StartPos;

	/*------------INPUT----------------*/

	private bool 	K_Jump = false;					// Z key
	private bool 	K_Boost = false;				// X Key
	private bool	K_Dash = false;					// C Key
	private bool 	K_Left = false;					// Left Arrow
	private bool 	K_Right = false;				// Right Arrow
	private bool	K_Up = false;					// Up Arrow
	private bool	K_Down = false;					// Down Arrow
	
	/*------------OBJECTS--------------*/

	private SpriteRenderer ArrowRenderer;
	private Transform ArrowTransform;
	public GameObject ArrowReference;
	
	private Transform N_Ground; // Everything with 'N_' is for collision checking
	private Transform N_GroundL;
	private Transform N_GroundR;
	//private Transform N_Ceiling; // Currently Unused
	private Transform N_Left;
	private Transform N_Right; 
	private Transform N_LeftGrab;
	private Transform N_RightGrab; 
	private Transform N_LeftGrabNoBlock;
	private Transform N_RightGrabNoBlock; 

	void Start () {
		// Finding all objects
		N_Ground = transform.Find ("N_Ground"); 
		N_GroundL = transform.Find ("N_GroundL");
		N_GroundR = transform.Find ("N_GroundR");
		//N_Ceiling = transform.Find ("N_Ceiling");
		N_Left = transform.Find ("N_Left");
		N_Right = transform.Find ("N_Right");
		N_LeftGrab = transform.Find ("N_LeftGrab");
		N_RightGrab = transform.Find ("N_RightGrab"); 
		N_LeftGrabNoBlock = transform.Find ("N_LeftGrabNoBlock");
		N_RightGrabNoBlock = transform.Find ("N_RightGrabNoBlock"); 
		//ArrowReference = GameObject.FindGameObjectWithTag("Arrow");
		ArrowTransform = ArrowReference.GetComponent<Transform>();
		ArrowRenderer = ArrowReference.GetComponent<SpriteRenderer>();
		Rigidbody = GetComponent<Rigidbody2D>();
		ParticleSys = GetComponent<ParticleSystem> ();
		ParticleSys.enableEmission = false;


		//UIReference = GameObject.Find ("UI_Canvas").GetComponent<S_UISystem>();
	}

	void FixedUpdate()
	{

			// Update Collision, returns true if in air (not touching ground, not touching walls) 
			InAir = collision (); 
		if (Running) {
			// Get Input
			fetchInput ();

			// Update Direction
			InputDirection = K_Left ? K_Right ? 0 : -1 : K_Right ? 1 : 0;					
			FacingDirection = (InputDirection == 0) ? FacingDirection : InputDirection;		

			// Execute everything related to boosting 
			boosting ();
	
			// Execute everything related to jumping
			jumping ();

			// Execute everything related to dashing
			dashing ();



			// You know the drill
			if (GlideAllowed)
			{
				gliding ();
			}
			// Execute everything related to wallgrabbing (or wallsticking, whatever you wanna call it)
			wallgrabbing ();

			pullingup ();
			if (!WalljumpEnabled) {
				wallclimbing ();
								

			}

			// Move the Player
			movement (); 



			// Lock for Dash Key
			DashKeylock = K_Dash;
		}
	}




	bool collision ()
	{
		// Starts off with all important variables false
		OnGround = false;
		TouchingWall = false;
		WallSide = 0;
		RightPullable = false;
		LeftPullable = false;

		// also pls pm me if you have any better solution for this, this looks tragic

		// There are three transforms on the ground below the player, one in the middle, two on the far sides, all check for collision, if they find one, they set ground to true
		Collider2D[] collider = Physics2D.OverlapCircleAll(N_Ground.position, NodeGroundRadius, CollisionMask); 
		for (int i = 0; i < collider.Length; i++)
		{
			if (collider[i].gameObject != gameObject)
				OnGround = true;
		};
		collider = Physics2D.OverlapCircleAll(N_GroundL.position, NodeGroundRadius, CollisionMask);
		for (int i = 0; i < collider.Length; i++)
		{
			if (collider[i].gameObject != gameObject)
				OnGround = true;
		};
		collider = Physics2D.OverlapCircleAll(N_GroundR.position, NodeGroundRadius, CollisionMask);
		for (int i = 0; i < collider.Length; i++) {
			if (collider [i].gameObject != gameObject)
				OnGround = true;
		};


		// Same with two transforms on the sides, which set up TouchingWall and WallSide 
		collider = Physics2D.OverlapCircleAll(N_Left.position, NodeElseRadius, CollisionMask);
		for (int i = 0; i < collider.Length; i++)
		{
			if (collider[i].gameObject != gameObject)
			{	
				TouchingWall = true;
				WallSide = -1;
			}
		}
		collider = Physics2D.OverlapCircleAll(N_Right.position, NodeElseRadius, CollisionMask);
		for (int i = 0; i < collider.Length; i++)
		{
			if (collider[i].gameObject != gameObject)
			{	
				TouchingWall = true;
				WallSide = 1;
			}
		}


		//Getting Grab collisions
		collider = Physics2D.OverlapCircleAll(N_RightGrab.position, NodeElseRadius, CollisionMask);
		for (int i = 0; i < collider.Length; i++)
		{
			if (collider[i].gameObject != gameObject)
			{	
				RightPullable = true;
			}
		}
		collider = Physics2D.OverlapCircleAll(N_RightGrabNoBlock.position, NodeElseRadius, CollisionMask);
		for (int i = 0; i < collider.Length; i++)
		{
			if (collider[i].gameObject != gameObject)
			{	
				RightPullable = false;
			}
		}
		collider = Physics2D.OverlapCircleAll(N_LeftGrab.position, NodeElseRadius, CollisionMask);
		for (int i = 0; i < collider.Length; i++)
		{
			if (collider[i].gameObject != gameObject)
			{	
				LeftPullable = true;
			}
		}
		collider = Physics2D.OverlapCircleAll(N_LeftGrabNoBlock.position, NodeElseRadius, CollisionMask);
		for (int i = 0; i < collider.Length; i++)
		{
			if (collider[i].gameObject != gameObject)
			{	
				LeftPullable = false;
			}
		}


		// Returns false if touching something, true if touching nothing
		return !(OnGround || TouchingWall);

	}





	void fetchInput()
	{
		if (!ReplayMode && GetInput) {
			// Simple enough, I guess you could set up InputDirection to accept this 'horizontal input' thingy I saw in the asset pack, but I have no pad to test it out (and it would screw with other stuff)
			K_Left = Input.GetKey (KeyCode.LeftArrow);
		
			K_Right = Input.GetKey (KeyCode.RightArrow);

			K_Up = Input.GetKey (KeyCode.UpArrow);

			K_Down = Input.GetKey (KeyCode.DownArrow);

			// I use GetKey even for keys that are only supposed to accept single frame of input, because GetKeyDown was unresponsive as fuck (even in Update())
			K_Jump = Input.GetKey (KeyCode.Z);

			K_Boost = Input.GetKey (KeyCode.C);

			K_Dash = Input.GetKey (KeyCode.X);

			byte crfr = 0;
			crfr = (K_Left) ? (byte)(crfr + 0x01) : crfr;	// 1 - Left
			crfr = (K_Right) ? (byte)(crfr + 0x02) : crfr;	// 2 - Right
			crfr = (K_Up) ? (byte)(crfr + 0x04) : crfr;	// 4 - Up
			crfr = (K_Down) ? (byte)(crfr + 0x08) : crfr;	// 8 - Down
			crfr = (K_Jump) ? (byte)(crfr + 0x10) : crfr;	// 16 - Jump
			crfr = (K_Boost) ? (byte)(crfr + 0x20) : crfr;	// 32 - Boost
			crfr = (K_Dash) ? (byte)(crfr + 0x40) : crfr;	// 64 - Dash
			InputList.Add (crfr);
		} else {
			K_Left = false;
			K_Right = false;
			K_Up = false;
			K_Down = false;
			K_Jump = false;
			K_Boost = false;
			K_Dash = false;
		}

		if (ReplayMode) {
			if (FrameNum > InputList.Count)
			{
				Debug.LogError("FATAL DESYNC");
			}
			else
			{
			K_Left = ((InputList[FrameNum] & 1) > 0);
			K_Right = ((InputList[FrameNum] & 2) > 0);
			K_Up = ((InputList[FrameNum] & 4) > 0);
			K_Down = ((InputList[FrameNum] & 8) > 0);
			K_Jump = ((InputList[FrameNum] & 16) > 0);
			K_Boost = ((InputList[FrameNum] & 32) > 0);
			K_Dash = ((InputList[FrameNum] & 64) > 0);
			}

			FrameNum++;
		}
	}





	void boosting()
	{
		Boostable = false;
		Collider2D[] collider = Physics2D.OverlapCircleAll(gameObject.transform.position, 2.5f, -1); 
		for (int i = 0; i < collider.Length; i++)
		{
			if (collider[i].gameObject.tag == "Bullet")
			{
				if (collider[i].gameObject.GetComponentInChildren<S_Boostable>().Boostable == true)
					Boostable = true;
			}
		};
		if (Boostable) {
			// If boost key is pressed and the player is ready for boost
			if (K_Boost && BoostCooldownTimer == 0 && !BoostKeylock) {
				Rigidbody.isKinematic = true; //prevents movement, accepting more elegant solutions
				BoostTimer++; 
				Boosting = true;

				// Arrow handling
				BoostArrowDirection += InputDirection * -5;
				BoostArrowDirection %= 360;
			}


			// Boosting itself
			if ((BoostTimer > 0 && !K_Boost) || BoostTimer == BoostSloMoTime) {
				BoostTimer = 0;
				BoostCooldownTimer = BoostCooldown;
				Boosting = false;
				BoostKeylock = true;
				Rigidbody.isKinematic = false; 
				Rigidbody.AddForce (new Vector2 (Mathf.Cos (Mathf.Deg2Rad * BoostArrowDirection) * BoostForce, Mathf.Sin (Mathf.Deg2Rad * BoostArrowDirection) * BoostForce));
				Boosted = true;
			}
		}

		// Cooldown
		if (BoostCooldownTimer > 0) {
			BoostCooldownTimer--;
		}

		// Update Boost Arrow 
		ArrowRenderer.enabled = Boosting;
		ArrowTransform.rotation = Quaternion.AngleAxis (BoostArrowDirection, Vector3.forward);

		// Lock for Boost key
		Boostable = false;
		BoostKeylock = (K_Boost) ? BoostKeylock : false;
	}





	void jumping()
	{
		float v = (Sprinting) ? SprintJumpMult : 1.0f;

		// Mid-Air Jump
		if (!OnGround && K_Jump && !JumpKeylock && (JumpsLeft>0) && !TouchingWall) 
		{
			
			Rigidbody.velocity = new Vector2(Rigidbody.velocity.x ,0); // Erasing vertical velocity, may look strange but try running the code without it
			Rigidbody.AddForce(new Vector2(0f, JumpingForce*v ));
			JumpsLeft--;

			if (JumpsLeft == 0) {
				JumpKeylock = true;
			}
		}


		// Normal Jump
		if (OnGround && K_Jump && !JumpKeylock) 
		{
			OnGround = false;

			Rigidbody.AddForce(new Vector2(0f, JumpingForce*v));
			TouchingWall = false;
		}


		// Walljump/Wallkick
		if (TouchingWall && K_Jump && !JumpKeylock) 
		{
			if (WalljumpEnabled)
			{
				float r = (GrabbingWall) ? WallgrabWalljumpMult : 1; // Multiplier to walljump power if wallgrabbing
				Rigidbody.velocity = new Vector2(Rigidbody.velocity.x,0);
				Rigidbody.AddForce (new Vector2(JumpingForce*0.66f*WallSide*-1*r,JumpingForce*r));
			}
			else
			{
				float r = (GrabbingWall) ? WallgrabWallkickMult : 1; // Multiplier to walljump power if wallgrabbing
				Rigidbody.velocity = new Vector2(Rigidbody.velocity.x,Rigidbody.velocity.y/5);
				Rigidbody.AddForce (new Vector2(JumpingForce*0.66f*WallSide*-1*r*WallkickHorizontalMult,JumpingForce*r*WallkickVerticalMult));
				WallkickComebackTimer = WallkickComebackTime;
				WallkickSide = WallSide;
				JumpKeylock = true;
			}
		}


		// Refill for Mid-Air Jumps
		if (JumpWallRegenEnabled) {
			JumpsLeft = (!InAir) ? MaxJumps : JumpsLeft;
		} else {
			JumpsLeft = (OnGround) ? MaxJumps : JumpsLeft;
		}

		if (JumpKeylock == false && JumpsLeft == 0 && !TouchingWall) {
			JumpKeylock = false;
		} else {
			JumpKeylock = K_Jump;
		}
	


		// Cooldown for Wallkick effect
		if (WallkickComebackTimer > 0) {
			WallkickComebackTimer--;
		} else {
			WallkickSide = 0;
		};


		// Lock for Jump Key


	}





	void dashing() // also sprinting
	{
		// Dash
		if (K_Dash && !DashKeylock && (DashesLeft > 0 || K_Down)) 
		{

			// Normal Dash
			if (!K_Down) 
			{
				DashesLeft--;
				Sprinting = true;
				Rigidbody.velocity = new Vector2 (MaximumSprintingSpeed*FacingDirection,0);
			}

			// Downdash
			else
			{
				Sprinting = false;
				Rigidbody.velocity = new Vector2(0,-MaximumSprintingSpeed);
			}

			// Additional force for bigger effect
			if (InAir)
			{
				if (!K_Down)
					Rigidbody.AddForce(new Vector2(FacingDirection*DashForce,DashAirPushForce));
				else
					Rigidbody.AddForce (new Vector2(0,-DashForce));
			}
		}


		// Dash regen
		DashesLeft = (OnGround) ? MaxDashes : DashesLeft;


		// Accelerating to Sprinting
		if (CurrentMovementDirection == InputDirection && CurrentMovementDirection != 0 && !TouchingWall) {
			CurrentMovementLength++;
		} else {
			CurrentMovementDirection = InputDirection;
			CurrentMovementLength = 0;
			Sprinting = false;
		}

		// Prevention from accelerating in midair
		if (InAir) {
			CurrentMovementLength--;
		}

		// ENGAGE THE FAST 
		if (CurrentMovementLength == SprintTime) {
			Sprinting = true;
		}
	}






	void wallgrabbing()
	{
		// If touching the wall and moving in its direction, start grabbing
		if (TouchingWall && !OnGround && (InputDirection == WallSide) && !GrabbingWall) 
		{
			WallGrabTimer++;
			if (WallGrabTimer >= WallGrabTime)
			{
				GrabbingWall = true;
				WallGrabTimer = 0;
			}
		}
		else
		{
			WallGrabTimer = 0;
		}

		// Stop grabbing wall if on the ground or jumping
		GrabbingWall = (OnGround || K_Jump) ? false : GrabbingWall;

		// If moving to the opposite direction to the grabbed wall, start letting go
		if (InputDirection != WallSide && GrabbingWall) {
			WallLetGoTimer++;
			if (WallLetGoTimer >= WallLetGoTime) {
				GrabbingWall = false;
				WallLetGoTimer = 0;
			}
		} 
		else 
		{
			WallLetGoTimer = 0;
		}

		// For 'sticky' effect
		if (GrabbingWall && Rigidbody.velocity.y < 0) {
			Rigidbody.velocity = new Vector2(Rigidbody.velocity.x,Rigidbody.velocity.y*WallgrabGlueFactor);
		}
	}



	void gliding()
	{
		// Simple enough, fall and move slower
		if (K_Jump && JumpsLeft == 0 && ( !JumpKeylock || GlideKeylockOverride ) && !TouchingWall) {
			Rigidbody.velocity = new Vector2(Rigidbody.velocity.x * GlideHorizontalMult, Rigidbody.velocity.y * GlideVerticalMult);
			GlideKeylockOverride = true;
		}

		// Override for JumpKeyLock
		GlideKeylockOverride = (K_Jump) ? GlideKeylockOverride : false;
	}




	void wallclimbing ()
	{
		if (K_Up && TouchingWall && WallclimbsLeft > 0 && WallclimbCooldownTimer == 0 && !UpKeylock) {
			Rigidbody.velocity = new Vector2(Rigidbody.velocity.x,0);
			Rigidbody.AddForce (new Vector2 (0, WallclimbForce));
			WallclimbsLeft--;
			WallclimbCooldownTimer = WallclimbCooldown;
			UpKeylock = true;
		}

		if (WallclimbCooldownTimer > 0) {
			WallclimbCooldownTimer--;
		}

		UpKeylock = (K_Up) ? UpKeylock : false;

		WallclimbsLeft = (OnGround) ? MaxWallclimbs : WallclimbsLeft;
	}






	void pullingup ()
	{

		// Suitable for pulling up?
		bool mv = ((LeftPullable && K_Left) || (RightPullable && K_Right));


		// Grab
		if (Rigidbody.velocity.y <= 0 && mv) 
		{

			Rigidbody.isKinematic = true;

			// And pull
			if (K_Up || K_Jump) {
				Rigidbody.isKinematic = false;
				Rigidbody.AddForce (new Vector2 (0, PullupForce)); 
			}
		} else if (!Boosting) {
			Rigidbody.isKinematic = false;
		}

	}






	void movement()
	{
		// If not grabbing wall, move
		if (!GrabbingWall) 
		{
			float r = (WallkickSide == InputDirection) ? WallkickComebackMult : (WallSide == InputDirection) ? 0.0f : 1.0f; 
			Rigidbody.AddForce (new Vector2 (InputDirection * RunningForce * r, 0));
		}; 
		
		float v = Rigidbody.velocity.x;

		// Limit speed if either no input from keys or moving faster than allowed
		if (InputDirection != 0)
		{

			if (!Boosted ) {

				float mx = (Sprinting) ? MaximumSprintingSpeed : MaximumRunningSpeed;

				v = (v > 0) ? (v > mx) ? mx : v : (v < -mx) ? -mx : v;
			} else {
				Boosted = (v > 0) ? (v > MaximumRunningSpeed) : (v < -MaximumRunningSpeed);
				v *= BoostedDecelerationFactor;
			}
		}
		else
		{
			v *= DecelerationFactor;
		}

		// Apply reduced speed
		Rigidbody.velocity = new Vector2 (v, (OnGround) ? 0 : Rigidbody.velocity.y );
	}

	public void death()
	{
		Debug.Log ("Ded");
		Running = false;
		ParticleSys.Emit (1);
		GameObject[] alltur = GameObject.FindGameObjectsWithTag ("Turret");
		foreach (GameObject k in alltur) {
			k.GetComponent<S_Turret> ().turretActive = false;
		}
		Dying = true;
		StartCoroutine (deathpriv ());
	}

	private IEnumerator deathpriv()
	{

		Rigidbody.isKinematic = true;
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		yield return new WaitForSeconds(0.30f);
		ParticleSys.Clear ();
		undeath ();
	}

	private void undeath()
	{
		if (Dying) {
			UIReference.deaths++;
			if (LevelStart)
			{
				GameObject.FindGameObjectWithTag ("StartLevel").GetComponent<S_LevelStart>().ResetLevel(false);
			}
			GameObject[] alltur = GameObject.FindGameObjectsWithTag ("Turret");
			foreach (GameObject k in alltur) {
				k.GetComponent<S_Turret> ().turretActive = true;
			}
			Rigidbody.isKinematic = false;
			gameObject.transform.position = new Vector3(CheckpointReference.x,CheckpointReference.y,CheckpointReference.z);
			gameObject.GetComponent<SpriteRenderer> ().enabled = true;
			Dying = false;
			Running = true;
		}
	}

	public void ResetLevel(ref List<byte> inp)
	{
		InputList = new List<byte> (inp);
		ReplayMode = true;
		FrameNum = 0;
		Running = false;
		LevelStart = true;
		CheckpointReference = StartPos;
		gameObject.transform.position = StartPos;
	}

	public void ResetLevel()
	{
		InputList = new List<byte> ();
		ReplayMode = false;
		Running = false;
		LevelStart = true;
		CheckpointReference = StartPos;
		GetInput = true;
		gameObject.transform.position = StartPos;
	}

	public void SetStartPos(Vector3 pos)
	{
		StartPos = pos;
	}





}
