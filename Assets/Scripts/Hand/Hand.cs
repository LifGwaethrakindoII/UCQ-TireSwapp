using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using VoidlessUtilities;

/// <summary>
/// PUTO EL QUE LO LEA (Nice, now I have your attention)
/// NOTES:
/// Hi, these are some anotations I have to make regarding the code:
/// 	- The User should be the one checking for the Hands. Because the User has reference to both Right and Left hands, so it
/// 	can apply additional logic. A Hand by itself has no other reference to the other hand.
/// 	- Properties (Getters and Setters) should be, at least by how I apply it, after the attributes' declarations.
/// 	- The Hand should be decoupled of the specific type of pickable (whether if it is CrossRim/RimCross, HydraulicJack, etc.).
/// 	The Hand only has a reference to self-contained components (SteamVR_TrackedObject for example) and an abstract Pickable
/// 	reference.
/// 
/// Specific attributes that should have the User, not the Hand:
/// 	- shouldTeleport: Which one teleports? The User or a single Hand?
/// 	- cameraRigTransform: If the user checks the Hands, do both Hands need info about the Camera's Transform?\
/// 	- headTransform: The same logic applied to the cameraRigTransform.
/// Lif Gwaethrakindo
/// </summary>

namespace UrielChallenge
{
public class Hand : MonoBehaviour
{
	public const string TAG_HANDLER = "Handler"; 						/// <summary>Handler's Tag.</summary>
	public const string ANIMATOR_KEY_HAND_ANIMATION = "Hand_Grip"; 		/// <summary>Hand's Grip Animation Key.</summary>
	public const int ANIMATION_ID_HAND_ANIMATION_DEFAULT = 0; 			/// <summary>Default Hand's Animation ID.</summary>
	public const int ANIMATION_ID_HAND_ANIMATION_DEFAULT_PICK = 1; 		/// <summary>Default Hand's Grip Animation ID.</summary>
	public const float DISTANCE_TELEPORT_RAY = 100.0f; 					/// <summary>Teleport Ray's Distance.</summary>

	[SerializeField] private Animator _animator; 	/// <summary>Hand's Animator.</summary>
	private User _user;
	private Pickable _pickable; 	/// <summary>Hand's Pickable.</summary>
	private Handler _handler; 		/// <summary>Hand's Handler.</summary>
	private SteamVR_TrackedObject _trackedObject;
	private VRUIEventSystem _eventSystem;
    private bool menuActivated;
    
	public float ratio;
    //public GameObject _canvasMenu;
	public LayerMask layer;

	public Transform cameraRigTransform; 
    public GameObject teleportReticlePrefab;
    private GameObject reticle;
    private Transform teleportReticleTransform; 
    public Transform headTransform; 
    public Vector3 teleportReticleOffset; 
    public LayerMask teleportMask; 
    public bool shouldTeleport; 

	private Vector3 hitPoint;
	private int _constraintSourceIndex;

#if UNITY_EDITOR
	public Color color;
#endif

#region Getters/Setters:
	/// <summary>Gets and Sets animator Component.</summary>
	public Animator animator
	{ 
		get
		{
			if(_animator == null)
			{
				_animator = GetComponent<Animator>();
			}
			return _animator;
		}
		set { _animator = value; }
	}

	/// <summary>Gets and Sets user property.</summary>
	public User user
	{
		get { return _user; }
		set { _user = value; }
	}

	/// <summary>Gets and Sets handler property.</summary>
	public Handler handler
	{
		get { return _handler; }
		set { _handler = value; }
	}

	/// <summary>Gets and Sets pickable property.</summary>
	public Pickable pickable
	{
		get { return _pickable; }
		set { _pickable = value; }
	}

	/// <summary>Gets and Sets trackedObject Component.</summary>
	public SteamVR_TrackedObject trackedObject
	{ 
		get
		{
			if(_trackedObject == null)
			{
				_trackedObject = GetComponent<SteamVR_TrackedObject>();
			}
			return _trackedObject;
		}
	}

