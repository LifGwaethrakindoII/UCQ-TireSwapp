using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Ulyssess;

namespace UrielChallenge
{
public enum RimCrossStates
{
	OnScrew,
	OffScrew
}

public class RimCrossPickable : AdditionalHandPickable
{
	private const string MESSAGE_SCREWS_HAVE_NO_NUTS = "Cannot put Rimcross on Screw. No Screw has not a nut installed.";
	private const int LIMIT_HANDS = 2;

	[SerializeField] private LayerMask _screwLayer; 		/// <summary>Screw's Layer Mask.</summary>
	[SerializeField] private Handler[] _handlers; 			/// <summary>Rim Cross's Handlers.</summary>
	[SerializeField] private Handler[] _forwardHandlers; 	/// <summary>Forward's Handler.</summary>
	[SerializeField] private Vector3[] _offsetPoints; 		/// <summary>Offset Points.</summary>
	[SerializeField] private float _screwCheckRadius; 		/// <summary>Screw Radius Check.</summary>
	[SerializeField] private float _interpolationDuration; 	/// <summary>Interpolation's Duration.</summary>
	[SerializeField] private float _angleLimit; 			/// <summary>Tolerance angle.</summary>
	private Screw _screw; 									/// <summary>Screw this CrossRim is connected to.</summary>
	private float deltaAngle; 								/// <summary>Delta Angle.</summary>
	private Coroutine coroutine;

	public AudioSource crossSound;

	/// <summary>Gets screwLayer property.</summary>
	public LayerMask screwLayer { get { return _screwLayer; } }

	/// <summary>Gets handlers property.</summary>
	public Handler[] handlers { get { return _handlers; } }

	public Handler[] forwardHandlers { get { return _forwardHandlers; } }

	/// <summary>Gets offsetPoints property.</summary>
	public Vector3[] offsetPoints { get { return _offsetPoints; } }

	/// <summary>Gets screwCheckRadius property.</summary>
	public float screwCheckRadius { get { return _screwCheckRadius; } }

	/// <summary>Gets interpolationDuration property.</summary>
	public float interpolationDuration { get { return _interpolationDuration; } }

	/// <summary>Gets angleLimit property.</summary>
	public float angleLimit { get { return _angleLimit; } }

	public bool onScrew { get { return screw != null; } }

	public Screw screw
	{
		get { return _screw; }
		set
		{
			if(value == null && _screw != null && _screw.rimCross != null) _screw.rimCross = null;
			_screw = value;
			if(_screw != null) _screw.rimCross = this;
		}
	}

	/// <summary>
	/// Callback to draw gizmos that are pickable and always drawn.
	/// </summary>
	void OnDrawGizmos()
	{
		if(offsetPoints != null)
		foreach(Vector3 offsetPoint in offsetPoints)
		{
			Gizmos.DrawWireSphere(transform.TransformPoint(offsetPoint), screwCheckRadius);
		}
	}

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{	
		if(hand != null && additionalHand != null) this.StartCoroutine(RotateScrew(), ref coroutine);
		else enabled = false;
	}

	/// <summary>
	/// This function is called when the object becomes Disabled and active.
	/// </summary>
	void OnDisable()
	{
		if(coroutine != null) this.DispatchCoroutine(ref coroutine);
	}

	void Awake()
	{
		if(handlers != null)
		foreach(Handler handler in handlers)
		{
			handler.onHandlerPicked += OnLateralHandlerPicked;
		}
		if(forwardHandlers != null)
		foreach(Handler forwardHandler in forwardHandlers)
		{
			forwardHandler.onHandlerPicked += OnForwardHandlersPicked;
		}

		hand = null;
		additionalHand = null;
		screw = null;
	}

	void OnDestroy()
	{
		if(handlers != null)
		foreach(Handler handler in handlers)
		{
			handler.onHandlerPicked -= OnLateralHandlerPicked;
		}
		if(forwardHandlers != null)
		foreach(Handler forwardHandler in forwardHandlers)
		{
			forwardHandler.onHandlerPicked -= OnForwardHandlersPicked;
		}
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		//SingleHandUpdate();
	}

#region FiniteStateMachine:
	/// <summary>Enters PickableState State.</summary>
	/// <param name="_state">PickableState State that will be entered.</param>
	public override void EnterState(PickableState _state){}
	
