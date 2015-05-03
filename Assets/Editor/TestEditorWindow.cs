using UnityEngine;
using UnityEditor;
using System.Collections;
using RedHelp;

public class RedAI : EditorWindow {

	string red = "REDAI";
	bool EditEnabled = false;
	float something = 1.69f;
	string outputname;



	[MenuItem ("Window/RedAI")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(RedAI));
	}


	void OnGUI()
	{
		GUILayout.Label(red, EditorStyles.boldLabel);
		outputname = EditorGUILayout.TextField("Output Name", outputname);

		EditEnabled = EditorGUILayout.BeginToggleGroup("Settings", EditEnabled);
		something = EditorGUILayout.FloatField("stuff", something);
		EditorGUILayout.EndToggleGroup ();
	}
}





[CustomPropertyDrawer(typeof(SubTurret))]
public class SubTurretDrawer : PropertyDrawer
{
 public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.PrefixLabel(position, label);
		EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		int i = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		Rect TypeRect = new Rect(position.x, position.y, 30, position.height);
		Rect ProjRect = new Rect(position.x+35, position.y, 100, position.height); 
		Rect OffsRect = new Rect(position.x+140, position.y, 30, position.height);
		Rect VeloRect = new Rect(position.x+175, position.y, 30, position.height);
		Rect InitRect = new Rect(position.x+210, position.y, 30, position.height);
		Rect ContRect = new Rect(position.x+245, position.y, 30, position.height);
		Rect TracRect = new Rect(position.x+280, position.y, 30, position.height);
		Rect TurnRect = new Rect(position.x+315, position.y, 30, position.height);

		EditorGUI.PropertyField (TypeRect, property.FindPropertyRelative ("SubType"), GUIContent.none);
		EditorGUI.PropertyField (ProjRect, property.FindPropertyRelative ("Projectile"), GUIContent.none);
		EditorGUI.PropertyField (OffsRect, property.FindPropertyRelative ("AngularOffset"), GUIContent.none);
		EditorGUI.PropertyField (VeloRect, property.FindPropertyRelative ("ProjectileVelocity"), GUIContent.none);
		EditorGUI.PropertyField (InitRect, property.FindPropertyRelative ("InitDelay"), GUIContent.none);
		EditorGUI.PropertyField (ContRect, property.FindPropertyRelative ("ContDelay"), GUIContent.none);
		EditorGUI.PropertyField (TracRect, property.FindPropertyRelative ("Tracking"), GUIContent.none);
		EditorGUI.PropertyField (TurnRect, property.FindPropertyRelative ("TurnSpeed"), GUIContent.none);

		EditorGUI.indentLevel = i;
		EditorGUI.EndProperty();
	}

}





