using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
/// HORRENDOUS PIECE OF CODE.
public class HydraulicJack : Pickable
{
	private const float RANGE_MIN_JACK_ROTATION = -75.0f;
	private const float LIMIT_ANGLE = 180.0f;
	private const float INITIAL_ROTATION = 270.0f;

	public GameObject Parent;
	public GameObject Elevator;
	public GameObject Placa;
	[Range(0f, RANGE_MIN_JACK_ROTATION)] public float JackRotation;
	private float lastFrameJR;
	private float diference;
	private Quaternion placaRotation;

	private Vector3 offsetPivot;
	private bool justHold;
	private float high;
	private bool grip;
	private Vector3 lastHand;
	public float dis;

	private AudioSource jackSound;
	private bool activeSoundJack;
	
	private void Start()
	{
		JackRotation = 0.0f;
		lastFrameJR = 0.0f;
		justHold = false;
		grip = false;
		transform.rotation = Quaternion.Euler(JackRotation + INITIAL_ROTATION, 0f,0f);
		lastFrameJR = transform.localEulerAngles.x;
		placaRotation = Placa.transform.rotation;

		jackSound = GetComponent<AudioSource>();
	}

	private void Update()
	{
		diference = transform.localEulerAngles.x - lastFrameJR;
		Placa.transform.rotation = placaRotation;
		
		if(transform.localEulerAngles.x < LIMIT_ANGLE) transform.localEulerAngles = new Vector3(LIMIT_ANGLE, 0f, 0f);
		else
		{
			if(IsDowning())
			{
				Elevator.transform.localRotation = Quaternion.Euler(Elevator.transform.localEulerAngles.x - (diference / 25) , 0f,0f);
			}
		}

		if(hand != null)
		{
			Vector3 relativePos = Vector3.zero;
	        Quaternion rotation = Quaternion.identity;
			
			if(!justHold)
			{
				high = Parent.transform.position.y;
				offsetPivot = hand.transform.position - Parent.transform.position;
				justHold = true;
				lastHand = hand.gameObject.transform.position;
			}

			if(hand.GetDevice().GetPressDown(SteamVR_Controller.ButtonMask.Grip))
			{
				grip = true;
			}
			if(hand.GetDevice().GetPressUp(SteamVR_Controller.ButtonMask.Grip))
			{
				grip = false;
			}

			if(grip)
			{
				relativePos = hand.gameObject.transform.position - transform.position;
	        	rotation = Quaternion.LookRotation(relativePos);
	        	transform.rotation = rotation;

	        	justHold = false;
			}
			else
			{
				if(Vector3.Distance(lastHand, hand.gameObject.transform.position ) > dis)
				{
					Vector3 direction = lastHand - hand.gameObject.transform.position;
					direction.y = 0;
					relativePos = new Vector3(direction.x - Parent.transform.position.x, 0f, direction.z - Parent.transform.position.z);
					
				}
				lastHand = hand.gameObject.transform.position;

				Parent.transform.position = hand.transform.position - offsetPivot;
				Parent.transform.position = new Vector3(Parent.transform.position.x, high, Parent.transform.position.z); 	
	        }
        	
		}
		else
		{
			justHold = false;
		}
	}



	private bool IsDowning()
	{
		if(lastFrameJR < transform.localEulerAngles.x )
		{
			lastFrameJR = transform.localEulerAngles.x;
			if(activeSoundJack == false)
			{
				activeSoundJack = true;
				jackSound.Play();
			}


			return true;
		}
		else
		{
			lastFrameJR = transform.localEulerAngles.x;
			activeSoundJack = false;
			return false;
		}
	}
}
}