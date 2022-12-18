using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public class Handler : MonoBehaviour, IPickableHandler
{
	public event OnHandlerPicked onHandlerPicked; 	/// <summary>OnHandlerPicked's subscription delegate.</summary>
	private Hand _hand; 							/// <summary>Pickable's Hand.</summary>
	private PickableState _state; 					/// <summary>Pickable's Current State.</summary>
	private PickableState _previousState; 			/// <summary>Pickable's Previous State.</summary>
	private int _ID; 								/// <summary>Handler's ID.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets hand property.</summary>
	public Hand hand
	{
		get { return _hand; }
		set { _hand = value; }
	}

	/// <summary>Gets and Sets state property.</summary>
	public PickableState state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets and Sets previousState property.</summary>
	public PickableState previousState
	{
		get { return _previousState; }
		set { _previousState = value; }
	}

	/// <summary>Gets and Sets ID property.</summary>
	public int ID
	{
		get { return _ID; }
		set { _ID = value; }
	}
#endregion

#region FiniteStateMachine:
	/// <summary>Enters PickableState State.</summary>
	/// <param name="_state">PickableState State that will be entered.</param>
	public virtual void EnterState(PickableState _state){}
	
	/// <summary>Leaves PickableState State.</summary>
	/// <param name="_state">PickableState State that will be left.</param>
	public virtual void ExitState(PickableState _state){}
#endregion

#region IPickableMethods:
	/// <summary>Callback invoked when this Pickable is picked.</summary>
	/// <param name="_hand">Hand that picked this Pickable.</param>
	public virtual void OnPicked(Hand _hand){}

	/// <summary>Callback invoked when this Pickable is dropped.</summary>
	/// <param name="_hand">Hand that dropped this Pickable.</param>
	public virtual void OnDropped(Hand _hand){}
#endregion
}
}