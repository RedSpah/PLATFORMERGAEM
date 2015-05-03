using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using AIDatatypes;

public class AIWindow : EditorWindow {
	[MenuItem ("Window/RedAI")]

	public static void  ShowWindow () {
		EditorWindow.GetWindow(typeof(AIWindow));
	}



	void OnGUI()
	{

	}

}
