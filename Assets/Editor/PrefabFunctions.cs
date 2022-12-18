using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabFunctions : EditorWindow
{
	protected static GameObject prefab;

	[MenuItem("Prefabs/Connect Selection To Prefab")]
	public static void ConnectToPrefab()
	{
		PrefabFunctions window = EditorWindow.GetWindow(typeof(PrefabFunctions)) as PrefabFunctions;
	}

	private void OnGUI()
	{
		prefab = EditorGUILayout.ObjectField("Prefab: ", prefab, typeof(GameObject), true) as GameObject;
		if(PrefabUtility.GetCorrespondingObjectFromSource(prefab) == null && PrefabUtility.GetPrefabObject(prefab) != null && Selection.gameObjects.Length > 0)
		if(GUILayout.Button("Connect Selection"))
		{
			GameObject[] selection = Selection.gameObjects;
			foreach(GameObject gameObject in selection)
			{
				PrefabUtility.ReplacePrefab(prefab, gameObject);
			}
		}
	}
}
