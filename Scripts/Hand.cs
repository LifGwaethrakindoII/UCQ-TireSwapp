using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public class Hand : MonoBehaviour
{
	private IPickable _pickable; 	/// <summary>Hand's Pickable.</summary>
	private Rigidbody _rigidbody; 	/// <summary>Rigidbody's Component.</summary>

	/// <summary>Gets and Sets pickable property.</summary>
	public IPickable pickable
	{
		get { return _pickable; }
		set { _pickable = value; }
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
}
}