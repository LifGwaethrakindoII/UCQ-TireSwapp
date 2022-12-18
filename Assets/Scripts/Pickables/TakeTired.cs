using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoidlessUtilities;

namespace UrielChallenge{

public class TakeTired : Pickable {

	private bool isGround = true;

	public void Update()
	{
		Debug.Log(isGround);
		if(hand != null)
		{
			if(isGround)
			{
				hand = null;
				this.transform.SetParent(null);
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Disco")
		{
			this.transform.parent = other.transform;
		}

		if(other.tag == "Ground")
		{
			isGround = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Ground")
		{
			isGround = false;
		}
	}


	

}
}
