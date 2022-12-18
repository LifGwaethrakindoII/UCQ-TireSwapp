using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ulyssess;

public class MenuGUI : MonoBehaviour
{
	public void ChangeScene(string _scene)
	{
		AsynchronousSceneLoader.LoadScene(this, _scene);
	}
}
