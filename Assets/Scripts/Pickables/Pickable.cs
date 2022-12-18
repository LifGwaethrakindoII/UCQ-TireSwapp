using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoidlessUtilities;

namespace UrielChallenge
{
public class Pickable : MonoBehaviour, IPickable
{
	[SerializeField] private int _animationID;
	private Hand _hand; 					/// <summary>Pickable's Hand.</summary>
	private PickableState _state; 			/// <summary>Pickable's Current State.</summary>
	private PickableState _previousState; 	/// <summary>Pickable's Previous State.</summary>
	private Rigidbody _rigidbody; 			/// <summary>Rigidbody's Component.</summary>
	private Collider _collider;
	private Renderer _renderer;

	public int animationID { get { return _animationID; } }

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

	/// <summary>Gets and Sets rigidbody Component.</summary>
	public Rigidbody rigidbody
	{ 
		get
		{
			if(_rigidbody == null)
			{
				_rigidbody = GetComponent<Rigidbody>();
			}
			return _rigidbody;
		}
	}

	/// <summary>Gets and Sets collider Component.</summary>
	public Collider collider
	{ 
		get
		{
			if(_collider == null)
			{
				_collider = GetComponent<Collider>();
			}
			return _collider;
		}
	}

	/// <summary>Gets and Sets renderer Component.</summary>
	public Renderer renderer
	{ 
		get
		{
			if(_renderer == null)
			{
				_renderer = GetComponent<Renderer>();
			}
			return _renderer;
		}
	}

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
	public virtual void OnPicked(Hand _hand)
	{
		AcceptPickRequest(_hand);
	}

	/// <summary>Callback invoked when this Pickable is dropped.</summary>
	/// <param name="_hand">Hand that dropped this Pickable.</param>
	public virtual void OnDropped(Hand _hand)
	{
		DropFromHand();
	}
#endregion

	/// <summary>Default Hand Pick's Request Confirmation Method. Overridable for more particular functionality.</summary>
	/// <param name="_hand">Hand that requested the pick.</param>
	protected virtual void AcceptPickRequest(Hand _hand)
	{
		if(hand != null && hand != _hand) DropFromHand();
		hand = _hand;
		hand.pickable = this;
		hand.SetAnimationID(animationID);
	}

	/// <summary>Default Hand Drop Execution. Overridable for more particular functionality.</summary>
	protected virtual void DropFromHand()
	{
		hand.SetAnimationID();
		hand.pickable = null;
		hand = null;
	}
}
}