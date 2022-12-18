using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public class RimCrossPickable : AdditionalHandPickable
{

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
#endregion

#region IAdditionalHandPickableMethods:
	/// <summary>Callback invoked when a Handler is picked.</summary>
	/// <param name="_ID">Handler's ID.</param>
	/// <param name="_hand">Hand that picked the handler.</param>
	/// <param name="_picked">Was the handler picked or dropped?.</param>
	public override void OnHandlerPicked(int _ID, Hand _hand, bool _picked){}
#endregion

	private void OnEnable()
	{
		foreach(Handler handler in handlers)
		{
			handler.onHandlerPicked += OnHandlerPicked;
		}
	}
}
}