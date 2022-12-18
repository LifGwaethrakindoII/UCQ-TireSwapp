using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public delegate void OnTireInstalled();

public class Disc : MonoBehaviour
{
	private const float DISTANCE_REFERENCE = 25.0f;

	public event OnTireInstalled onTireInstalled;

	[SerializeField] LayerMask _floorLayer;
	[SerializeField] private Screw[] _screws;
	[SerializeField] private Vector3 _tirePoint;
	[SerializeField] private float _distanceToBeRisen;
	[SerializeField] private Tire _tire;
	private Rigidbody _rigidbody;
	private FixedJoint _fixedJoint;
#if UNITY_EDITOR
	public float radius;
#endif

	public LayerMask floorLayer { get { return _floorLayer; } }

	public Screw[] screws { get { return _screws; } }

	public Vector3 tirePoint { get { return _tirePoint; } }

	public float distanceToBeRisen { get { return _distanceToBeRisen; } }

	public Tire tire
	{
		get { return _tire; }
		set
		{
			if(_tire != null) _tire.onContactWithFloor -= OnContactWithFloor;
			_tire = value;
			if(_tire != null) _tire.onContactWithFloor += OnContactWithFloor;
			//else _tire.onContactWithFloor -= OnContactWithFloor;
		}
	}

	public Rigidbody rigidbody
	{
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
			return _rigidbody;
		}
	}

	public FixedJoint fixedJoint
	{
		get
		{
			if(_fixedJoint == null) _fixedJoint = GetComponent<FixedJoint>();
			return _fixedJoint;
		}
	}

	private void OnEnable()
	{
		if(screws != null)
		foreach(Screw screw in screws)
		{
			screw.onNutInstalled += OnNutInstalled;
		}
		if(tire != null) tire.onContactWithFloor += OnContactWithFloor;
	}

	private void OnDisable()
	{
		if(screws != null)
		foreach(Screw screw in screws)
		{
			screw.onNutInstalled -= OnNutInstalled;
		}
		if(tire != null) tire.onContactWithFloor -= OnContactWithFloor;
	}

	/// <summary>
	/// Callback to draw gizmos that are pickable and always drawn.
	/// </summary>
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.TransformPoint(tirePoint), radius);
	}

	private void Awake()
	{
		if(screws != null)
		for(int i = 0; i < screws.Length; i++)
		{
			screws[i].ID = i;
			screws[i].disc = this;	
		}

		InstallTire();
	}

	public bool AllScrewsTightWithTire()
	{
		if(screws != null)
		foreach(Screw screw in screws)
		{
			if(!screw.HasNutInstalled()) return false;
		}

		return tire != null;
	}

	public bool HasAllNutsInstalled()
	{
		if(screws != null)
		{
			foreach(Screw screw in screws)
			{
				if(!screw.HasNutInstalled()) return false;
			}
		}
		else return false;

		return true;
	}

	public bool HasANutInstalled()
	{
		if(screws != null)
		{
			foreach(Screw screw in screws)
			{
				if(screw.nut != null) return true;
			}
		}
		else return false;

		return false;
	}

	public bool TireInstalled()
	{
		return tire != null ? (HasAllNutsInstalled() && !tire.flat && tire.onFloor) : false;
	}

	private void OnNutInstalled(int _ID, bool _installed)
	{
		if(TireInstalled() && onTireInstalled != null) onTireInstalled();
	}

	private void OnContactWithFloor(bool _contact)
	{
		if(_contact && onTireInstalled != null) onTireInstalled();
	}

	public void InstallTire()
	{
		if(tire != null)
		{
			tire.disc = this;
			tire.collider.isTrigger = false;
			tire.transform.position = transform.TransformPoint(tirePoint);
			tire.transform.rotation = transform.rotation;
			tire.transform.parent = transform;
			if(tire.fixedJoint != null) tire.fixedJoint.connectedBody = rigidbody;
		}
	}

	public bool IsDiscRisenEnough()
	{
		Ray ray = new Ray(transform.position, Vector3.down * DISTANCE_REFERENCE);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, DISTANCE_REFERENCE, floorLayer))
		{
			Vector3 direction = (hit.point - transform.position);
			float distance = direction.magnitude;
			Debug.DrawRay(ray.origin, ray.direction.normalized * distance, Color.red, 15.0f);
			return direction.sqrMagnitude >= (distanceToBeRisen * distanceToBeRisen);
		}
		else return true;
	}
}
}