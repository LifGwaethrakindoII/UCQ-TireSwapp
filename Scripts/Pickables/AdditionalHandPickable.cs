using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public abstract class AdditionalHandPickable : Pickable, IAdditionalHandPickable
{
	[SerializeField] private IPickableHandler[] _handlers; 	/// <summary>Rim Cross's Handlers.</summary>
	private Hand _additionalHand; 							/// <summary>Pickable's Additional Hand.</summary>

	/// <summary>Gets and Sets additionalHand property.</summary>
	public Hand additionalHand
	{
		get { return _additionalHand; }
		set { _additionalHand = value; }
	}

	/// <summary>Gets handlers property.</summary>
	public IPickableHandler[] handlers { get { return _handlers; } }

#region FiniteStateMachine:
	/// <summary>Enters PickableState State.</summary>
	/// <param name="_state">PickableState State that will be entered.</param>
	public override abstract void EnterState(PickableState _state);
	
	/// <summary>Leaves PickableState State.</summary>
	/// <param name="_state">PickableState State that will be left.</param>
	public override abstract void ExitState(PickableState _state);
#endregion

#region IPickableMethods:
	/// <summary>Callback invoked when this Pickable is picked.</summary>
	/// <param name="_hand">Hand that picked this Pickable.</param>
	public override abstract void OnPicked(Hand _hand);

	/// <summary>Callback invoked when this Pickable is dropped.</summary>
	/// <param name="_hand">Hand that dropped this Pickable.</param>
	public override abstract void OnDropped(Hand _hand);
#endregion

#region IAdditionalHandPickableMethods:
	/// <summary>Callback invoked when a Handler is picked.</summary>
	/// <param name="_ID">Handler's ID.</param>
	/// <param name="_hand">Hand that picked the handler.</param>
	/// <param name="_picked">Was the handler picked or dropped?.</param>
	public abstract void OnHandlerPicked(int _ID, Hand _hand, bool _picked);
#endregion
}
}