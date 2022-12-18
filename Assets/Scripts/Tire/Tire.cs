using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ulyssess;

namespace UrielChallenge
{
public delegate void OnContactWithFloor(bool _contact);

[RequireComponent(typeof(Rigidbody))]
public class Tire : AdditionalHandPickable
{
	public event OnContactWithFloor onContactWithFloor;

	[SerializeField] private LayerMask _discLayer; 
	[SerializeField] private LayerMask _floorLayer; 	/// <summary>Floor's Layers.</summary>
	[SerializeField] private bool _flat; 			/// <summary>Is the Tire Flat.</summary>
	[SerializeField] private float _checkRadius;
	[SerializeField] private float _interpolationDuration;
	[SerializeField] private float _distanceTolerance;
	private Disc _disc;
	private FixedJoint _fixedJoint;
	[SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
	private bool _onFloor;

	public LayerMask discLayer { get { return _discLayer; } }

	/// <summary>Gets floorLayer property.</summary>
	public LayerMask floorLayer { get { return _floorLayer; } }

	public FixedJoint fixedJoint
	{
		get
		{
			if(_fixedJoint == null)
			{
				_fixedJoint = GetComponent<FixedJoint>();
				if(_fixedJoint == null)
				{
					gameObject.AddComponent<FixedJoint>();
					_fixedJoint = GetComponent<FixedJoint>();
				}
			}
			return _fixedJoint;
		}
	}
	/// <summary>Gets and Sets skinnedMeshRenderer Component.</summary>
	public SkinnedMeshRenderer skinnedMeshRenderer
	{ 
		get
		{
			if(_skinnedMeshRenderer == null)
			{
				_skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
			}
			return _skinnedMeshRenderer;
		}
	}

	/// <summary>Gets and Sets flat property.</summary>
	public bool flat
	{
		get { return _flat; }
		set
		{
			_flat = value;
			EvaluateTire();
		}
	}

	/// <summary>Gets and Sets onFloor property.</summary>
	public bool onFloor
	{
		get { return _onFloor; }
		private set { _onFloor = value; }
	}

	/// <summary>Gets and Sets distanceTolerance property.</summary>
	public float distanceTolerance
	{
		get { return _distanceTolerance; }
		private set { _distanceTolerance = value; }
	}

	public float checkRadius { get { return _checkRadius; } }

	public float interpolationDuration { get { return _interpolationDuration; } }
	
	public bool hasBothHands { get { return hand != null && additionalHand != null; } }

	public Disc disc
	{
		get { return _disc; }
		set
		{
			_disc = value;
			rigidbody.isKinematic = (disc == null);
		}
	}

	/// <summary>
	/// Callback to draw gizmos that are pickable and always drawn.
	/// </summary>
	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, checkRadius);
		Gizmos.DrawWireSphere(transform.position, skinnedMeshRenderer.bounds.extents.y + distanceTolerance);
	}

	private void Awake()
	{
		float summedDistance = (skinnedMeshRenderer.bounds.extents.y + distanceTolerance);
		distanceTolerance = (summedDistance * summedDistance);
		EvaluateTire();
		enabled = hasBothHands;
	}

	private void Update()
	{
		if(hasBothHands)
		{
			float squareDistance = (additionalHand.transform.position - transform.position).sqrMagnitude;
			if(squareDistance <= distanceTolerance) return;
			else
			{
				TotalDropFromHands();
			}
		}
		else TotalDropFromHands();
	}

	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	void OnCollisionEnter(Collision col)
	{
		GameObject obj = col.gameObject;
	
		if(Extensions.IsOnLayerMask(obj.layer, floorLayer))
		{
			//Debug.Log("[Tire] AIIII IT DID!!!");
			onFloor = true; 
			if(onContactWithFloor != null) onContactWithFloor(true);
		}
	}

	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	void OnCollisionExit(Collision col)
	{
		GameObject obj = col.gameObject;
	
		if(Extensions.IsOnLayerMask(obj.layer, floorLayer))
		{
			//Debug.Log("[Tire] AIIII IT LEFT!!!");
			onFloor = false; 
			if(onContactWithFloor != null) onContactWithFloor(false);
		}
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
		if(disc == null || !disc.HasANutInstalled())
		{ /// If the tired is not on a disc, or the tire's disc has not a single nut installed. Let the hand pick this tire.
			AcceptPickRequest(_hand);
			if(disc != null)
			{
				disc.tire = null;
				disc = null;
			}
		} else if(disc != null && !disc.HasANutInstalled() && disc.IsDiscRisenEnough())
		{ /// If the tire is on a disc, the disc has not a single nut installed and the disc is risen enough, let the tire be picked by the hand.
			disc.tire = null;
			disc = null;
			AcceptPickRequest(_hand);
		}
	}

