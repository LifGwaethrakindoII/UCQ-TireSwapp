using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public delegate void OnNutInstalled(int _ID, bool _installed);

public class Screw : MonoBehaviour
{
	public event OnNutInstalled onNutInstalled;

	[SerializeField] private float _maxAngle; 	/// <summary>Maximum Angle.</summary>
	[SerializeField] private Vector3 _minPoint;
	[SerializeField] private Vector3 _maxPoint;
	[SerializeField] private Nut _nut;
    [SerializeField]
    [Range(0.0f, 1.0f)] private float _tightTolerance;
#if UNITY_EDITOR
    public float pointRadius;
#endif
	private RimCrossPickable _rimCross;
    private Disc _disc; 
	private int _ID;

	/// <summary>Gets maxAngle property.</summary>
	public float maxAngle { get { return _maxAngle; } }

	/// <summary>Gets minPoint property.</summary>
	public Vector3 minPoint { get { return _minPoint; } }

	/// <summary>Gets maxPoint property.</summary>
	public Vector3 maxPoint { get { return _maxPoint; } }

    /// <summary>Gets tightTolerance property.</summary>
    public float tightTolerance { get { return _tightTolerance; } }

	public RimCrossPickable rimCross
	{
		get { return _rimCross; }
		set
		{
			if(value == null && rimCross != null && rimCross.screw == null) rimCross.screw = null;
			_rimCross = value;
			if(_rimCross != null && _rimCross.screw != this)
			{
				rimCross.screw.rimCross = null;
				rimCross.screw = this;
			}
		}
	}

    /// <summary>Gets and Sets disc property.</summary>
    public Disc disc
	{
		get { return _disc; }
		set { _disc = value; }
	}

    /// <summary>Gets and Sets nut property.</summary>
    public Nut nut
	{
		get { return _nut; }
		set { _nut = value; }
	}

	/// <summary>Gets and Sets ID property.</summary>
	public int ID
	{
		get { return _ID; }
		set { _ID = value; }
	}

	public float normalizedAngle { get { return (nut != null ? (nut.angle / maxAngle) : 0.0f); } }

	public bool hasRimCross { get { return rimCross != null; } }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.DrawWireSphere(transform.TransformPoint(minPoint), pointRadius);
        Gizmos.DrawWireSphere(transform.TransformPoint(maxPoint), pointRadius);
#endif
    }

	private void Awake()
	{
		if(nut != null)
		{
			nut.angle = maxAngle;
			nut.screw = this;

			InstallNut();
		}
	}

    public void RotateNut(float _deltaRotation)
	{
	    nut.UpdateProgress(_deltaRotation);
        nut.transform.rotation = GetLerpedRotation();
	    nut.transform.position = GetLerpedPosition();

		if(HasNutInstalled() && onNutInstalled != null) onNutInstalled(ID, true);
    }

	public Quaternion GetLerpedRotation()
	{
		return Quaternion.Euler(new Vector3(
			transform.eulerAngles.x,
			transform.eulerAngles.y,
			Mathf.Lerp(0.0f, maxAngle, normalizedAngle)
		));
	}

	public void InstallNut()
	{
		if(nut != null)
		{
			nut.angle = maxAngle;
			nut.transform.position = GetLerpedPosition();
        	nut.transform.rotation = GetLerpedRotation();
        	nut.transform.parent = transform;
        	nut.rigidbody.isKinematic = true;
		}
	}

	public Vector3 GetLerpedPosition()
	{
		return Vector3.Lerp(transform.TransformPoint(minPoint), transform.TransformPoint(maxPoint), normalizedAngle);
	}

	public bool HasNutInstalled()
	{
		return (normalizedAngle >= (1.0f - tightTolerance));
	}

	public bool CanRotate(float _deltaRotation)
	{
		//return (normalizedAngle > 0.0 && normalizedAngle <= (1.0f - tightTolerance));
		return nut != null && (NormalizedAngleProjection(_deltaRotation) >= 0.0 && NormalizedAngleProjection(_deltaRotation) <= 1.0f);
	}

	public float NormalizedAngleProjection(float _deltaRotation)
	{
		return ((nut.angle + _deltaRotation) / maxAngle);
	}


	public bool CanConnectRimCross()
	{
		return
			disc != null ?
				nut != null ?
					disc.IsDiscRisenEnough() : false : false; 
	}

	public bool CanPickNut()
	{
		return rimCross == null;
	}
}
}