using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class S_LevelStart : MonoBehaviour {

	public GameObject PlayerPrefabReference;
	public GameObject CameraPrefabReference;
	public Canvas CanvasPrefabReference;

	public GameObject[] TurretsRef;

	public Transform BannerLevelloc;
	public GameObject startbanner;
	private GameObject bannercontrol;
	public Canvas fastcanvasPrefabRef;

	public Sprite fast3, fast2, fast1, fastfast;

	private Canvas fastcanvasref;
	public bool BeginFinal;
	private int Begintime = 90;
	private GameObject playerRef;
	private GameObject camRef;
	private Canvas canvasRef;
	private bool huboverride = false;

	private int BeginTimer = 0;
	private Transform FirstFocusRef;
	public bool isPlayerFoc = false;
	public int CamNum = 0;
	public float FirstCamBufX, FirstCamBufY, FirstCamZoom, FirstCamSmooth;
	public bool FirstCamFollowX, FirstCamFollowY;
	private bool showdeaths = false;
	void Start () {
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;

		if (Application.loadedLevel != 0 || GameObject.FindGameObjectWithTag ("SpawnManager").GetComponent<S_SpawnManager> ().returnposition == new Vector2 (0, 0)) {
			SetupAll ();
			ResetLevel (true);
		} else if(Application.loadedLevel == 0){
			SetupAll ();
			ResetLevel (false);
		}
	}

	

	public void ResetLevel(bool init, List<byte> inp = null)
	{
		// PREPARE FAST CANVAS
		fastcanvasref.GetComponentInChildren<Image>().enabled = true;

		// RESET PLAYER AND PREPARE FOR REPLAY
        if (inp == null)
            playerRef.gameObject.GetComponent<S_PlayerMovement>().ResetLevel();
        else
		    playerRef.gameObject.GetComponent<S_PlayerMovement> ().ResetLevel(ref inp);

		// RESET CAMERA
		camRef.gameObject.GetComponent<S_Camera> ().ResetCamera (gameObject.transform.position);
		camRef.gameObject.GetComponent<S_Camera> ().NextFocus(FirstFocusRef,FirstCamFollowX,FirstCamFollowY,FirstCamBufX,FirstCamBufY,FirstCamSmooth,FirstCamZoom);

		// RESET UI
		canvasRef.GetComponent<S_UISystem>().ResetLevel(showdeaths);

		foreach (GameObject k in TurretsRef) {
			k.GetComponent<S_Turret> ().ResetLevel();
			//k.GetComponent<S_Turret> ().ResetLevelInit();
		}
		
		GameObject[] h = GameObject.FindGameObjectsWithTag("Bullet");
		foreach (GameObject y in h) {
			Destroy(y);
		}

		GameObject[] i = GameObject.FindGameObjectsWithTag("Keyblock");
		foreach (GameObject y in i) {
			y.GetComponent<S_Keyblock>().ResetLevel();
		}

		GameObject[] j = GameObject.FindGameObjectsWithTag("Key");
		foreach (GameObject y in j) {
			y.GetComponent<S_Key>().ResetLevel();
		}

		// PREPARE TO BEGIN
		if (init) {
			BeginTimer = Begintime;
			fastcanvasref.GetComponentInChildren<Image> ().sprite = fast3;
			foreach (GameObject k in TurretsRef) {
				k.GetComponent<S_Turret> ().ResetLevelInit();
			}
		} else {
			BeginTimer = 0;
			fastcanvasref.GetComponentInChildren<Image>().enabled = false;
		}
	}
	

	void SetupAll()
	{
		// BASIC SETUP
		fastcanvasref = Instantiate (fastcanvasPrefabRef);
		if (!huboverride) {
			Debug.Log("player spawned naturally");
			playerRef = (GameObject)Instantiate (PlayerPrefabReference, new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, 0), new Quaternion (0, 0, 0, 1));
		} else {
			playerRef.GetComponent<S_PlayerMovement>().SetStartPos(playerRef.transform.position);
			Debug.Log (playerRef.transform.position);
		}
		GameObject f = GameObject.FindGameObjectWithTag("MainCamera");
		if (f != null) {
			f.AddComponent<S_Camera> ();
			camRef = f;
		} else {
			camRef = Instantiate (CameraPrefabReference);
		}
		canvasRef = Instantiate (CanvasPrefabReference);


		// SETUP FIRST FOCUS FOR THE CAMERA
		GameObject[] allfoc = GameObject.FindGameObjectsWithTag ("CameraFocus");
		foreach (GameObject k in allfoc) {
			if (k.GetComponent<S_CameraFocus> ().CamNum == CamNum) {
				FirstFocusRef = k.transform;
			}
		}

		if (FirstFocusRef == null && !isPlayerFoc) {
			Debug.LogError ("ERROR: CANNOT FIND A CAMERA FOCUS WITH GIVEN ID.");
		} else if (isPlayerFoc) {
			FirstFocusRef = playerRef.transform;
		}

		GameObject[] allch = GameObject.FindGameObjectsWithTag("Checkpoint");
		if (allch.Length == 0) {
			showdeaths = false;
		} else {
			showdeaths = true;
		}

		// SETUP PLAYER REFERENCE FOR TURRETS
		TurretsRef = GameObject.FindGameObjectsWithTag ("Turret");
		foreach (GameObject k in TurretsRef) {
			k.GetComponent<S_Turret> ().SetTarget(playerRef.transform);
            k.GetComponent<S_Turret>().AngleAssure(k.gameObject.transform.rotation.eulerAngles);
		}


		// SETUP CAMERA TRIGGERS
		GameObject[] alltrig = GameObject.FindGameObjectsWithTag ("CameraTrigger");
		{
			foreach(GameObject y in alltrig)
			{
				y.GetComponent<S_CamTrigger>().CamRef = camRef;
				if (y.GetComponent<S_CamTrigger>().isFocPlayer)
				{
					y.GetComponent<S_CamTrigger>().FocusRef = playerRef;
				}
			}
		}



		// SETUP COLLISION
		GameObject[] collision = GameObject.FindGameObjectsWithTag("CollisionBlock");
		foreach (GameObject s in collision) {
			s.transform.position = new Vector2 (Mathf.Round (s.transform.position.x), Mathf.Round (s.transform.position.y));
			PolygonCollider2D singlecol = s.GetComponent<PolygonCollider2D>();
			s.GetComponent<SpriteRenderer>().enabled = false;
			Vector2[] pts = s.GetComponent<PolygonCollider2D>().points;
			for (int t = 0; t < singlecol.points.Length; t++)
			{
				pts[t] = new Vector2 ( Mathf.Round(singlecol.points[t].x),  Mathf.Round(singlecol.points[t].y));
			}
			s.GetComponent<PolygonCollider2D>().points = pts;
		}

		// SETUP SPIKES
		GameObject[] h1 = GameObject.FindGameObjectsWithTag("SpikeD");
		foreach (GameObject y in h1) {
			y.AddComponent<S_Spike>();
			PolygonCollider2D s = y.GetComponent<PolygonCollider2D>();
			Vector2[] pts = s.points; 
			for (int t = 0; t < s.points.Length; t++)
			{
				pts[t] = new Vector2 ( s.points[t].x,  s.points[t].y - 0.2f);
			}
			y.GetComponent<PolygonCollider2D>().points = pts;
		}
		GameObject[] h2 = GameObject.FindGameObjectsWithTag("SpikeU");
		foreach (GameObject y in h2) {
			y.AddComponent<S_Spike>();
			PolygonCollider2D s = y.GetComponent<PolygonCollider2D>();
			Vector2[] pts = s.points; 
			for (int t = 0; t < s.points.Length; t++)
			{
				pts[t] = new Vector2 ( s.points[t].x,  s.points[t].y + 0.2f);
			}
			y.GetComponent<PolygonCollider2D>().points = pts;
		}
		GameObject[] h3 = GameObject.FindGameObjectsWithTag("SpikeL");
		foreach (GameObject y in h3) {
			y.AddComponent<S_Spike>();
			PolygonCollider2D s = y.GetComponent<PolygonCollider2D>();
			Vector2[] pts = s.points; 
			for (int t = 0; t < s.points.Length; t++)
			{
				pts[t] = new Vector2 ( s.points[t].x - 0.2f,  s.points[t].y );
			}
			y.GetComponent<PolygonCollider2D>().points = pts;
		}
		GameObject[] h4 = GameObject.FindGameObjectsWithTag("SpikeR");
		foreach (GameObject y in h4) {
			y.AddComponent<S_Spike>();
			PolygonCollider2D s = y.GetComponent<PolygonCollider2D>();
			Vector2[] pts = s.points; 
			for (int t = 0; t < s.points.Length; t++)
			{
				pts[t] = new Vector2 ( s.points[t].x + 0.2f,  s.points[t].y );
			}
			y.GetComponent<PolygonCollider2D>().points = pts;
		}



		// SETUP KEYLOCKS
		Sprite d = null;
		GameObject[] i = GameObject.FindGameObjectsWithTag("Keyblock");
		foreach (GameObject y in i) {
			if (y.GetComponent<S_Keyblock> () != null) {
				d = y.GetComponent<S_Keyblock> ().break0;
			}
			//y.GetComponent<BoxCollider2D>().offset = new Vector2 (0.05f, -0.05f);
			y.GetComponent<BoxCollider2D>().size = new Vector2 (0.9f, 1.1f);
		}
		foreach (GameObject y in i) {
			if (y.GetComponent<S_Keyblock> () == null) {
				y.AddComponent<S_Keyblock> ();
				y.GetComponent<S_Keyblock>().break0 = d;
			}
		}

		// SETUP UI REFERENCES
		playerRef.gameObject.GetComponent<S_PlayerMovement> ().UIReference = canvasRef.GetComponent<S_UISystem> ();
		if (!huboverride) {
			playerRef.gameObject.GetComponent<S_PlayerMovement> ().SetStartPos (gameObject.transform.position);
		}
		GameObject.FindGameObjectWithTag ("EndLevel").GetComponent<S_EndLevel> ().UIScript = canvasRef.GetComponent<S_UISystem> ();



	}

	public void HubSpawnOverride(GameObject pla)
	{
		playerRef = pla;
		playerRef.GetComponent<S_PlayerMovement> ().SetStartPos (playerRef.transform.position);
		Debug.Log (playerRef.transform.position);
		huboverride = true;
		SetupAll ();
		ResetLevel (false);
	}


	void FixedUpdate()
	{
		if (BeginTimer == 0) {
			canvasRef.GetComponent<S_UISystem> ().running = true;
			canvasRef.GetComponent<S_UISystem> ().time = 0;
			playerRef.gameObject.GetComponent<S_PlayerMovement> ().Running = true;
			fastcanvasref.GetComponentInChildren<Image>().sprite = fastfast;
			BeginTimer = -1;
			foreach (GameObject k in TurretsRef) {
				k.GetComponent<S_Turret> ().ToggleTurret(true);
			}

		} else if (BeginFinal) {
			BeginTimer--;
		}

		if (BeginTimer == 90) {
			fastcanvasref.GetComponentInChildren<Image>().sprite = fast3;
		}
		if (BeginTimer == 60) {
			fastcanvasref.GetComponentInChildren<Image>().sprite = fast2;
		}
		if (BeginTimer == 30) {
			fastcanvasref.GetComponentInChildren<Image>().sprite = fast1;
		}
		if (BeginTimer == -20) {
			fastcanvasref.GetComponentInChildren<Image>().enabled = false;
		}
	}
}