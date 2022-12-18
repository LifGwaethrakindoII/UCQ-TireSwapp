using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public interface IAdditionalHandPickable : IPickable
{
	IPickableHandler[] handlers { get; } 	/// <summary>Pickable's Handlers.</summary> 
	Hand additionalHand { get; set; } 		/// <summary>Pickable's Additional Hand.</summary>

	/// <summary>Callback invoked when a Handler is picked.</summary>
	/// <param name="_ID">Handler's ID.</param>
	/// <param name="_hand">Hand that picked the handler.</param>
	/// <param name="_picked">Was the handler picked or dropped?.</param>
	void OnHandlerPicked(int _ID, Hand _hand, bool _picked);
}
}