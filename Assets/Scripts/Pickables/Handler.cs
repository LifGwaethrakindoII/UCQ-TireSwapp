using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public class Handler : MonoBehaviour, IPickableHandler
{
	public event OnHandlerPicked onHandlerPicked; 	/// <summary>OnHandlerPicked's subscription delegate.</summary>
	private int _ID; 								/// <summary>Handler's ID.</summary>

	/// <summary>Gets and Sets ID property.</summary>
	public int ID
	{
		get { return _ID; }
		set { _ID = value; }
	}

	public void OnPick(Hand _hand)
	{
		if(onHandlerPicked != null)
		{
			onHandlerPicked(ID, _hand, true);
			AcceptPickRequest(_hand);
		}
	}

	public void OnDrop(Hand _hand)
	{
		if(onHandlerPicked != null)
		{
			_hand.handler = null;
			onHandlerPicked(ID, _hand, false);
		}
	}

	public virtual void AcceptPickRequest(Hand _hand)
	{
		_hand.handler = this;
	}
}
}