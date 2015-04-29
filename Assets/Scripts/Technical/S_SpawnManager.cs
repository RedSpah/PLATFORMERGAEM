using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using RedHelp;


public class S_SpawnManager : MonoBehaviour {

	public GameObject playerprefabref;

	[SerializeField]
	const int NoOfLevels = 5;
	int[] leveltime = new int[NoOfLevels];

	struct Level
	{
		public Vector2 position;
		public int index;
		public Level(Vector2 p, int i)
		{
			position = p;
			index = i;
		}
	}


	[SerializeField]
	public LevelTime[] LvTimes = new LevelTime[NoOfLevels];
	Predicate<Level> predicate = FindLevelByIndex;

	private static bool FindLevelByIndex(Level lv)
	{
		return (lv.index == leveltofind);
	}




	public bool initialloadoverride = true;
	static int leveltofind;
	public Vector2 returnposition = new Vector2(0,0);

	List<Level> LevelIndex = new List<Level>();

	void Start () {
		DontDestroyOnLoad (gameObject);
		GameObject[] testforothers = GameObject.FindGameObjectsWithTag("SpawnManager");
		{
			foreach (GameObject d in testforothers)
			{
				if (d != gameObject)
				{
					Destroy(gameObject);
				}
			}
		}

		GameObject[] doorlist = GameObject.FindGameObjectsWithTag("Door");
		foreach (GameObject f in doorlist) {
			LevelIndex.Add(new Level(f.transform.position, f.GetComponent<S_Door>().NormalLevelIndex));
			LevelIndex.Add(new Level(f.transform.position, f.GetComponent<S_Door>().HardLevelIndex));
			LevelIndex.Add(new Level(f.transform.position, f.GetComponent<S_Door>().ExpertLevelIndex));
			LevelIndex.Add(new Level(f.transform.position, f.GetComponent<S_Door>().InsaneLevelIndex));
		}
	}
	

	void OnLevelWasLoaded (int level) {

		GameObject[] allsnap = UnityEngine.Object.FindObjectsOfType<GameObject>() ;
		foreach(GameObject f in allsnap)
		{
			f.transform.position = new Vector3(Helper.HalfRound (f.transform.position.x), Helper.HalfRound (f.transform.position.y), Helper.HalfRound (f.transform.position.z));
		}

		if (level != 0) {
			leveltofind = level;
			returnposition = LevelIndex.Find (predicate).position;
			initialloadoverride = false;
		} else {
			Debug.Log ("this far works");
			if (!initialloadoverride)
			{
				GameObject.FindGameObjectWithTag("StartLevel").GetComponent<S_LevelStart>().HubSpawnOverride( Instantiate(playerprefabref,returnposition, new Quaternion (0,0,0,1)) as GameObject);
				Debug.Log (returnposition);
				initialloadoverride = true;
			}
		
		}
	}


	void Awake()
	{



		for(int i = 0; i < NoOfLevels; i++)
		{
			leveltime[i] = Helper.GetRecordTime(i);
		}
		foreach(GameObject d in GameObject.FindGameObjectsWithTag("Door"))
		{
			d.GetComponent<S_Door>().GetTimesRef(ref leveltime);
		}
	}

}
