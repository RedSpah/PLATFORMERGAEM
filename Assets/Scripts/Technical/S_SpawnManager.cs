using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



public class S_SpawnManager : MonoBehaviour {

	public GameObject playerprefabref;

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
		}
	}
	
	// Update is called once per frame
	void OnLevelWasLoaded (int level) {
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


}