	/// <summary>Callback invoked when this Pickable is dropped.</summary>
	/// <param name="_hand">Hand that dropped this Pickable.</param>
	public override void OnDropped(Hand _hand)
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius, discLayer);

		TotalDropFromHands();

		if(colliders != null && colliders.Length > 0)
		{
			Disc currentDisc = colliders[0].GetComponent<Disc>();

			if(currentDisc != null && currentDisc.tire == null && !currentDisc.HasANutInstalled())
			{ /// If there isa disc, the disc has not a tire and the same disc has not a nut installed, install to disc.
				disc = currentDisc;
				rigidbody.isKinematic = true;
				disc.tire = this;
				disc.InstallTire();
				StartCoroutine(InterpolateTowardsDisc(disc));
			}
		}
		else rigidbody.isKinematic = false;
	}

	/// <summary>Default Hand Pick's Request Confirmation Method. Overridable for more particular functionality.</summary>
	/// <param name="_hand">Hand that requested the pick.</param>
	protected override void AcceptPickRequest(Hand _hand)
	{
		//base.AcceptPickRequest(_hand);

		if(hand == null)
		{
			hand = _hand;
			hand.pickable = this;
			hand.SetAnimationID(animationID);
		}
		else if(additionalHand == null)
		{
			additionalHand = _hand;
			additionalHand.pickable = this;
			additionalHand.SetAnimationID(animationID);
		}

		EvaluateBothHands();	
	}

	protected override void DropFromAdditionalHand()
	{
		additionalHand.SetAnimationID();
		additionalHand.pickable = null;
		additionalHand = null;
		TotalDropFromHands();
	}

	protected override void DropFromHand()
	{
		base.DropFromHand();
		TotalDropFromHands();
	}

	private void EvaluateBothHands()
	{
		if(hasBothHands)
		{
			fixedJoint.connectedBody = null;
			Destroy(fixedJoint);
			transform.parent = hand.transform;
			rigidbody.isKinematic = true;
			enabled = true;
		}
	}

	private void TotalDropFromHands()
	{
		if(hand != null)
		{
			hand.SetAnimationID();
			hand.pickable = null;
			hand = null;
		}
		if(additionalHand != null)
		{
			additionalHand.SetAnimationID();
			additionalHand.pickable = null;
			additionalHand = null;
		}
		rigidbody.isKinematic = false;
		transform.parent = null;

		enabled = false;
	}
#endregion

	private void EvaluateTire()
	{
		if(flat && skinnedMeshRenderer != null)
		{
			skinnedMeshRenderer.SetBlendShapeWeight(0, flat ? 100 : 0);
		}
	}

	private void OnNutInstalled(int _ID, bool _installed)
	{

	}

	private IEnumerator InterpolateTowardsDisc(Disc _disc)
	{
		Vector3 orgiginalPosition = transform.position;
		Quaternion originalRotation = transform.rotation;
		float n = 0.0f;

		while(n < (1.0f + Mathf.Epsilon))
		{
			transform.position = Vector3.Lerp(orgiginalPosition, _disc.transform.TransformPoint(_disc.tirePoint), n);
			transform.rotation = Quaternion.Lerp(originalRotation, _disc.transform.rotation, n);
			n += (Time.deltaTime / interpolationDuration);
			yield return null;
		}

		transform.position = _disc.transform.TransformPoint(_disc.tirePoint);
		transform.rotation = _disc.transform.rotation;
		transform.parent = _disc.transform;
		fixedJoint.connectedBody = _disc.rigidbody;
	}
}
}