using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public static class ApplicationExtensions
{
	/// <summary>Checks if angle originated by the dot product of two vectors is within an angle [degrees] range.</summary>
	/// <param name="a">Vector a.</param>
	/// <param name="b">Vector b.</param>
	/// <param name="degreeTolerance">Angle tolerance in degrees.</param>
	/// <returns>True if the dot product's angle is between the angle tolerance's range. False otherwise.</returns>
	public static bool DotProductBetweenTolerance(Vector3 a, Vector3 b, float degreeTolerance)
	{
		float dot = Vector3.Dot(a, b);
		float angleToDot = Mathf.Cos(degreeTolerance * Mathf.Deg2Rad);

		return (dot >= 0 ? dot >= angleToDot : dot <= angleToDot);
	}

	/// <summary>Changes Finite State Machine's state.</summary>
	/// <param name="_fms">IFiniteStateMachine's implementer.</param>
	/// <param name="_state">New State.</param>
	public static void changeState<T>(this IFiniteStateMachine<T> _fsm, T _state)
	{
		_fsm.previousState = _fsm.state;
		_fsm.state = _state;

		_fsm.ExitState(_fsm.previousState);
		_fsm.EnterState(_fsm.state);
	}
}
}