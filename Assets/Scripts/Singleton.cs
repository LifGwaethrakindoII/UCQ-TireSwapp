using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	[SerializeField] private bool destroyIfNotInstance; 	/// <summary>Destroy this GameObjects if it is not the Instance?.</summary>
	private static T _Instance; 							/// <summary>Static Instance's Reference.</summary>

	/// <summary>Gets and Sets Instance property.</summary>
	public static T Instance
	{
		get
		{
			if(_Instance == null)
			{
				_Instance = Object.FindObjectOfType<T>();
				if(_Instance == null) Debug.LogError("Instance of type " + typeof(T) + " could not be found on scene.");
			}
			return _Instance;
		}
		protected set { _Instance = value; }
	}

	private void Awake()
	{
		if(Instance != this && destroyIfNotInstance) Destroy(gameObject);
		else OnAwake();
	}

	/// <summary>Callback internally called on Awake.</summary>
	protected virtual void OnAwake() { /*...*/ }
}
}