	/// <summary>Leaves PickableState State.</summary>
	/// <param name="_state">PickableState State that will be left.</param>
	public override void ExitState(PickableState _state){}
#endregion

#region IPickableMethods:
	/// <summary>Callback invoked when this Pickable is picked.</summary>
	/// <param name="_hand">Hand that picked this Pickable.</param>
	public override void OnPicked(Hand _hand)
	{
		/// When the RimCross receives a pick request, the Rim must not have a screw. Thus, both Screw and Rim Cross must delete any dependency.
		/*if(!onScrew)*/ AcceptPickRequest(_hand);
		/*else
		{
			if(hand == null) hand = _hand;
			else if(hand != _hand) additionalHand = _hand;
		}*/
	}

	/// <summary>Callback invoked when this Pickable is dropped.</summary>
	/// <param name="_hand">Hand that dropped this Pickable.</param>
	public override void OnDropped(Hand _hand)
	{
		if(hand == _hand) DropFromHand();
		else if(additionalHand == _hand) DropFromAdditionalHand();

		//UserFeedbackUI.Instance.ShowMessage("Drop on Screw Condition met: " + (offsetPoints != null && screw == null && hand == null && additionalHand == null));
		if(offsetPoints != null && screw == null && hand == null && additionalHand == null)
		{
			foreach(Vector3 offsetPoint in offsetPoints)
			{
				Collider[] colliders = Physics.OverlapSphere(transform.TransformPoint(offsetPoint), screwCheckRadius, screwLayer);
				if(colliders != null && colliders.Length > 0)
				{
					foreach(Collider collider in colliders)
					{
						/// Interpolate to screw...
						Screw currentScrew = collider.GetComponent<Screw>();
						//UserFeedbackUI.Instance.ShowMessage("Can stick into Screw: " + (currentScrew != null && currentScrew.CanRotate()));
						if(currentScrew != null && currentScrew.CanConnectRimCross())
						{
							if(onScrew) screw = null;
							screw = currentScrew;
							if(screw.nut != null)
							{	
								rigidbody.isKinematic = true;
								StartCoroutine(InterpolateToScrew(screw));
								return;
							}
						}
					}
					UserFeedbackUI.Instance.ShowMessage(MESSAGE_SCREWS_HAVE_NO_NUTS);
				}
			}
		}
	}

	/// <summary>Default Hand Pick's Request Confirmation Method. Overridable for more particular functionality.</summary>
	/// <param name="_hand">Hand that requested the pick.</param>
	protected override void AcceptPickRequest(Hand _hand)
	{
		//base.AcceptPickRequest(_hand);
		//if(hand != null && hand != _hand) DropFromHand();

		if(hand == null)
		{
			hand = _hand;
			hand.pickable = this;
			hand.SetAnimationID(animationID);
			crossSound.Play();
			if(!onScrew) transform.parent = hand.transform;
		} else if(hand != null)
		{
			if(hand != _hand && additionalHand == null)
			{
				additionalHand = _hand;
				additionalHand.pickable = this;
				additionalHand.SetAnimationID(animationID);

				if(!onScrew) transform.parent = additionalHand.transform;
			}
		} else if(additionalHand != null)
		{
			if(additionalHand != _hand && hand == null)
			{
				hand = _hand;
				hand.pickable = this;
				hand.SetAnimationID(animationID);

				if(!onScrew) transform.parent = hand.transform;
			}
		}

		rigidbody.isKinematic = true;
	}

	/// <summary>Default Hand Drop Execution. Overridable for more particular functionality.</summary>
	protected override void DropFromHand()
	{
		base.DropFromHand();
		if(additionalHand != null)
		{
			Hand newHand = additionalHand; 
			hand = newHand;
			additionalHand = null;	
		} else if(!onScrew)
		{
			transform.parent = null;
			rigidbody.isKinematic = false;
		}
	}

	/// <summary>Hand Drop execution from additional Hand.</summary>
	protected override void DropFromAdditionalHand()
	{
		additionalHand.SetAnimationID();
		additionalHand.pickable = null;
		additionalHand = null;

		if(!onScrew)
		{
			if(hand != null)
			{
				transform.parent = hand.transform;
			} else
			{
				transform.parent = null;
				rigidbody.isKinematic = false;
			}
		}
	}

	private void OnForwardHandlersPicked(int _ID, Hand _hand, bool _picked)
	{
		if(onScrew) screw = null;
		if(_picked) OnPicked(_hand);
		else if(!onScrew && !_picked) OnDropped(_hand);
	}

