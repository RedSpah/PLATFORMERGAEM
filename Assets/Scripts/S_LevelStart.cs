using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class S_LevelStart : MonoBehaviour {

	public GameObject PlayerPrefabReference;
	public GameObject CameraPrefabReference;
	public Canvas CanvasPrefabReference;

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
	private Transform FocusRef;
	public bool isPlayerFoc = false;
	public int CamNum = 0;
	public float FirstCamBufX, FirstCamBufY, FirstCamZoom, FirstCamSmooth;
	public bool FirstCamFollowX, FirstCamFollowY;

	void Start () {
		fastcanvasref = (Canvas)Instantiate (fastcanvasPrefabRef);

		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		initlevel ();

	}

	private void initlevel()
	{
		fastcanvasref.GetComponentInChildren<Image>().enabled = true;
		playerRef = (GameObject) Instantiate (PlayerPrefabReference, new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, 0), new Quaternion (0, 0, 0, 1));
		playerRef.gameObject.GetComponent<S_PlayerMovement> ().CheckpointReference = transform;
		playerRef.gameObject.GetComponent<S_PlayerMovement> ().LevelStart = true;
		playerRef.gameObject.GetComponent<S_PlayerMovement> ().initlevel();
		playerRef.gameObject.GetComponent<S_PlayerMovement> ().Running = false;

		GameObject[] allfoc = GameObject.FindGameObjectsWithTag ("CameraFocus");
		foreach (GameObject k in allfoc) {
			if (k.GetComponent<S_CameraFocus> ().CamNum == CamNum) {
				FocusRef = k.transform;
			}
		}

		GameObject[] alltur = GameObject.FindGameObjectsWithTag ("Turret");
		foreach (GameObject k in alltur) {
			k.GetComponent<S_Turret> ().PlayerRef = playerRef.transform;
			k.GetComponent<S_Turret> ().turretActive = false;
		}

		if (FocusRef == null && !isPlayerFoc) {
			Debug.LogError ("ERROR: CANNOT FIND A CAMERA FOCUS WITH GIVEN ID.");
		} else if (isPlayerFoc) {
			FocusRef = playerRef.transform;
		}
		
		camRef = Instantiate (CameraPrefabReference);
		camRef.gameObject.GetComponent < S_Camera > ().NextFocus(FocusRef,FirstCamFollowX,FirstCamFollowY,FirstCamBufX,FirstCamBufY,FirstCamSmooth,FirstCamZoom);
		fastcanvasref.GetComponent<Canvas> ().worldCamera = camRef.GetComponent<Camera>();
		GameObject[] alltrig = GameObject.FindGameObjectsWithTag ("CameraTrigger");
		{
			foreach(GameObject y in alltrig)
			{
				y.GetComponent<S_CamTrigger>().CamRef = camRef;
				y.GetComponent<S_CamTrigger>().playerFound = false;
			}
		}
		
		
		if (canvasRef == null) {canvasRef = Instantiate (CanvasPrefabReference);}
		else
		{
			canvasRef.GetComponent<S_UISystem>().resetlevel();
			canvasRef.GetComponent<S_UISystem>().running = false;
		}
		playerRef.gameObject.GetComponent<S_PlayerMovement> ().UIReference = canvasRef.GetComponent<S_UISystem> ();

		GameObject.FindGameObjectWithTag ("EndLevel").GetComponent<S_EndLevel> ().UIScript = canvasRef.GetComponent<S_UISystem> ();
		BeginTimer = Begintime;
		fastcanvasref.GetComponentInChildren<Image>().sprite = fast3;
	}

	private void initlevel(ref List<byte> inp)
	{
		fastcanvasref.GetComponentInChildren<Image>().enabled = true;
		playerRef = (GameObject) Instantiate (PlayerPrefabReference, new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, 0), new Quaternion (0, 0, 0, 1));
		playerRef.gameObject.GetComponent<S_PlayerMovement> ().CheckpointReference = transform;
		playerRef.gameObject.GetComponent<S_PlayerMovement> ().LevelStart = true;
		playerRef.gameObject.GetComponent<S_PlayerMovement> ().initlevel(ref inp);
		playerRef.gameObject.GetComponent<S_PlayerMovement> ().Running = false;
		
		GameObject[] allfoc = GameObject.FindGameObjectsWithTag ("CameraFocus");
		foreach (GameObject k in allfoc) {
			if (k.GetComponent<S_CameraFocus> ().CamNum == CamNum) {
				FocusRef = k.transform;
			}
		}

		GameObject[] alltur = GameObject.FindGameObjectsWithTag ("Turret");
		foreach (GameObject k in alltur) {
			k.GetComponent<S_Turret> ().PlayerRef = playerRef.transform;
			k.GetComponent<S_Turret> ().turretActive = false;
		}

		if (FocusRef == null && !isPlayerFoc) {
			Debug.LogError ("ERROR: CANNOT FIND A CAMERA FOCUS WITH GIVEN ID.");
		} else if (isPlayerFoc) {
			FocusRef = playerRef.transform;
		}
		
		 camRef = Instantiate (CameraPrefabReference);
		camRef.gameObject.GetComponent < S_Camera > ().NextFocus(FocusRef,FirstCamFollowX,FirstCamFollowY,FirstCamBufX,FirstCamBufY,FirstCamSmooth,FirstCamZoom);
		fastcanvasref.GetComponent<Canvas> ().worldCamera = camRef.GetComponent<Camera>();
		GameObject[] alltrig = GameObject.FindGameObjectsWithTag ("CameraTrigger");
		{
			foreach(GameObject y in alltrig)
			{
				y.GetComponent<S_CamTrigger>().CamRef = camRef;
				y.GetComponent<S_CamTrigger>().playerFound = false;
			
			}
		}
		
		
		if (canvasRef == null) {canvasRef = Instantiate (CanvasPrefabReference);}	
		else
		{
			canvasRef.GetComponent<S_UISystem>().resetlevel();
			canvasRef.GetComponent<S_UISystem>().running = false;
			canvasRef.GetComponent<S_UISystem>().setupReplay();
		}
		playerRef.gameObject.GetComponent<S_PlayerMovement> ().UIReference = canvasRef.GetComponent<S_UISystem> ();
		BeginTimer = Begintime;
		fastcanvasref.GetComponentInChildren<Image>().sprite = fast3;
	}

	public void resetlevel (ref List<byte> inp)
	{
		if (camRef != null) {
			Object.Destroy (camRef);
		}
		if (playerRef != null) {
			Object.Destroy (playerRef);
		}
		initlevel (ref inp);
	}

	public void resetlevel ()
	{
		if (camRef != null) {
			Object.Destroy (camRef);
		}
		if (playerRef != null) {
			Object.Destroy (playerRef);
		}
	
		initlevel ();
	}



	void FixedUpdate()
	{
		if (BeginTimer == 0) {
			canvasRef.GetComponent<S_UISystem> ().running = true;
			canvasRef.GetComponent<S_UISystem> ().time = 0;
			playerRef.gameObject.GetComponent<S_PlayerMovement> ().Running = true;
			fastcanvasref.GetComponentInChildren<Image>().sprite = fastfast;
			BeginTimer = -1;
			GameObject[] alltur = GameObject.FindGameObjectsWithTag ("Turret");
			foreach (GameObject k in alltur) {
				k.GetComponent<S_Turret> ().turretActive = true;
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