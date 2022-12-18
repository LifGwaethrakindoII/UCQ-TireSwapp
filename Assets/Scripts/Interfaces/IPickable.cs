using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public interface IPickable : IFiniteStateMachine<PickableState>
{
	Hand hand { get; set; } 	/// <summary>Hand currently picking this Pickable.</summary>

	/// <summary>Callback invoked when this Pickable is picked.</summary>
	/// <param name="_hand">Hand that picked this Pickable.</param>
	void OnPicked(Hand _hand);

	/// <summary>Callback invoked when this Pickable is dropped.</summary>
	/// <param name="_hand">Hand that dropped this Pickable.</param>
	void OnDropped(Hand _hand);
}
}