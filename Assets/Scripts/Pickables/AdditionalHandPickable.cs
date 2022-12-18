using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public abstract class AdditionalHandPickable : Pickable, IAdditionalHandPickable
{
	private Hand _additionalHand; 	/// <summary>Pickable's Additional Hand.</summary>

	/// <summary>Gets and Sets additionalHand property.</summary>
	public Hand additionalHand
	{
		get { return _additionalHand; }
		set { _additionalHand = value; }
	}

#region FiniteStateMachine:
	/// <summary>Enters PickableState State.</summary>
	/// <param name="_state">PickableState State that will be entered.</param>
	public override void EnterState(PickableState _state){}
	
	/// <summary>Leaves PickableState State.</summary>
	/// <param name="_state">PickableState State that will be left.</param>
	public override void ExitState(PickableState _state){}
#endregion

#region IPickableMethods:
	/// <summary>Callback invoked when this Pickable is picked.</summary>
	/// <param name="_hand">Hand that picked this Pickable.</param>
	public override void OnPicked(Hand _hand){}

	/// <summary>Callback invoked when this Pickable is dropped.</summary>
	/// <param name="_hand">Hand that dropped this Pickable.</param>
	public override void OnDropped(Hand _hand){}

	/// <summary>Hand Drop execution from additional Hand.</summary>
	protected virtual void DropFromAdditionalHand() {  }
#endregion
}
}