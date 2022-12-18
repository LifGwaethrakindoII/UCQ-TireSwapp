using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UrielChallenge;

[RequireComponent(typeof(Hand))]
[RequireComponent(typeof(LineRenderer))]
public class VRUIEventSystem : MonoBehaviour
{
	[SerializeField] private LayerMask _layer; 		/// <summary>GUI's Layer.</summary>
	[SerializeField] private float _rayDistance; 	/// <summary>Ray's Distance.</summary>
	private Button3D _selectable; 					/// <summary>Current Button3D.</summary>
	private LineRenderer _lineRenderer; 			/// <summary>LineRenderer's Component.</summary>
	private Hand _hand; 							/// <summary>Hand's Component.</summary>

	/// <summary>Gets and Sets layer property.</summary>
	public LayerMask layer
	{
		get { return _layer; }
		set { _layer = value; }
	}

	/// <summary>Gets and Sets rayDistance property.</summary>
	public float rayDistance
	{
		get { return _rayDistance; }
		set { _rayDistance = value; }
	}

	/// <summary>Gets and Sets selectable property.</summary>
	public Button3D selectable
	{
		get { return _selectable; }
		set { _selectable = value; }
	}

	/// <summary>Gets and Sets lineRenderer Component.</summary>
	public LineRenderer lineRenderer
	{ 
		get
		{
			if(_lineRenderer == null)
			{
				_lineRenderer = GetComponent<LineRenderer>();
			}
			return _lineRenderer;
		}
	}

	/// <summary>Gets and Sets hand Component.</summary>
	public Hand hand
	{ 
		get
		{
			if(_hand == null)
			{
				_hand = GetComponent<Hand>();
			}
			return _hand;
		}
	}

	private void OnEnable()
	{
		lineRenderer.enabled = true;
	}

	private void OnDisable()
	{
		lineRenderer.enabled = false;
	}

	private void Update()
	{
		CheckGUI();
		CheckInput();
	}

	private void CheckGUI()
	{
		Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, rayDistance, layer))
        {
            selectable = hit.transform.GetComponent<Button3D>();
            if(selectable != null)
            {
                selectable.OnPointerEnter(null);
            }
        } else if(selectable != null)
        {
            selectable.OnPointerExit(null);
            selectable.OnPointerUp(null);
            selectable = null;
        }

        lineRenderer.SetPosition(0, transform.position);
        //lineRenderer.SetPosition(1, (ray.direction * (hit.transform != null ? hit.distance : rayDistance)));
        lineRenderer.SetPosition(1, (ray.origin + ray.direction * rayDistance));
	}

	private void CheckInput()
	{
		if(selectable != null)
		{
			if(hand.GetDevice().GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
			{
				Debug.Log("Push");
				selectable.OnPointerDown(null);
			}
			//else if(hand.GetDevice().GetPress(SteamVR_Controller.ButtonMask.Trigger))
			else if(hand.GetDevice().GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) selectable.OnPointerUp(null);
		}
	}
}