	private void OnLateralHandlerPicked(int _ID, Hand _hand, bool _picked)
	{
		//UserFeedbackUI.Instance.ShowMessage("Hand: " + _hand + ", Picked: " + _picked + ", Has Screw: " + onScrew);
		
		if(_picked) OnPicked(_hand);
		else OnDropped(_hand);

		if(onScrew) enabled = (hand != null && additionalHand != null);
	}
#endregion

	private void SingleHandUpdate()
	{
		if((hand != null || additionalHand != null) && onScrew)
		{
			Vector3 crossDirection = (transform.position - hand.transform.position).normalized;
			if(Vector3.Angle(crossDirection, hand.transform.forward) <= angleLimit)
			{
				float angularSpeed = hand.GetDevice().angularVelocity.z;
				if(screw.CanRotate(angularSpeed))
				{
					transform.Rotate(new Vector3(0.0f, 0.0f, angularSpeed), Space.Self);
					screw.RotateNut(angularSpeed);
				}
			}
		}
	}

	private void DoubleHandUpdate()
	{
		if(hand != null && additionalHand != null && onScrew)
		{
			
		}
	}

	private void CalculateAngle()
	{ 
		/*Vector3 lateralDirectionA = transform.InverseTransformDirection(handlers[0].position - transform.position);
		Vector3 lateralDirectionB = transform.InverseTransformDirection(handlers[1].position - transform.position);
		Vector3 handDirection = transform.InverseTransformDirection(hand.transform.position - transform.position);
		Vector3 additionalHandDirection = transform.InverseTransformDirection(additionalHand.transform.position - transform.position);
		lateralDirectionA.z = 0.0f;
		lateralDirectionB.z = 0.0f;
		handDirection.z = 0.0f;
		additionalHandDirection.z = 0.0f;
		Transform targetHand = Vector3.Dot(lateralDirectionA, handDirection) > Vector3.Dot(lateralDirectionA, additionalHandDirection) ? hand.transform : additionalHand.transform;
		float handAndLateralAAngle = Vector3.Angle(lateralDirectionA, handDirection);
		float handAndLateralBAngle = Vector3.Angle(lateralDirectionB, handDirection);
		float additionalHandAndLateralAAngle = Vector3.Angle(lateralDirectionA, additionalHandDirection);
		float additionalHandAndLateralBAngle = Vector3.Angle(lateralDirectionB, additionalHandDirection);*/
	}

	private IEnumerator InterpolateToScrew(Screw _screw)
	{
		Vector3 originalPosition = transform.position;
		Vector3 screwMaxRotation = new Vector3
		(
			_screw.transform.rotation.x,
			_screw.transform.rotation.y,
			_screw.maxAngle
		);
		Quaternion originalRotation = transform.rotation;
		float  n = 0.0f;

		while(n < 1.0f)
		{
			transform.rotation = Quaternion.Lerp(originalRotation, /*_screw.transform.rotation*/ _screw.GetLerpedRotation(), n);
			transform.position = Vector3.Lerp(originalPosition, _screw.transform.TransformPoint(-offsetPoints[0]), n);
			n += (Time.deltaTime / interpolationDuration);
			yield return null;
		}

		transform.position = _screw.transform.TransformPoint(-offsetPoints[0]);
		transform.rotation = _screw.GetLerpedRotation();
		transform.parent = _screw.transform;
	}

	private IEnumerator RotateScrew()
	{
		Vector3 direction = (hand.transform.position - transform.position);
		direction.z = 0.0f;
		direction.Normalize();
		Vector3 lateDirection = direction;
		float angularSpeed = Vector3.SignedAngle(direction, lateDirection, transform.forward);

		while(hand != null && additionalHand != null && onScrew)
		{
			lateDirection = (hand.transform.position - transform.position);
			lateDirection.z = 0.0f;
			lateDirection.Normalize();
			angularSpeed = Vector3.SignedAngle(direction, lateDirection, transform.forward);
			if(screw.CanRotate(-angularSpeed))
			{
				transform.Rotate(new Vector3(0.0f, 0.0f, angularSpeed), Space.Self);
				screw.RotateNut(-angularSpeed);
			}
			direction = (hand.transform.position - transform.position);
			direction.z = 0.0f;
			direction.Normalize();
			yield return null;
		}

		enabled = false;
	}
}
}