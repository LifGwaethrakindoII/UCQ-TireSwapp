using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public class Nut : Pickable
{
    private const string MESSAGE_CANNOT_PICK_NUT = "Cannot Pick Nut. It is tight on screw.";
    private const string MESSAGE_CANNOT_PUT_SCREW = "Cannot put Nut. Screws have a nut already.";

    [Range(0.0f, 1.0f)] public float minValueToBeTight;
    public LayerMask screwLayer;
    public float lerpDuration;
    public float checkRadius;
	private Screw _screw;
    public float angle;

    public bool onScrew { get { return screw != null; } }

    public Screw screw
    {
        get { return _screw; }
        set
        {
            _screw = value;
            if(screw != null) PutOnScrew();
            else angle = 0.0f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        //angle = 0.0f;
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
        if(!IsTight() || (!IsTight() && screw != null && screw.CanPickNut()))
		{ 
            // If the nut is not tighten on a screw, it has a screw reference, and its
            //screw allows the nut to be pickable, let the nut be picked.
    		AcceptPickRequest(_hand);
            
		}
        else UserFeedbackUI.Instance.ShowMessage(MESSAGE_CANNOT_PICK_NUT);
	}

	/// <summary>Callback invoked when this Pickable is dropped.</summary>
	/// <param name="_hand">Hand that dropped this Pickable.</param>
	public override void OnDropped(Hand _hand)
	{
        if(screw == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius, screwLayer);

            if(colliders != null && colliders.Length > 0)
            {
                foreach(Collider collider in colliders)
                {
                    Screw currentScrew = collider.GetComponent<Screw>();
                    if(currentScrew != null && currentScrew.nut == null)
                    {
                        angle = 0.0f;
                        screw = currentScrew;
                        screw.nut = this;
                        TurnGravity(false);
                        StartCoroutine(LerpTowardsScrew());
                        break;
                    }
                }

                if(screw == null)
                {
                    UserFeedbackUI.Instance.ShowMessage(MESSAGE_CANNOT_PUT_SCREW);
                    DropFromHand();
                }
            } else TurnGravity(true);
        } else TurnGravity(true);

        if(hand != null) DropFromHand();
	}

    /// <summary>Default Hand Pick's Request Confirmation Method. Overridable for more particular functionality.</summary>
    /// <param name="_hand">Hand that requested the pick.</param>
    protected override void AcceptPickRequest(Hand _hand)
    {
        base.AcceptPickRequest(_hand);
        transform.parent = hand.transform;
        TurnGravity(false);

        if(onScrew)
        {
            screw.nut = null;
            screw = null;
        }
        
    }

    /// <summary>Default Hand Drop Execution. Overridable for more particular functionality.</summary>
    protected override void DropFromHand()
    {
        base.DropFromHand();
        transform.parent = null;
    }
#endregion

    private void TurnGravity(bool _turn)
    {
        rigidbody.useGravity = _turn;
        rigidbody.isKinematic = !_turn;
    }

    public void UpdateProgress(float _deltaRotation)
    {
        angle = (screw != null) ? Mathf.Clamp(angle + _deltaRotation, 0.0f, screw.maxAngle) : 0.0f;
    }

    public bool IsTight()
    {
        return screw != null ? screw.normalizedAngle >= minValueToBeTight : false;
    }

    private void PutOnScrew()
    {
        transform.position = screw.GetLerpedPosition();
        transform.rotation = screw.GetLerpedRotation();
        transform.parent = screw.transform;
    }

    private IEnumerator LerpTowardsScrew()
    {
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;
        float n = 0.0f;
        //angle = 0.0f;

        while(n < 1.0f + Mathf.Epsilon)
        {
            transform.position = Vector3.Lerp(originalPosition, screw.GetLerpedPosition(), n);
            transform.rotation = Quaternion.Lerp(originalRotation, screw.GetLerpedRotation(), n);

            n += (Time.deltaTime / lerpDuration);

            yield return null;
        }

        PutOnScrew();
    }
}
}