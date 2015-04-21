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

	private int BeginTimer = 0;
	private Transform FirstFocusRef;
	public bool isPlayerFoc = false;
	public int CamNum = 0;
	public float FirstCamBufX, FirstCamBufY, FirstCamZoom, FirstCamSmooth;
	public bool FirstCamFollowX, FirstCamFollowY;

	void Start () {
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;


		SetupAll ();
		ResetLevel (true);

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
		canvasRef.GetComponent<S_UISystem>().ResetLevel();

		foreach (GameObject k in TurretsRef) {
			k.GetComponent<S_Turret> ().ResetLevel();
		}

		GameObject[] h = GameObject.FindGameObjectsWithTag("Bullet");
		foreach (GameObject y in h) {
			Destroy(y);
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
		fastcanvasref 	= Instantiate (fastcanvasPrefabRef);
		playerRef 		= (GameObject) Instantiate (PlayerPrefabReference, new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, 0), new Quaternion (0, 0, 0, 1));
		camRef 			= Instantiate (CameraPrefabReference);
		canvasRef 		= Instantiate (CanvasPrefabReference);


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

		// SETUP
		//fastcanvasref.GetComponent<Canvas> ().worldCamera = camRef.GetComponent<Camera>();

		// SETUP UI REFERENCES
		playerRef.gameObject.GetComponent<S_PlayerMovement> ().UIReference = canvasRef.GetComponent<S_UISystem> ();
		playerRef.gameObject.GetComponent<S_PlayerMovement> ().SetStartPos (gameObject.transform.position);
		GameObject.FindGameObjectWithTag ("EndLevel").GetComponent<S_EndLevel> ().UIScript = canvasRef.GetComponent<S_UISystem> ();



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