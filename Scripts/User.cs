using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace UrielChallenge
{
public class User : MonoBehaviour
{
	[SerializeField] private Hand _leftHand; 	/// <summary>User's Left Hand.</summary>
	[SerializeField] private Hand _rightHand; 	/// <summary>User's Right Hand.</summary>
	private Gender _gender; 					/// <summary>User's Gender.</summary>

	/// <summary>Gets leftHand property.</summary>
	public Hand leftHand { get { return _leftHand; } }

	/// <summary>Gets rightHand property.</summary>
	public Hand rightHand { get { return _rightHand; } }

	/// <summary>Gets and Sets gender property.</summary>
	public Gender gender
	{
		get { return _gender; }
		set { _gender = value; }
	}

	/// <returns>String representing this User's Information.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("User's Information: ");
		builder.Append("\n Gender: ");
		builder.Append(gender.ToString());

		return builder.ToString();
	}
}
}