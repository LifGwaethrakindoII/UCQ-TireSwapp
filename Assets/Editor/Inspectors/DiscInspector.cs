using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UrielChallenge
{
[CustomEditor(typeof(Disc))]
[CanEditMultipleObjects]
public class DiscInspector : Editor
{
	private Disc disc;

	private void OnEnable()
	{
		disc = target as Disc;
	}

	public override void OnInspectorGUI()
    {
    	DrawDefaultInspector();

    	GUILayout.Label("Editor Utilities: ");
    	EditorGUILayout.Space();
    	EditorGUILayout.Space();
    	for(int i = 0; i < disc.screws.Length; i++)
    	{
    		Screw screw = disc.screws[i];

    		if(screw != null)
    		{
    			EditorGUILayout.BeginHorizontal();
    			screw.nut = EditorGUILayout.ObjectField("Screw " + i + " Nut: ", screw.nut, typeof(Nut), true) as Nut;
    			if(screw.nut != null) if(GUILayout.Button("Install Nut")) screw.InstallNut();
    			EditorGUILayout.EndHorizontal();

    			EditorUtility.SetDirty(screw);
    		}
    	}
    	if(disc.tire != null && GUILayout.Button("Install Tire"))
    	{
    		disc.InstallTire();
    		EditorUtility.SetDirty(disc.tire); 
    	}

    	EditorUtility.SetDirty(disc);
    }
}
}