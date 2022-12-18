using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace UrielChallenge
{
public class User : MonoBehaviour
{
	[SerializeField] private Transform _eye; 					/// <summary>Camera's Eye.</summary>
	[SerializeField] private Hand _leftHand; 					/// <summary>User's Left Hand.</summary>
	[SerializeField] private Hand _rightHand; 					/// <summary>User's Right Hand.</summary>
	[Space(5f)]
	[Header("UI:")]
	[SerializeField] private UserFeedbackUI _userFeedbackUI; 	/// <summary>UserFeedback's UI.</summary>
	[Space(5f)]
	[Header("KILL YOURSELVES:")]
	[SerializeField] private GameObject canvas3D; 				/// <summary>TEST 3D Canvas.</summary>
	[SerializeField] private Vector3 canvasSpawnOffset; 		/// <summary>Description.</summary>
	[SerializeField] private float upOffset; 					/// <summary>Description.</summary>
	[SerializeField] private float canvasOffsetScalar; 			/// <summary>Description.</summary>
	private Gender _gender; 									/// <summary>User's Gender.</summary>
	private Vector3 normal;
	private Vector3 forward;

	/// <summary>Gets userFeedbackUI property.</summary>
	public UserFeedbackUI userFeedbackUI { get { return _userFeedbackUI; } }

	/// <summary>Gets eye property.</summary>
	public Transform eye { get { return _eye; } }

	/// <summary>Gets leftHand property.</summary>
	public Hand leftHand { get { return _leftHand; } }

	/// <summary>Gets rightHand property.</summary>
	public Hand rightHand { get { return _rightHand; } }

	/// <summary>Gets and Sets gender property.</summary>
	public Gender gender
	{
		get { return _gender; }
		set { _gender = value; }
	}

	void OnEnable()
	{
		leftHand.user = this;
		rightHand.user = this;
	}

	private void Awake()
	{
		//...
	}

	private void Update()
	{
		CalculateNormals();
	}

	public void OnApplicationMenu(bool _active)
	{
		canvas3D.SetActive(_active);
		if(_active)
		{
			Vector3 spawnPosition = (forward * canvasOffsetScalar) + (Vector3.up * upOffset);
			canvas3D.transform.position = eye.position + spawnPosition;
			canvas3D.transform.LookAt(eye.position);
		}
	}

	private void TrackInput()
	{
		
	}

	private void CalculateNormals()
	{
		forward = Vector3.Cross(Vector3.up, eye.right);
		normal = forward + Vector3.up + eye.right;
	}

	/// <returns>String representing this User's Information.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("User's Information: ");
		builder.Append("\n Gender: ");
		builder.Append(gender.ToString());

		return builder.ToString();
	}
}
}