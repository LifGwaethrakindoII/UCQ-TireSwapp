using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public class BajarJack : Pickable
{
	public GameObject Elevator;
	public bool bajar = false;

	void Update()
	{
		if(hand != null || bajar)
		{
			if(Elevator.transform.localRotation.x >= 0)
			{
				Elevator.transform.localRotation = Quaternion.Euler(Elevator.transform.localEulerAngles.x + 3 * Time.deltaTime , 0f,0f);
			}
			
		}
	}
}
}