	/// <summary>Gets and Sets eventSystem Component.</summary>
	public VRUIEventSystem eventSystem
	{ 
		get
		{
			if(_eventSystem == null)
			{
				_eventSystem = GetComponent<VRUIEventSystem>();
			}
			return _eventSystem;
		}
	}

	public int constraintSourceIndex
	{
		get { return _constraintSourceIndex; }
		set { _constraintSourceIndex = value; }
	}
#endregion

#region UnityCallbacks:
	/// <summary>
	/// Callback to draw gizmos that are pickable and always drawn.
	/// </summary>
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, ratio);
	}

	private void Awake()
	{
		eventSystem.enabled = false;
		pickable = null;
	}

    private void Start()
    {
        menuActivated = false;

		reticle = Instantiate(teleportReticlePrefab);
		teleportReticleTransform = reticle.transform;
		reticle.SetActive(false);
    }

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(GetDevice() != null) TrackInput();
	}
#endregion

	/// <returns>Hand's VR Device [If it has one].</returns>
	public SteamVR_Controller.Device GetDevice()
	{
		SteamVR_Controller.Device device = null;

		try { device = SteamVR_Controller.Input((int)trackedObject.index); }
		catch(Exception exception) { Debug.LogWarning("[Hand] Warning with SteamVR_Controller's Device: " + exception.Message); }

		return device;
	}

	private void TrackInput()
	{
		if(GetDevice().GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) CastTeleportRay();
		if(GetDevice().GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && shouldTeleport) Teleport();
        if(GetDevice().GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu)) ToggleCanvasMenu();
		if(GetDevice().GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && pickable == null) CheckPickables();
		if(GetDevice().GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) OnRelease();
		if(GetDevice().GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu)) ToggleEventSystem();
	}

	private void CheckPickables()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, ratio, layer);

		if(colliders != null && colliders.Length > 0)
		{
			foreach(Collider collider in colliders)
			{
				if(collider.gameObject.tag == TAG_HANDLER)
				{
					Handler currentHandler = collider.GetComponent<Handler>();
					if(currentHandler != null) currentHandler.OnPick(this);
				}
				else
				{
					Pickable currentPickable = collider.GetComponent<Pickable>();
					if(currentPickable != null) currentPickable.OnPicked(this);
				}

				if(pickable != null || handler != null) break;
			}
				
		} else SetAnimationID(ANIMATION_ID_HAND_ANIMATION_DEFAULT_PICK);
	}

	private void OnRelease()
	{
		if(handler != null) handler.OnDrop(this);
		if(pickable != null) pickable.OnDropped(this);
		else SetAnimationID();
	}

	private void ToggleCanvasMenu()
	{
		//menuActivated = !menuActivated;
        //_canvasMenu.SetActive(menuActivated);
	}

	private void CastTeleportRay()
	{
		RaycastHit hit;

		if(Physics.Raycast(trackedObject.transform.position, transform.forward, out hit, DISTANCE_TELEPORT_RAY, teleportMask))
		{
			hitPoint = hit.point;
			reticle.SetActive(true);
			teleportReticleTransform.position = hitPoint + teleportReticleOffset;
		}
		else reticle.SetActive(false);
	}

	private void ToggleEventSystem()
	{
		eventSystem.enabled = !eventSystem.enabled;
		user.OnApplicationMenu(eventSystem.enabled);
	}

	private void Teleport()
	{
		Vector3 difference = cameraRigTransform.position - headTransform.position;

		shouldTeleport = false;
		reticle.SetActive(false);
		difference.y = 0;
		cameraRigTransform.position = hitPoint + difference;
	}

	public void SetAnimationID(int _ID = ANIMATION_ID_HAND_ANIMATION_DEFAULT)
	{
		if(animator != null) animator.SetInteger(Hand.ANIMATOR_KEY_HAND_ANIMATION, _ID);
	}
}
}