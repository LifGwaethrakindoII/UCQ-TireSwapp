using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public interface IFiniteStateMachine<T>
{
	T state { get; set; } 			/// <summary>Current State.</summary>
	T previousState { get; set; } 	/// <summary>Previous State.</summary>

	/// <summary>Enters State.</summary>
	/// <param name="_state">State to enter.</param>
	void EnterState(T _state);

	/// <summary>Exits State.</summary>
	/// <param name="_state">State to exit.</param>
	void ExitState(T _state);
}
}