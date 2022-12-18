using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public enum Gender
{
	Male, 												/// <summary>Male Gender.</summary>
	Female 												/// <summary>Female Gender.</summary>
}

public enum PickableState
{
	Unpicked, 											/// <summary>Un-picked State.</summary>
	Picked, 											/// <summary>Picked State.</summary>
	Dropped 											/// <summary>Dropped State.</summary>
}

/// <summary>Event invoked when a Handler is picked.</summary>
/// <param name="_ID">Handler's ID.</param>
/// <param name="_hand">Hand that picked the handler.</param>
/// <param name="_picked">Was it Picked?.</param>
public delegate void OnHandlerPicked(int _ID, Hand _hand, bool _picked);

public class Application : Singleton<Application>
{
	[SerializeField] private ApplicationData _data; 	/// <summary>Application's Data.</summary>
	private User _user; 								/// <summary>Application's User.</summary>

	/// <summary>Gets data property.</summary>
	public static ApplicationData data { get { return Instance._data; } }

	/// <summary>Gets user property.</summary>
	public static User user { get { return Instance._user; } }

	/// <summary>Callback internally called on Awake.</summary>
	protected override void OnAwake() { /*...*/ }
}
}